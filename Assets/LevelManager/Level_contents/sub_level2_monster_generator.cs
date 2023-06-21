using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class sub_level2_monster_generator : monster_generator
    // Start is called before the first frame update
{
    

    
    public List<int> zomibeNumForEachWave;

    [Header("wave 1")]
    public int zomibeWaveInWave1;
    public float zomibeWaveCoolDownInWave1;
    public float zombieCoolDown = 2f;

  
   


    
  
    // Update is called once per frame
    public override void init_level_event(){

        base.Waves = new List<Action>();
        base.Waves.Add(Wave1);
        base.Waves.Add(Wave2);
        base.numWave = Waves.Count;   
    }   


    
    IEnumerator Wave1_(){
        bool canGenerateZombie = true;

        for(int i = 0 ; i < zomibeWaveInWave1 ; i++ ){
            int curZombieNum = 0;

            while( zomibeNumForEachWave[0] > curZombieNum ){
            
                if(canGenerateZombie && (zomibeNumForEachWave[0] > curZombieNum)){
                    canGenerateZombie = false;
                    generateEnemy(EnemyType.zombie);

                    curZombieNum += 1;

                    Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                                }
                                            , zombieCoolDown );
                }
                    
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(zomibeWaveCoolDownInWave1);
        }
        while( WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }

    IEnumerator Wave2_(){
        
        generateEnemy(EnemyType.flying_zombie_generator , new Vector2(6.6f, -4.77f));
        
        
        while( WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }

    void Wave1(){
        StartCoroutine(Wave1_());
   
    }

    void Wave2(){
        StartCoroutine(Wave2_());
    }

   
}
