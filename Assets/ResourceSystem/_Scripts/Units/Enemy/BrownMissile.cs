using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownMissile : EnemyBase
{
     
    public MeleeEnemy.MeleeArgs meleeArgs;
  
    Vector3 target ;
    float alpha = 1 ;
    float decay_rate = 0.9f;
    public override void SetData(EnemyType type)
    {
        base.SetData(type);
        ScriptableMeleeEnemy scriptable = (ScriptableMeleeEnemy)ResourceSystem.Instance.GetEnemyData(type);
        meleeArgs = scriptable.meleeArgs;
    }

    
    new void Start(){
       
        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-3f, 3f);
        target = new Vector3( x,y , 0 );
    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, base.Player.position) < 1f){

        }
        else if(Vector3.Distance(transform.position, target) > 0.01f ){

            
            var step =  baseArgs.moveSpeed * Time.fixedDeltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            
            Vector2 relative = new Vector2( target.x - transform.position.x , target.y - transform.position.y ); 
            
           
            transform.rotation = Quaternion.FromToRotation(Vector3.left, relative);
            alpha *= decay_rate ; 
        }
        else{
            float x = Random.Range(-7f, 8f * alpha);
            float y = Random.Range(-3f, 3f * alpha);
            target = new Vector3( x,y , 0 );
        }
        
    }

  
 
}
