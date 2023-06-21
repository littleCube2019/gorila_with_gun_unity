using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBase
{
     [System.Serializable]
    public struct MeleeArgs
    {
        public float attackPower;
        public float attackRange;
        public float cooldown;

        public bool isKnockBack;
        public float knockBackSpeed;
        public float knockBackIntervel;
        public bool isFaceToPlayer;
    }
    public MeleeArgs meleeArgs;
    public Vector3 target ;
    
    public bool isKnockBacking;
    bool isground ;
    //bool ishurting;

    //float hurtingCoolDown = 0.5f;  

    public override void SetData(EnemyType type)
    {
        base.SetData(type);
        ScriptableMeleeEnemy scriptable = (ScriptableMeleeEnemy)ResourceSystem.Instance.GetEnemyData(type);
        meleeArgs = scriptable.meleeArgs;
    }

    
    new void Start(){
        target = base.Player.position;
        
        if(baseArgs.isGround){
            target.y = ground.position.y + GetComponent<BoxCollider2D>().size.y/2 ;
            if(transform.position.y > target.y){
                isground = false;
            }
            else{
                isground = true;
            }
        }
       
    }
  
    
    // Update is called once per frame
    void FixedUpdate()
    {   

        if(Vector3.Distance(transform.position, target) > 0.9f){
            if(!isKnockBacking){
                var step =  baseArgs.moveSpeed * Time.fixedDeltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
            else if( Vector3.Distance(transform.position, target) < 30f )  {
                var step =  meleeArgs.knockBackSpeed * Time.fixedDeltaTime; // calculate distance to move
                Vector3 dir = transform.position - target;
                transform.position = Vector3.MoveTowards(transform.position, target+2*dir, step);
            }
            // face to player
            
            if( meleeArgs.isFaceToPlayer ){
                Vector2 relative = new Vector2(  target.x - transform.position.x , target.y - transform.position.y ); 
                transform.rotation = Quaternion.FromToRotation(Vector3.left, relative);
            }
          
            if( baseArgs.isGround){
                if(transform.position.y > target.y){
                    isground = false;
                }
                else{
                    transform.position = new Vector3(transform.position.x, target.y, transform.position.z );
                    isground = true;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                
                if(!isground){
                GetComponent<Rigidbody2D>().velocity += Vector2.down * 0.1f ;
                }  
            }
            //Vector3 direction = player.position - transform.position;
        }
        
    }

    public override void TakeDamage(int d){
        if(health <= 0) return ;
        if(true){ //!ishurting
            health -= d;

            ishurting = true;
   
            Utils.Instance.SetTimer(()=>{ishurting = false;}, hurtingCoolDown );
            if(meleeArgs.isKnockBack && !isKnockBacking ){
                isKnockBacking = true;
                Utils.Instance.SetTimer(()=>{isKnockBacking = false;}, meleeArgs.knockBackIntervel);
            }
        
            base.tint_color.a = 1;
            if(health <= 0){
                _Destroy();
            }
        }
    }

  
 
}
