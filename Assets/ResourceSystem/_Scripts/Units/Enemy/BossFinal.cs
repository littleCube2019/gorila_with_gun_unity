using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossFinal : EnemyBase
{
     [System.Serializable]
    public struct BossFinalArgs
    {
        public  Gradient hpGradient ; 
       
        [Header("skill 1 - shield ")]


        public float GenerateSheildZombie_CoolDown;
        public int GenerateSheildZombie_Num;
        public float GenerateSheildZombie_Interval;
        public int shieldHp;

        [Header("skill 2 - fire missile ")]

        public float FireMissile_CoolDown;
        public int GenerateMissile_Num;

        public float GenerateMissile_interval;

        [Header("skill 3 - fire hands ")]
        public float FireHand_CoolDown;
        public int FireHand_num;

        [Header("skill 4 throw bomb")]
        public float ThrowBomb_CoolDown;
        public int ThrowBomb_num;

        public string bossName;
        
    }
    int fireHandAnimationNum = 6;
    public BossFinalArgs bossFinalArgs;
    
    
    public Transform bombTrail;  
    moving_path bombTrailCurve;
    bool readyFireHand =false;
    
    bool readyGenerateSheildZombie = false;

    bool readyStartGenerating = false;
    
    bool readyFireMissile = false;

    bool readyThrowBomb = false;

    bool readySkill = true;

    bool UseShield = false;
   // bool readyBrownSkill = false;

    int fire_hands_num = 0;

    Gradient hpGradient ; 
    GameObject hpBarObj ;
    Slider hpBar ;    
    Image hpFill ;

    public Animator FinalBossAnimator; 
    public GameObject hand;
    
    int shieldHealth = 0 ; 
    //public Transform mouth;
    //public List<Transform> ears;
    TextMeshProUGUI barText;
    
    Material hand_material ;
    Color hand_tintColor;
      //new Vector2(15.25f,-3.65f);

    // IEnumerator flash(){
    //     bool flag = true;
    //     while(true){
    //         if(flag){
    //             Color tmp = GetComponent<SpriteRenderer>().color;
    //             tmp.a = 1 ;
    //             GetComponent<SpriteRenderer>().color = tmp;
    //         }
    //         else{
    //             Color tmp = GetComponent<SpriteRenderer>().color;
    //             tmp.a = 0 ;
    //             GetComponent<SpriteRenderer>().color = tmp;
    //         }
    //         flag ^= true ;
    //         yield return new WaitForSeconds(0.5f);
    //     }
    // }

    public override void SetData(EnemyType type)
    {
        base.SetData(type);
        //StartCoroutine(flash());
        ScriptableBossFinalEnemy scriptable = (ScriptableBossFinalEnemy)ResourceSystem.Instance.GetEnemyData(type);
        bossFinalArgs = scriptable.bossFinalArgs;
        hpGradient = bossFinalArgs.hpGradient;
        
    }

    public void  _DestroyBoss(){
        hpBarObj.SetActive(false);
        level_manager.Instance.DestroyAllEnemy();
    
        base._Destroy();
    }
    public override void TakeDamage(int d){

         
            tint_color = material.GetColor("_tint");
            tint_color.a = 0 ;
        if(true){//!ishurting
            hpBar.value = health;
            hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
            
            
            ishurting = true;
       
            Utils.Instance.SetTimer(()=>{ishurting = false;}, hurtingCoolDown );
            
            if(!UseShield){
                health = Mathf.Max( 0 , health-d);
                FinalBossAnimator.SetTrigger("IsHurting");
                base.tint_color.a = 1 ;
                hand_tintColor.a = 1;
            }
            else{
                shieldHealth -= d;
                print(shieldHealth);
                FinalBossAnimator.SetTrigger("ShieldIsHurting");
            }
              
            
            if(health <= 0){
                _DestroyBoss();
            }
            if(UseShield && shieldHealth <= 0 ){
                UseShield = false;
                FinalBossAnimator.SetBool("UseShield", UseShield);
                readySkill = true;
                Utils.Instance.SetTimer(()=>{ readyFireHand = true; } , bossFinalArgs.GenerateSheildZombie_CoolDown);
            }
        }
                
        
    }

    protected override void Start(){
        Utils.Instance.SetTimer(()=>{ readyFireHand = true; } , 0.1f*bossFinalArgs.FireHand_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyStartGenerating = true; } , 0.1f*bossFinalArgs.GenerateSheildZombie_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyFireMissile = true; } , 0.1f*bossFinalArgs.FireMissile_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyThrowBomb = true; } , 0.1f*bossFinalArgs.ThrowBomb_CoolDown);

        hand_material = hand.transform.GetComponent<SpriteRenderer>().material;   
        hand_tintColor = hand_material.GetColor("_tint");
        hand_tintColor.a = 0 ;
        
        hpBarObj = GameObjectPlacement.Instance.bossHpBarObj;
        barText = GameObjectPlacement.Instance.levelProgressBarText;
        hpBar = hpBarObj.GetComponent<Slider>();
        hpBar.maxValue = health;
        hpBar.value = health;
        hpFill = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
      
        barText.text = bossFinalArgs.bossName;

        Vector3 player_pos = Player.position;
        bombTrailCurve = bombTrail.GetComponent<moving_path>();
        Vector3 hand_pos = bombTrailCurve.get_position(0);
        bombTrailCurve.set_position(3, player_pos) ;
        bombTrailCurve.set_position(2, (player_pos * 2/3 )+ hand_pos  ) ;
        bombTrailCurve.set_position(1, (player_pos * 1/3 )+ hand_pos  ) ;
        setBombTrailHeight(5);
    }

    void setBombTrailHeight(float h){
        Vector3 pos1 = bombTrailCurve.pointers[1].position;
        pos1.y = h;
        bombTrailCurve.set_position(1, pos1  ) ;

        Vector3 pos2 = bombTrailCurve.pointers[2].position;
        pos2.y = h;
        bombTrailCurve.set_position(2,  pos2 ) ;
    }

    void FireHandsAttack(){
        float unitAngle = 95 / bossFinalArgs.FireHand_num;
         for (int i = 0 ; i < bossFinalArgs.FireHand_num  ; i++){

            Vector3 tmp = 20* Vector3.up;
            tmp = Quaternion.AngleAxis(-i*unitAngle, Vector3.forward) * tmp;   
           
            monster_generator.Instance.generateEnemy( EnemyType.gorilla_hand , Player.position + tmp);
            
                 
        }
    }
    
    IEnumerator IFireMissle(){
        
        for(int i = 0 ; i < bossFinalArgs.GenerateMissile_Num ; i ++){
            Vector3 pos = transform.position + UnityEngine.Random.Range(-0.5f, 0.5f) * Vector3.up; 
            monster_generator.Instance.generateEnemy( EnemyType.missileBullet , pos);
            yield return new WaitForSeconds(bossFinalArgs.GenerateMissile_interval);
        }

        readySkill = true;
    }

    IEnumerator IGenerateShieldZombie(){
        
        for(int i = 0 ; i < bossFinalArgs.GenerateMissile_Num ; i ++){
            
            monster_generator.Instance.generateEnemy( EnemyType.shield_zombie , transform.position + Vector3.right*3);
            yield return new WaitForSeconds(bossFinalArgs.GenerateSheildZombie_Interval);
        }

       
    }

    void GenerateSheildZombie(){
        if(readyGenerateSheildZombie){
            StartCoroutine(IGenerateShieldZombie());
            readyGenerateSheildZombie = false;
            Utils.Instance.SetTimer(()=>{ readyGenerateSheildZombie = true; } , bossFinalArgs.GenerateSheildZombie_CoolDown);
        }       
    }

    void FireMissile(){
        StartCoroutine(IFireMissle());
    }

    void ThrowBombAnimaitonFinish(){
        float rd_height = UnityEngine.Random.Range(2f,8f);
        setBombTrailHeight(rd_height);
        MovingByPathEnemy bomb =  (MovingByPathEnemy)monster_generator.Instance.generateEnemy( EnemyType.ThrowingBomb , bombTrailCurve.pointers[0].position );
        bomb.startMoving(bombTrailCurve);
       
        Utils.Instance.SetTimer(()=>{  readyThrowBomb = true;  } , bossFinalArgs.ThrowBomb_CoolDown);

        if( UnityEngine.Random.Range(0f,1f) > 0.8f ){
            
            FinalBossAnimator.SetBool("ThrowBomb", false);
            readySkill = true;
        }
    }

    void FireHandsUpAnimation(){
        //print(fire_hands_num);
        if(fire_hands_num > bossFinalArgs.FireHand_num){
            FinalBossAnimator.SetBool("FireHand", false);
            
            Utils.Instance.SetTimer(()=>{ readyFireHand = true; readySkill = true; } , bossFinalArgs.FireHand_CoolDown);
            FireHandsAttack();
            readySkill = true;
        }
        
        
       
        fire_hands_num = (fire_hands_num+1);
       
    }
   


    protected override void Update()
    {   
        base.Update();
        hand_tintColor.a = Mathf.Clamp01(tint_color.a - Time.deltaTime);
        hand_material.SetColor("_tint" , hand_tintColor  );  
        if(readySkill){
            int ch = UnityEngine.Random.Range(1,5);
           
   
            if( readyThrowBomb && ch == 1){
                readySkill = false;
                readyThrowBomb = false;
                FinalBossAnimator.SetBool("ThrowBomb", true);

            }
           
            if(readyStartGenerating && ch == 2){
                readySkill = false;
                readyStartGenerating = false;
                
                readyGenerateSheildZombie = true;
                shieldHealth = bossFinalArgs.shieldHp;
                UseShield = true;
                FinalBossAnimator.SetBool("UseShield", true);
                // readyStartGenerating will set to true after finish animation !
            }
            
            
            if(readyFireHand && ch == 3){
                readyFireHand = false;
                readySkill = false;
            
                fire_hands_num = 0;
                FinalBossAnimator.SetBool("FireHand", true);
                
                
                //FireHandsUpAnimationBack();
                //Utils.Instance.SetTimer(()=>{ readyFireHand = true; } , bossFinalArgs.FireHand_CoolDown);
            }
         
            if( readyFireMissile && ch == 4){
                readySkill = false;
                readyFireMissile = false;
                //FireMissileAnimation();
                FireMissile();
                //FireMissileAnimationBack();
                
                Utils.Instance.SetTimer(()=>{ readyFireMissile = true;  } , bossFinalArgs.FireMissile_CoolDown);
            
            }

        
        }
     }

 
}
