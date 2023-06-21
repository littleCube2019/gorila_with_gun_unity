using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_1 : level_wave
{   
    public int ZombieNum ;
    public int MissileNum;


    public int FasterMissileNum;

    public float missileWaveCoolDown ;
    public int missileNumInWave ;
    public float zombieCoolDownUpper;
    public float zombieCoolDownLower;
    public float missileCoolDown;

    public float fastermissileCoolDown ;
    public bool generateFakeZombieGenerator;
    public bool generateBoss1;


    void Start(){
        StartCoroutine(Wave1_());
    }
    IEnumerator Wave1_(){
        bool canGenerateZombie = true;
        bool canGenerateMissile =  true;
        bool canGenerateFasterMissile = true;
        int curZombieNum = 0;
        int curMissileNum = 0;
        int curFasterMissile = 0;
        while( ZombieNum > curZombieNum || MissileNum > curMissileNum || FasterMissileNum > curFasterMissile   ){
          
            if(canGenerateZombie && ZombieNum > curZombieNum){
                canGenerateZombie = false;
                monster_generator.Instance.generateEnemy(EnemyType.zombie,zombie_pos);

                curZombieNum += 1;
                float r  = Random.Range(zombieCoolDownLower,zombieCoolDownUpper) ;
                
                Utils.Instance.SetTimer( ()=>{canGenerateZombie = true;
                                            }
                                        , r );
            }
            if(canGenerateMissile && MissileNum > curMissileNum ){
            
                canGenerateMissile = false;
                generateMissileWave();
                curMissileNum += missileNumInWave;

                Utils.Instance.SetTimer( ()=>{canGenerateMissile = true;}, missileWaveCoolDown );
               
            }      
            if(canGenerateFasterMissile && FasterMissileNum > curFasterMissile ){
            
                canGenerateFasterMissile = false;
                
                Vector3 pos ;
                float r = UnityEngine.Random.Range(0f,1f);
                if(r > 0.5f){
                    pos = new Vector3(UnityEngine.Random.Range(-7f, 10f),6,0);
                }
                else{
                    pos = new Vector3(10f,UnityEngine.Random.Range(-1.5f,6f),0);
                }

                monster_generator.Instance.generateEnemy(EnemyType.FasterMissile,pos);
                curFasterMissile += 1;

                Utils.Instance.SetTimer( ()=>{canGenerateFasterMissile = true;}, fastermissileCoolDown );
               
            }             
            yield return new WaitForSeconds(0.01f);
        }

        if(generateFakeZombieGenerator){
            monster_generator.Instance.generateEnemy(EnemyType.flying_zombie_generator_fake);
        }   
        if(generateBoss1){
            monster_generator.Instance.generateEnemy(EnemyType.boss1 , new Vector3(8f,3f,0));;
        }
        
        while( level_manager.Instance.WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
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
            monster_generator.Instance.generateEnemy(EnemyType.missile , pos);

            yield return new WaitForSeconds(missileCoolDown);
        }
    }
}
