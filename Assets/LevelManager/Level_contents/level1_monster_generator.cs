using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class level1_monster_generator : monster_generator
    // Start is called before the first frame update
{
    
   
    bool bossIsGenerated = false; 

    
    public List<int> zomibeNumForEachWave;
    public List<int> missileNumForEachWave;
    public float zombieCoolDown = 2f;

    public float missileCoolDown = 0.5f;

    public float missileWaveCoolDown = 10f;
    
    public int missileNumInWave = 20;

    public float beforeBossTime = 120f;
    
    
   
    public void generateMissileWave(){
        
        StartCoroutine(_generateMissileWave());
    }

    IEnumerator _generateMissileWave(){
       
        
        for(int i = 0; i < missileNumInWave ;i ++){
           
            float r = UnityEngine.Random.Range(0f,1f);
            Vector3 pos ;
            if(r > 0.5f){
                pos = new Vector3(UnityEngine.Random.Range(-7f, 10f),6,0);
            }
            else{
                pos = new Vector3(10f,UnityEngine.Random.Range(-1.5f,6f),0);
            }
            generateEnemy(EnemyType.missile , pos);

            yield return new WaitForSeconds(missileCoolDown);
        }
    }

    void generateBoss1(){
        generateEnemy(EnemyType.boss1 , new Vector3(7.5f, 2.5f, 0));
        bossIsGenerated = true;

        
    }
   
    // Update is called once per frame
    public override void init_level_event(){

        base.Waves = new List<Action>();
        //base.Waves.Add(Wave1);
        //base.Waves.Add(Wave2);
        base.Waves.Add(Wave3);
        base.numWave = Waves.Count;   
        
    }

  


    IEnumerator Wave1_(){
        bool canGenerateZombie = true;
        bool canGenerateMissile =  true;
        int curZombieNum = 0;
        int curMissileNum = 0;

        while( zomibeNumForEachWave[0] > curZombieNum || missileNumForEachWave[0] > curMissileNum  ){
          
            if(canGenerateZombie && zomibeNumForEachWave[0] > curZombieNum){
                canGenerateZombie = false;
                generateEnemy(EnemyType.zombie);

                curZombieNum += 1;

                Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                            }
                                        , zombieCoolDown );
            }
            if(canGenerateMissile && missileNumForEachWave[0] > curMissileNum ){
            
                canGenerateMissile = false;
                generateMissileWave();
                curMissileNum += missileNumInWave;

                Utils.Instance.SetTimer( ()=>{canGenerateMissile = true;}, missileWaveCoolDown );
              
            }               
            yield return new WaitForSeconds(0.01f);
        }
        
        while( WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }

    IEnumerator Wave2_(){
        bool canGenerateZombie = true;
        bool canGenerateMissile =  true;
        int curZombieNum = 0;
        int curMissileNum = 0;

        while( zomibeNumForEachWave[1] > curZombieNum || missileNumForEachWave[1] > curMissileNum  ){
          
            if(canGenerateZombie && zomibeNumForEachWave[1] > curZombieNum){
                canGenerateZombie = false;
                generateEnemy(EnemyType.zombie);

                curZombieNum += 1;

                Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                            }
                                        , zombieCoolDown );
            }
            if(canGenerateMissile && missileNumForEachWave[1] > curMissileNum ){
            
                canGenerateMissile = false;
                generateMissileWave();
                curMissileNum += missileNumInWave;

                Utils.Instance.SetTimer( ()=>{canGenerateMissile = true;}, missileWaveCoolDown );
              
            }               
            yield return new WaitForSeconds(0.01f);
        }
        
        while( WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
    IEnumerator Wave3_(){
        generateEnemy(EnemyType.boss1 , new Vector3(8f,3f,0));

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


    void Update()
    {   

        
       
        
      
    }
}
