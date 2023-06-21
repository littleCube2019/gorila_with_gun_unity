using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Boss1 : EnemyBase
{
     [System.Serializable]
    public struct Boss1Args
    {
        public  Gradient hpGradient ; 
        public float generateSkill_CoolDown;
        public int generateCircleMissile_Num;
        
        // random missile
        public float generateRandomMissile_CoolDown;
        public int generateRandomMissile_Num;

        public int generateRandomFastMissile_Num;
        
        // random heal missile
        public float generateRandomHealMissile_CoolDown;
        public int generateRandomHealMissile_Num;
       
        // brown missile skill
        public int generateBrownMissile_Num;
        public string bossName;

    }
    public Boss1Args boss1Args;
  
    bool readyCircleOrBrownSkill = false;
    bool readyRandomMissleSkill = false;

    bool readyRandomHealMissileSkill = false;
   // bool readyBrownSkill = false;

    Gradient hpGradient ; 
    GameObject hpBarObj ;
    Slider hpBar ;    
    Image hpFill ;

    moving_path curve;
    
    TextMeshProUGUI barText;


  
    public override void SetData(EnemyType type)
    {   
        
        base.SetData(type);
        ScriptableBoss1Enemy scriptable = (ScriptableBoss1Enemy)ResourceSystem.Instance.GetEnemyData(type);
        boss1Args = scriptable.boss1Args;
        hpGradient = boss1Args.hpGradient;
    }

    public void  _DestroyBoss(){
        hpBarObj.SetActive(false);
        level_manager.Instance.DestroyAllEnemy();
        base._Destroy();
        
    }

    public override void EventduringHurt(){
//        print(health);
        hpBar.value = health;
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
        
        if(health <= 0){
            
            hpBar.value = 0;
            hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
            _DestroyBoss();
        }
    }

    public override void EventduringHeal(){
        hpBar.value = health;
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
    }
    protected override void Start(){
        curve = GameObject.Find("boss1_curve").GetComponent<moving_path>();  
        StartCoroutine(flying());
        
        Utils.Instance.SetTimer(()=>{ readyCircleOrBrownSkill = true; } , boss1Args.generateSkill_CoolDown * 0.3f);
        Utils.Instance.SetTimer(()=>{ readyRandomMissleSkill = true; } , boss1Args.generateRandomMissile_CoolDown);
        readyRandomHealMissileSkill = true;
        //Utils.Instance.SetTimer(()=>{ readyRandomHealMissileSkill = true; } , boss1Args.generateRandomHealMissile_CoolDown);
        
        
        hpBarObj = GameObjectPlacement.Instance.bossHpBarObj;
        barText = GameObjectPlacement.Instance.levelProgressBarText;
        hpBar = hpBarObj.GetComponent<Slider>();
        hpBar.maxValue = health;
        hpBar.value = health;
        hpFill = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
      
        barText.text = boss1Args.bossName;
        barText.color = new Color(0f,0f,0f);
    }

    void CircleMissile(){
        float unitAngle = 75 / boss1Args.generateCircleMissile_Num;
         for (int i = 0 ; i < boss1Args.generateCircleMissile_Num  ; i++){

            Vector3 tmp = 20* Vector3.right;
            tmp = Quaternion.AngleAxis(i*unitAngle, Vector3.forward) * tmp;   
           
            monster_generator.Instance.generateEnemy( EnemyType.missile , Player.position + tmp);
                  
                 
        }
            
    }

    void RandomMissle(){
        
        StartCoroutine(_generateMissileWave());
    }

    void GenerateHealMissle(){
        monster_generator.Instance.generateEnemy(EnemyType.HealMissile , new Vector3(-10,3.5f,0));
    }

    IEnumerator _generateMissileWave(){
       
        float r1 = Random.Range(0f,1f);
        if( r1 > 0.5f ){
            for(int i = 0; i < boss1Args.generateRandomMissile_Num ;i ++){
            
                float r = Random.Range(0f,1f);
                Vector3 pos ;
                if(r > 0.5f){
                    pos = new Vector3(Random.Range(-8f, 10f),6,0);
                }
                else{
                    pos = new Vector3(10f,Random.Range(-1.5f,6f),0);
                }
                monster_generator.Instance.generateEnemy(EnemyType.missile , pos);

                yield return new WaitForSeconds(0.5f);
            }
        }
        else{
            for(int i = 0; i < boss1Args.generateRandomFastMissile_Num ;i ++){
            
                float r = Random.Range(0f,1f);
                Vector3 pos ;
                if(r > 0.5f){
                    pos = new Vector3(Random.Range(-8f, 10f),6,0);
                }
                else{
                    pos = new Vector3(10f,Random.Range(-1.5f,6f),0);
                }
                monster_generator.Instance.generateEnemy(EnemyType.FasterMissile , pos);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void BrownMissile(){
        StartCoroutine(_generateBrownMissileWave());
    }

    IEnumerator _generateBrownMissileWave(){
        for(int i = 0; i < boss1Args.generateBrownMissile_Num ;i ++){
           
    
            monster_generator.Instance.generateEnemy(EnemyType.brownMissile , transform.position);

            yield return new WaitForSeconds(0.5f);
        }
    }


    IEnumerator flying(){
        //int counter = 0;    
        while(true){
            float t = Random.Range(0f,1f);
            Vector2 target = curve.get_position(t);
            while( Vector2.Distance( target , transform.position) > 0.01f){
                transform.position = Vector3.MoveTowards(transform.position, target, baseArgs.moveSpeed * Time.fixedDeltaTime);

                yield return new WaitForSeconds(0.1f);
            }
     
            yield return new WaitForSeconds(0.05f) ;
        }

    }



    protected override void Update()
    {   
    

        base.Update();
        if(readyCircleOrBrownSkill){
            readyCircleOrBrownSkill = false;

            float r = Random.Range(0f,1f);
            if( r > 0.5f ){
                CircleMissile();
            }
            else{
                BrownMissile();
            }

            Utils.Instance.SetTimer(()=>{ readyCircleOrBrownSkill = true; } , boss1Args.generateSkill_CoolDown);
        }

        if(readyRandomMissleSkill){
            readyRandomMissleSkill = false;
            RandomMissle();
            Utils.Instance.SetTimer(()=>{ readyRandomMissleSkill = true; } , boss1Args.generateRandomMissile_CoolDown);
        }
        
        if(readyRandomHealMissileSkill){
            readyRandomHealMissileSkill = false;
            GenerateHealMissle();
            Utils.Instance.SetTimer(()=>{ readyRandomHealMissileSkill = true; } , boss1Args.generateRandomHealMissile_CoolDown);
        }   
        

    }
 
}
