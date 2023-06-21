using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_zombie_generator : level_wave
{   
  
    
    void Start(){
        StartCoroutine(Wave());
    }
    IEnumerator Wave(){
        
        monster_generator.Instance.generateEnemy(EnemyType.flying_zombie_generator , new Vector2(6.6f, -4.77f));
        
        while( level_manager.Instance.WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
   
}
