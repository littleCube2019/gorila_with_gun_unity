using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Boss3 : EnemyBase
{
     [System.Serializable]
    public struct Boss3Args
    {
        public  Gradient hpGradient ; 
        public float GenerateZombie_CoolDown;
        [Header("zombie")]
        public int GenerateZombie_Num;
        public int GenerateZombie_Waves;
        public float GenerateZombie_interval;
        [Header("missile")]
        public float GenerateMissile_CoolDown;
        public int GenerateMissile_Num;
        public float GenerateMissile_interval;
        [Header("hand")]
        public float FireHand_CoolDown ;
      
        public string bossName;
    }
    public Boss3Args boss3Args;
  
    bool readyGenerateMissile = false;
    
    bool readyGenerateZombie = false;
    
    bool readyFireHand = false;
   // bool readyBrownSkill = false;



    Gradient hpGradient ; 
    GameObject hpBarObj ;


    Slider hpBar ;    
    Image hpFill ;

    bezier_curve h_line ;
    bezier_curve v_line ;
    
    
    public GameObject head;
    public Transform mouth;
    public List<Transform> ears;
    TextMeshProUGUI barText;
 
      //new Vector2(15.25f,-3.65f);
    public override void SetData(EnemyType type)
    {
        base.SetData(type);
     
        ScriptableBoss3Enemy scriptable = (ScriptableBoss3Enemy)ResourceSystem.Instance.GetEnemyData(type);
        boss3Args = scriptable.boss3Args;
        hpGradient = boss3Args.hpGradient;
        
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
        Utils.Instance.SetTimer(()=>{ readyGenerateMissile = true; } , 0.1f*boss3Args.GenerateZombie_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyGenerateZombie = true; } , 0.1f*boss3Args.GenerateMissile_CoolDown);
        Utils.Instance.SetTimer(()=>{ readyFireHand = true; } , boss3Args.FireHand_CoolDown);

      

        h_line = GameObject.Find("h_line").transform.GetComponent<bezier_curve>();
        v_line = GameObject.Find("v_line").transform.GetComponent<bezier_curve>();

        FireHand();
        FireHand();
        hpBarObj = GameObjectPlacement.Instance.bossHpBarObj;
        barText = GameObjectPlacement.Instance.levelProgressBarText;
        hpBar = hpBarObj.GetComponent<Slider>();
        hpBar.maxValue = health;
        hpBar.value = health;
        hpFill = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        hpFill.color = hpGradient.Evaluate(hpBar.normalizedValue);
      
        barText.text = boss3Args.bossName;
    }

    IEnumerator zombie_generate(){
       
       
        
        for(int i = 0 ; i < boss3Args.GenerateZombie_Waves ; i++ ){
           // effectManager.Instance.explode(mouth.position);
            

            for(int j = 0 ; j < boss3Args.GenerateZombie_Num ; j++){
                int scale  =  UnityEngine.Random.Range(1, 3)  ;
                Vector2 force = scale*Vector2.left;
                float r = UnityEngine.Random.Range(-10f, 10f) ;
                force = Quaternion.AngleAxis(r, Vector3.forward) * force;
               
                monster_generator.Instance.generateEnemy(EnemyType.zombie , mouth.position , force );
            }
            yield return new WaitForSeconds(boss3Args.GenerateZombie_interval);
        }

       
    }

    IEnumerator missile_generate(){
         for(int j = 0 ; j < boss3Args.GenerateMissile_Num ; j++){
                int r = UnityEngine.Random.Range(0,2);
                Vector3 pos = ears[r].position + UnityEngine.Random.Range(0.5f,1f) * Vector3.left;
                monster_generator.Instance.generateEnemy(EnemyType.missile , pos );

                yield return new WaitForSeconds(boss3Args.GenerateMissile_interval);
        }
    }

    void GenerateMissile(){
        StartCoroutine(missile_generate());
        //effectManager.Instance.explode(right.position);
       
    }
    
    void FireHand(){
        
        int coin = UnityEngine.Random.Range(0,1);
        float r = UnityEngine.Random.Range(0,1f);
        Vector3 pos ;
        if(coin == 0){
            pos = h_line.get_position(r);
        }
        else{
            pos = v_line.get_position(r);
        }
        monster_generator.Instance.generateEnemy( EnemyType.big_robot_arm , pos );
    }

    protected override void Update()
    {   
        base.Update();
        if(readyGenerateZombie){
            readyGenerateZombie = false;
            
            StartCoroutine(zombie_generate());
            

            Utils.Instance.SetTimer(()=>{ readyGenerateZombie = true; } , boss3Args.GenerateZombie_CoolDown);
        }

        if( readyGenerateMissile ){
           readyGenerateMissile = false;
           GenerateMissile();
           Utils.Instance.SetTimer(()=>{ readyGenerateMissile = true; } , boss3Args.GenerateMissile_CoolDown);
        }

        if(readyFireHand){
            readyFireHand = false;
            FireHand();
            Utils.Instance.SetTimer(()=>{ readyGenerateMissile = true; } , boss3Args.FireHand_CoolDown);
        }

    }

 
}
