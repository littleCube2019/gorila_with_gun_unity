using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class level_manager : Singleton<level_manager>
{
    [HideInInspector] public int current_wave;
    public Image statistic_panel ;
    public Image fail_panel;
    public GameObject statistic_row;
    public Transform statistic_area;
    int currentWaveId = 0;

    public GameObject level_obj;
    // statistic
    public Transform Rewards;
    public Transform Enemies;

    public Transform Tomb;
    public shop_panel shop;
    public GameObject shop_pannel;
    //public level_statistic_type level_statistic_type_template;
    // statistic
    //public List<int> unlock_next_levels;
    //public level_statistic current_level_statistic;
    //public Button back_menu_btn;
    public monster_generator current_wave_generator;


    public List<GameObject> waves;
    
    
    GameObject currentWave;
    
    void Start(){
       
        currentWaveId = 0;
        StartWave();
        //player.Instance.playerSkills.OnSkillUnlocked += level_manager_OnSkillUnlocked;
    }
    
   // public void level_manager_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e){
    //    LevelUpFinish();
   // }

    public void StartWave( ){
        GameObject new_wave = Instantiate(waves[currentWaveId] , new Vector3(10f,-3.5f,0), transform.rotation);
        currentWave = new_wave;

        
        
    }

    public bool WaveNotFinish(){
       
        return (Enemies.childCount != 0
                || Rewards.childCount != 0) ;
    }


    public void WaveFinish(){
        print("wave clear, time to level up !!");
        currentWaveId += 1;
        Destroy(currentWave);
        currentWave = null ;

        // tmp
        //shop.open();
        shop_pannel.SetActive(true);
    }


    public void LevelUpFinish(){
        print("finish level uping");
        
        StartWave();
    }


    public void DestroyAllEnemy(){
        foreach( Transform enemy in Enemies){
            Destroy(enemy.gameObject);
        }
    }

    public void killAllEnemy(){
        foreach( Transform enemy in Enemies){
            enemy.GetComponent<EnemyBase>()._Destroy();
        }
    }

    public void levelFailed(){
        DestroyAllEnemy();
        currentWave.GetComponent<level_wave>().stopLevel();
        //fail_panel.gameObject.SetActive(true);

      
        
    }



  

    void Update(){
        
    }


}
