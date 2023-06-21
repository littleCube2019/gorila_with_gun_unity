using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_boss3 : level_wave
{   
    void Start(){
        StartCoroutine(Wave());
    }
    IEnumerator Wave(){
        
        monster_generator.Instance.generateEnemy(EnemyType.boss3 , new Vector3(7f,3f,0) );
        
        while( level_manager.Instance.WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
  
}
