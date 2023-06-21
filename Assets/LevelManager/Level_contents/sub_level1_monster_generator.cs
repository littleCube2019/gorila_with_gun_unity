using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class sub_level1_monster_generator : monster_generator
    // Start is called before the first frame update
{
    

    
    public List<int> zomibeNumForEachWave;
    public float zombieCoolDown = 2f;

    public int missileNumInWave2 ;
    public float missileCoolDown = 4f;
    moving_path h_line ;
    moving_path v_line ;


    
  
    // Update is called once per frame
    public override void init_level_event(){

        base.Waves = new List<Action>();
        base.Waves.Add(Wave1);
        base.Waves.Add(Wave2);
        base.Waves.Add(Wave3);
        base.numWave = Waves.Count;   

        h_line = GameObject.Find("h_line").transform.GetComponent<bezier_curve>();
        v_line = GameObject.Find("v_line").transform.GetComponent<bezier_curve>();
    }


    
    IEnumerator Wave1_(){
        bool canGenerateZombie = true;
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
        
        while( WaveNotFinish()){
            yield return 0;
        }
      
        level_manager.Instance.WaveFinish();
    }


     IEnumerator Wave2_(){
        
        for(int i = 0 ; i < missileNumInWave2 ; i++){
            float r = UnityEngine.Random.Range(0.1f,0.9f);
            generateEnemy(EnemyType.missile , h_line.get_position(r));
            yield return new WaitForSeconds(missileCoolDown);
        }
        
        for(int i = 0 ; i < missileNumInWave2 ; i++){
            float r = UnityEngine.Random.Range(0.1f,0.9f);
            generateEnemy(EnemyType.missile , v_line.get_position(r));
            yield return new WaitForSeconds(missileCoolDown);
        }
        
        while( WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }

    IEnumerator Wave3_(){
        
        generateEnemy(EnemyType.flying_zombie_generator_fake , new Vector2(6.6f, -4.77f));
        
        
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

    void Wave3(){
        StartCoroutine(Wave3_());
    }

   
}
