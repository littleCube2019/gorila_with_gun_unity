using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class monster_generator : Singleton<monster_generator>
{
    // Start is called before the first frame update
    
    public List<Action> Waves;
    [HideInInspector] public int numWave;

    [HideInInspector]
    public int curWave;
    
     [HideInInspector] public TextMeshProUGUI progressText;
     [HideInInspector] public Slider progressBar;

    public void stopLevel(){
        StopAllCoroutines();
    }

    public EnemyBase generateEnemy(EnemyType type){
        ScriptableEnemyBase scriptable =  ResourceSystem.Instance.GetEnemyData(type);
        EnemyBase spawnedEnemy = Instantiate(scriptable.prefab
                                    ,transform.position
                                    , Quaternion.identity);
        spawnedEnemy.SetData(type);
        if (spawnedEnemy.baseArgs.isGround){
           Vector3 pos = spawnedEnemy.transform.position;
           spawnedEnemy.transform.position = new Vector3 (pos.x
                            , -4.3f + spawnedEnemy.GetComponent<BoxCollider2D>().size.y/2
                            , 0) ; 
        }

        return spawnedEnemy;
    }


 
    public EnemyBase generateEnemy(EnemyType type, Vector3 pos ){
            ScriptableEnemyBase scriptable =  ResourceSystem.Instance.GetEnemyData(type);
            EnemyBase spawnedEnemy = Instantiate(scriptable.prefab
                                    ,pos
                                    , Quaternion.identity);
            spawnedEnemy.SetData(type);
            /*if (spawnedEnemy.baseArgs.isGround){
                Vector3 pos_ = spawnedEnemy.transform.position;
                spawnedEnemy.transform.position = new Vector3 (pos_.x
                                , -4.3f + spawnedEnemy.GetComponent<BoxCollider2D>().size.y/2
                                , 0) ; 
            }*/

            return spawnedEnemy;
    }
    
    public EnemyBase generateEnemy(EnemyType type, Vector3 pos , Vector3 force ){
            ScriptableEnemyBase scriptable =  ResourceSystem.Instance.GetEnemyData(type);
            EnemyBase spawnedEnemy = Instantiate(scriptable.prefab
                                    ,pos
                                    , Quaternion.identity);
            
            spawnedEnemy.GetComponent<Rigidbody2D>().AddForce(force , ForceMode2D.Impulse);
            
            spawnedEnemy.SetData(type);
            /*if (spawnedEnemy.baseArgs.isGround){
                Vector3 pos_ = spawnedEnemy.transform.position;
                spawnedEnemy.transform.position = new Vector3 (pos_.x
                                , -4.3f + spawnedEnemy.GetComponent<BoxCollider2D>().size.y/2
                                , 0) ; 
            }*/

            return spawnedEnemy;
    }

   
    public void UpdateProgressBar(){
        progressBar.value = curWave;
      
        int tmp = curWave+1;
       
        if(numWave > curWave){
            progressText.text = "Wave " + tmp.ToString();
            
            progressText.color = new Color(1f,1f,1f);
            
        }
        else{
            progressText.text = "Level clear!" ; 
        }
        
    }

    public bool WaveNotFinish(){
       
        return (level_manager.Instance.Enemies.childCount != 0
                || level_manager.Instance.Rewards.childCount != 0) ;
    }


    public virtual void  Start(){
        progressBar = GameObjectPlacement.Instance.levelProgressBar.GetComponent<Slider>();
        progressText = GameObjectPlacement.Instance.levelProgressBarText;
       
        
    }
    public void init_level(){
       

        curWave = 0; 
        Waves = new List<Action>();
        
        init_level_event();
        
        progressBar.minValue = curWave;
        progressBar.maxValue = numWave;
      
        progressBar.value = curWave;
        
       
        UpdateProgressBar();
        start_wave();
    }
    public void start_wave(){
       
        if( numWave > curWave ){
            Waves[curWave]();
        }
        else{
            // level_manager.Instance.levelClear();
        }
        start_wave_event();
    }

    public void wave_finish(){
       
        curWave += 1;
        UpdateProgressBar();
      
        wave_finish_event();
    }

    public virtual void  wave_finish_event(){

    }

    public virtual void start_wave_event(){

    }

    public virtual void init_level_event(){

    }
}
