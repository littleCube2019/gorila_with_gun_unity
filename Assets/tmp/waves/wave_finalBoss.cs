using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_finalBoss : level_wave
{   
    void Start(){
        StartCoroutine(Wave());
    }
    IEnumerator Wave(){
        
        monster_generator.Instance.generateEnemy(EnemyType.BossFinal, new Vector3(7f,-3.43f,0) );
        
        while( level_manager.Instance.WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
  
}
