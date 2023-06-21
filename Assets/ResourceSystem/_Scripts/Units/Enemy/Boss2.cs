using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Boss2 : EnemyBase
{
     [System.Serializable]
    public struct Boss2Args
    {
        public  Gradient hpGradient ; 
        public float UpFire_CoolDown;

        public int UpFire_Num;
        public int UpFire_Waves;

        public float ToPlayerFire_CoolDown;
        public int ToPlayerFire_Num;
       


      
        public string bossName;
    }
    public Boss2Args boss2Args;
  
    bool readyUpFire = false;
    bool  isUpFiring  = false;
    bool readyToPlayerFire = false;
    bool isToPlayerFiring = false;
   // bool readyBrownSkill = false;



    Gradient hpGradient ; 
    GameObject hpBarObj ;
    Slider hpBar ;    
    Image hpFill ;

    public GameObject head;
    public Transform right;
    public Transform upper;
    TextMeshProUGUI barText;
    
    protected override  void  Awake() {
        base.Awake();
        material = head.GetComponent<SpriteRenderer>().material ; 
        tint_color = material.GetColor("_tint");
        tint_color.a = 0 ;
    }


      //new Vector2(15.25f,-3.65f);
    public override void SetData(EnemyType type)
    {
        base.SetData(type);
     
        ScriptableBoss2Enemy scriptable = (ScriptableBoss2Enemy)ResourceSystem.Instance.GetEnemyData(type);
        boss2Args = scriptable.boss2Args;
        hpGradient = boss2Args.hpGradient;
        
    }

    public void  _DestroyBoss(){
        hpBarObj.SetActive(false);
        level_manager.Instance.DestroyAllEnemy();
        base._Destroy();
       
    }

     public override void EventduringHurt(){
    
        hpBar.value = health;
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
        if(health <= 0){
            hpBar.value = 0;
            hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
            _DestroyBoss();
        }
    }

    protected override void Start(){
        Utils.Instance.SetTimer(()=>{ readyUpFire = true; } , 0.1f*boss2Args.UpFire_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyToPlayerFire = true; } , 0.1f*boss2Args.ToPlayerFire_CoolDown);
        //Utils.Instance.SetTimer(()=>{ readyUpFire = true; } , boss2Args.UpFire_CoolDown);
        //Utils.Instance.SetTimer(()=>{ readyRandomMissleSkill = true; } , boss1Args.generateRandomMissile_CoolDown);
        //Utils.Instance.SetTimer(()=>{ readyBrownSkill = true; } , boss1Args.generateBrownMissile_CoolDown);

        hpBarObj = GameObjectPlacement.Instance.bossHpBarObj;
        barText = GameObjectPlacement.Instance.levelProgressBarText;
        hpBar = hpBarObj.GetComponent<Slider>();
        hpBar.maxValue = health;
        hpBar.value = health;
        hpFill = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
      
        barText.text = boss2Args.bossName;
    }

    IEnumerator rotation_head(float targetAngle , float speed){
        isUpFiring = true;
        while(Mathf.Abs(head.transform.rotation.eulerAngles.z-targetAngle) > 1f ){
            
            head.transform.RotateAround( head.transform.position, Vector3.forward, speed * Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        

        
        for(int i = 0 ; i < boss2Args.UpFire_Waves ; i++ ){
            effectManager.Instance.explode(upper.position);
            

            for(int j = 0 ; j < boss2Args.UpFire_Num ; j++){
                int scale  =  UnityEngine.Random.Range(5, 10)  ;
                Vector2 force = scale*Vector2.up;
                float r = UnityEngine.Random.Range(-10f, 10f) ;
                force = Quaternion.AngleAxis(r, Vector3.forward) * force;
                int coin = UnityEngine.Random.Range(0,3);
                EnemyType bomb_type = EnemyType.bomb_low;
                
                if (coin == 0){
                    bomb_type = EnemyType.bomb_mid;
                }
                else if(coin == 1){
                    bomb_type = EnemyType.bomb_high;
                }
             
                monster_generator.Instance.generateEnemy(bomb_type , upper.position , force );
            }
            yield return new WaitForSeconds(1f);
        }


        while(Mathf.Abs(head.transform.rotation.eulerAngles.z-0) > 1f ){
            
            head.transform.RotateAround( head.transform.position, Vector3.forward, -speed * Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        isUpFiring = false;
    }

    void ToPlayerFiring(){
        effectManager.Instance.explode(right.position);
        for(int j = 0 ; j < boss2Args.ToPlayerFire_Num ; j++){
                Vector3 pos = right.position + UnityEngine.Random.Range(0.5f,1f) * Vector3.left;
                monster_generator.Instance.generateEnemy(EnemyType.bomb_low , pos );
            }
    }
    


    protected override void Update()
    {   
        base.Update();
        if(readyUpFire){
            readyUpFire = false;
            
            StartCoroutine(rotation_head(270f , -80f));
            

            Utils.Instance.SetTimer(()=>{ readyUpFire = true; } , boss2Args.UpFire_CoolDown);
        }

        if( readyToPlayerFire && !isUpFiring ){
           readyToPlayerFire = false;
           ToPlayerFiring();
           Utils.Instance.SetTimer(()=>{ readyToPlayerFire = true; } , boss2Args.ToPlayerFire_CoolDown);
        }

        

    }

 
}
