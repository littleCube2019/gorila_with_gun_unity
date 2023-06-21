using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingByPathEnemy : EnemyBase
{

      
    public moving_path path;

    
    
    //bool ishurting;

    //float hurtingCoolDown = 0.5f;  

    public void startMoving( moving_path curve){
        
        path = curve;
        
        StartCoroutine(move());
    }


    IEnumerator move(){
        for (float i = 0 ; i < 1 ;  i += 0.01f){
            transform.position = path.get_position(i);
            yield return new WaitForSeconds( 1/baseArgs.moveSpeed );
        }
    }


 
    
  


    

  
 
}
