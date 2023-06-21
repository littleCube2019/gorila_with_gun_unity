using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_bomb : level_wave
{   
    public int HighBombNum ;
    public int MidBombNum ;
    public int LowBombNum ;
    
    

    public float HighBombCoolDown ;
    public float MidBombCoolDown ;
    public float LowBombCoolDown ;


    public bool generateBombKnight;
    public bool generateBoss2;

    [ Header("show up")]
    public bool parallel;
    public bool seq; 

    public bool withMissile;
    void Start(){
        StartCoroutine(Wave1_());
    }
    IEnumerator Wave1_(){
        bool canGenerateHighBomb = true;
        bool canGenerateMidBomb =  true;
        bool canGenerateLowBomb =  true;
        int curHighBombNum = 0;
        int curMidBombNum = 0;
        int curLowBombNum = 0;
        if( parallel){
            while( HighBombNum > curHighBombNum || 
            MidBombNum > curMidBombNum ||
            LowBombNum > curLowBombNum   ){
            
                if(canGenerateHighBomb && HighBombNum > curHighBombNum){
                    canGenerateHighBomb = false;
                    monster_generator.Instance.generateEnemy(EnemyType.bomb_high,zombie_pos);

                    curHighBombNum += 1;

                    Utils.Instance.SetTimer( ()=>{canGenerateHighBomb = true;
                                                }
                                            , HighBombCoolDown );
                }
                if(canGenerateMidBomb && MidBombNum > curMidBombNum ){
                
                    canGenerateMidBomb = false;
                    monster_generator.Instance.generateEnemy(EnemyType.bomb_mid,zombie_pos);
                    curMidBombNum += 1;

                    Utils.Instance.SetTimer( ()=>{canGenerateMidBomb = true;}, MidBombCoolDown );
                
                }     
                if(canGenerateLowBomb && LowBombNum > curLowBombNum ){
                
                    canGenerateLowBomb = false;
                    monster_generator.Instance.generateEnemy(EnemyType.bomb_low,zombie_pos);
                    curLowBombNum += 1;

                    Utils.Instance.SetTimer( ()=>{canGenerateLowBomb = true;}, LowBombCoolDown );
                
                }                 
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if(seq){

            for(int i = 0 ; i < LowBombNum ; i++){
                monster_generator.Instance.generateEnemy(EnemyType.bomb_low,zombie_pos);
                yield return new WaitForSeconds(LowBombCoolDown);
            }
            for(int i = 0 ; i < MidBombNum ; i++){
                monster_generator.Instance.generateEnemy(EnemyType.bomb_mid,zombie_pos);
                yield return new WaitForSeconds(MidBombCoolDown);
            }
            for(int i = 0 ; i < HighBombNum ; i++){
                monster_generator.Instance.generateEnemy(EnemyType.bomb_high,zombie_pos);
                yield return new WaitForSeconds(HighBombCoolDown);
            }

        }

        else if(withMissile){
            moving_path hline = GameObject.Find("h_line").GetComponent<moving_path>();
            moving_path vline = GameObject.Find("v_line").GetComponent<moving_path>();
            
            for(float i = 0 ; i < 1 ; i+=0.1f){
                monster_generator.Instance.generateEnemy(EnemyType.missile, vline.get_position(i));    
            }

            while( HighBombNum > curHighBombNum || 
            MidBombNum > curMidBombNum ||
            LowBombNum > curLowBombNum   ){
            
                    if(canGenerateHighBomb && HighBombNum > curHighBombNum){
                        canGenerateHighBomb = false;
                        monster_generator.Instance.generateEnemy(EnemyType.bomb_high,zombie_pos);

                        curHighBombNum += 1;

                        Utils.Instance.SetTimer( ()=>{canGenerateHighBomb = true;
                                                    }
                                                , HighBombCoolDown );
                    }
                    if(canGenerateMidBomb && MidBombNum > curMidBombNum ){
                    
                        canGenerateMidBomb = false;
                        monster_generator.Instance.generateEnemy(EnemyType.bomb_mid,zombie_pos);
                        curMidBombNum += 1;

                        Utils.Instance.SetTimer( ()=>{canGenerateMidBomb = true;}, MidBombCoolDown );
                    
                    }     
                    if(canGenerateLowBomb && LowBombNum > curLowBombNum ){
                    
                        canGenerateLowBomb = false;
                        monster_generator.Instance.generateEnemy(EnemyType.bomb_low,zombie_pos);
                        curLowBombNum += 1;

                        Utils.Instance.SetTimer( ()=>{canGenerateLowBomb = true;}, LowBombCoolDown );
                    
                    }                 
                    yield return new WaitForSeconds(0.01f);
            }

            for(float i = 0 ; i < 1 ; i+=0.1f){
                monster_generator.Instance.generateEnemy(EnemyType.missile, hline.get_position(i));    
            }
        }
        
        while( level_manager.Instance.WaveNotFinish()){
             yield return 0;
           
           
        }

        if(generateBombKnight){
                monster_generator.Instance.generateEnemy(EnemyType.bomb_knight_big , new Vector3(7f,10f,0) );
            }   
        if(generateBoss2){
                monster_generator.Instance.generateEnemy(EnemyType.boss2 , new Vector3(7f, -3f, 0));
        }


        while( level_manager.Instance.WaveNotFinish()){
            yield return 0;
        }

        level_manager.Instance.WaveFinish();
    }
  
}
