using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamagable
{
    [System.Serializable]
    public struct BaseArgs
    {
        public int maxHealth;
        public float moveSpeed;
        public int deadRewardNum;
        public RewardType deadRewardType;
        public bool isGround;

        //public float hurtingCoolDown ; 

    }
    [HideInInspector] public EnemyType enemyType;
    public BaseArgs baseArgs;
    protected float health;
    public Transform Player;
    public Transform ground;
    public Animator anim ;

    protected float hurtingCoolDown = 0.01f;
    protected bool ishurting ;

    protected Material material;
    protected Color tint_color;

    /*IEnumerator modifyEnemyCounter(int n){
        
        while(level_manager.Instance.isModifyingEnemy){ 
            print(gameObject.name);
            yield return 0;
        }

        level_manager.Instance.isModifyingEnemy = true;
        
        level_manager.Instance.current_level_statistic.updateEnemyCounter( enemyType , n);
       
        
        level_manager.Instance.isModifyingEnemy = false;
        print("enemy");
        print(level_manager.Instance.current_level_statistic.currentEnemyNum);
        if(n == -1){
            Destroy(gameObject);
        }

    }*/
   
    public void _Destroy( bool killedByAmmo = true  ){
        StopAllCoroutines();
        effectManager.Instance.explode(transform.position);
        if(killedByAmmo){
            reward_manager.Instance.generateRewards( baseArgs.deadRewardType  , baseArgs.deadRewardNum, transform.position);
            //Transform enemy = Instantiate(level_manager.Instance.level_statistic_type_template).transform;
            //enemy.GetComponent<level_statistic_type>().enemyType = enemyType;
            //enemy.SetParent(level_manager.Instance.Tomb);
        }
        BeforeDied();
       
       
        AudioManager.instance.Play("explosion");
        Destroy(gameObject);
        
    }
    public virtual void BeforeDied(){

    }

    public virtual void TakeDamage(int d){
        if(true){ //!ishurting
            
            ishurting = true;
            
            Utils.Instance.SetTimer(()=>{ishurting = false;}, hurtingCoolDown );
            
            health = Mathf.Max( 0 , health-d);
            
            EventduringHurt();
            // if(anim != null){   
            //     anim.SetTrigger("hurt");
            // }
         
            tint_color.a = 1;
          
           
            if(health <= 0){
                _Destroy();
            }

        }
        
        
    }

    public virtual void EventduringHeal(){

    }
    public virtual void EventduringHurt(){

    }

    protected virtual void Awake()
    {   
        ishurting = false;
        player p = FindObjectOfType<player>();
        Player = p.transform;
        anim = GetComponent<Animator>();
        ground = GameObject.FindGameObjectWithTag("ground").transform;
        
        transform.SetParent(level_manager.Instance.Enemies);



        if(GetComponent<SpriteRenderer>() != null ){
            
            material = GetComponent<SpriteRenderer>().material;
            tint_color = material.GetColor("_tint");
            tint_color.a = 0 ;
        }
        
        
        
        //material.SetTexture("main_",sprite.texture);
        
        //anim.Play("move");
    }
    protected virtual void Start()
    {
        
    }


    public virtual void SetData(EnemyType type) { 
        ScriptableEnemyBase scriptable = (ScriptableEnemyBase)ResourceSystem.Instance.GetEnemyData(type);
       
        if(scriptable.anim != null){
            anim.runtimeAnimatorController = scriptable.anim;
         }
        else{
            anim = null;
        }
        //anim = null;
        baseArgs = scriptable.baseArgs;
        health = baseArgs.maxHealth;
        enemyType = scriptable.enemyType;

       
        
    }

    protected virtual void Update()
    {   

        tint_color.a = Mathf.Clamp01(tint_color.a - Time.deltaTime);
        
        material.SetColor("_tint" , tint_color  );  
    }

    public void heal(int h)
    {   
        
        health = Mathf.Min( baseArgs.maxHealth , health+h );
        EventduringHeal();
    }
   
}
