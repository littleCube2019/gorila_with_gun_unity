using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class sub_level4_monster_generator : monster_generator
    // Start is called before the first frame update
{
    

    
    public List<int> BombNumForEachWave;
    public float BombCoolDown = 1f;

  
   


    
  
    // Update is called once per frame
    public override void init_level_event(){

        base.Waves = new List<Action>();
        base.Waves.Add(Wave1);
        base.Waves.Add(Wave2);
       
        base.numWave = Waves.Count;   
    }


    
    IEnumerator Wave1_(){
        bool canGenerateZombie = true;
        int curBombNum = 0;

        while( BombNumForEachWave[0] > curBombNum ){
          
            if(canGenerateZombie && (BombNumForEachWave[0] > curBombNum)){
                canGenerateZombie = false;

                int r = UnityEngine.Random.Range(0,3);
                if(r == 0){
                    generateEnemy(EnemyType.bomb_low);
                }
                else if(r == 1){
                    generateEnemy(EnemyType.bomb_mid);
                }
                else{
                    generateEnemy(EnemyType.bomb_high);
                }
                float bias = UnityEngine.Random.Range(0f,1f);
                curBombNum += 1;

                Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                            }
                                        , BombCoolDown + bias );
            }
            //print(level_manager.Instance.current_level_statistic.currentEnemyNum);
            yield return new WaitForSeconds(0.01f);
        }
        
        while( WaveNotFinish() ){
           // print(level_manager.Instance.current_level_statistic.currentEnemyNum);
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }

     IEnumerator Wave2_(){
        bool canGenerateZombie = true;
        int curBombNum = 0;

        while( BombNumForEachWave[1] > curBombNum ){
          
            if(canGenerateZombie && (BombNumForEachWave[1] > curBombNum)){
                canGenerateZombie = false;

                int r = UnityEngine.Random.Range(0,3);
                if(r == 0){
                    generateEnemy(EnemyType.bomb_low);
                }
                else if(r == 1){
                    generateEnemy(EnemyType.bomb_mid);
                }
                else{
                    generateEnemy(EnemyType.bomb_high);
                }
                
                float bias = UnityEngine.Random.Range(-0.25f,0.25f);
                curBombNum += 1;

                Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                            }
                                        , BombCoolDown + bias );
            }
                 
            yield return new WaitForSeconds(0.01f);
        }
        
        while( WaveNotFinish() ){
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
