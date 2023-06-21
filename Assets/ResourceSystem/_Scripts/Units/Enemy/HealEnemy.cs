using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEnemy : MeleeEnemy
{
     [System.Serializable]
    public struct HealArgs
    {
        public int hp_heal;
    
        
    }
    public HealArgs healArgs;

    //bool isground ;
    //bool ishurting;

    //float hurtingCoolDown = 0.5f;  

    public override void SetData(EnemyType type)
    {
        base.SetData(type); // -->Melee, and --> its base (EnemyBase) !!1
        ScriptableHealEnemy scriptable = (ScriptableHealEnemy)ResourceSystem.Instance.GetEnemyData(type);
        healArgs = scriptable.healArgs;
        

    }

    
    new void Start(){
       Vector2 pos = FindObjectOfType<Boss1>().transform.position;
       if(pos != null){
          base.target = new Vector3(8f,3f,0); 
       }
       
       
    }
    
    
    new void Update(){
       Vector2 pos = FindObjectOfType<Boss1>().transform.position;
       if(pos == null){  
          base.target = new Vector3(8f,3f,0); 
       }
       else{
          base.target = pos;
       }
       base.Update();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.transform.GetComponent<Boss1>() == null) return ;
    
        effectManager.Instance.heal(transform.position + 1.5f* Vector3.right , Vector3.one*2);
        Collider2D[] hits;
        hits = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            if(hit.gameObject.GetComponent<Boss1>()!=null ){

                
                hit.GetComponent<Boss1>().heal( healArgs.hp_heal );
            }   
        }  

        Destroy(gameObject);
    }

    
    public override void TakeDamage(int d){
        
        if(true){ //!ishurting
            health -= d;

            ishurting = true;
   
            Utils.Instance.SetTimer(()=>{ishurting = false;}, hurtingCoolDown );
           
        
            base.tint_color.a = 1;
            if(health <= 0){
                _Destroy();
            }
        }
    }

  
 
}
