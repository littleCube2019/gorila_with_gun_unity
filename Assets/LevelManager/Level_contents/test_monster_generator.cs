using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class test_monster_generator : monster_generator
    // Start is called before the first frame update
{
    
    IEnumerator _generateMissileWave(){
       
        
        for(int i = 0; i < 1 ;i ++){
           
            //generateEnemy(EnemyType.boss2 , new Vector2(7f,-3f));
            //generateEnemy(EnemyType.shield_zombie );
            //generateEnemy(EnemyType.big_robot_arm);
            
            yield return new WaitForSeconds(1f);
        }

        for(int i = 0 ; i < 100 ; i++){
             generateEnemy(EnemyType.bomb_low );
             //generateEnemy(EnemyType.bomb_from_sky, new Vector3(1,5,0) );
            // generateEnemy(EnemyType.bomb_high );
            // generateEnemy(EnemyType.bomb_mid );
            //generateEnemy(EnemyType.zombie);
          
             yield return new WaitForSeconds(1f);
        }
    }
    void Wave2(){
        //generateEnemy(EnemyType.boss3 , new Vector3(7f,3f,0) );
        //generateEnemy(EnemyType.big_robot_arm , new Vector3(-4f,3f,0) );
        //generateEnemy(EnemyType.big_robot_arm , new Vector3(7f,3f,0) );
        generateEnemy(EnemyType.BossFinal, new Vector3(7f,-3f,0));
    }
    public override void init_level_event(){

        base.Waves = new List<Action>();
        //base.Waves.Add(Wave1);
        base.Waves.Add(Wave2);

    }
    new void Start(){
        base.Start();
       // StartCoroutine(_generateMissileWave());
        
        
    }
    // Update is called once per frame
   
}
