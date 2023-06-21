using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class shop_manager : Singleton<shop_manager>
{

    public GameObject shop_panel ;
    public GameObject Weapon_area;
    public GameObject Ammo_area;

    public List<GameObject> panelList;
    public List<clicky_tag> tagList; 

    public static event Action open_shop;

    public int currentPanelNumber;
    public void generateGoods(GoodsType type , Transform parent){
        ScriptableGoodsBase scriptable =  ResourceSystem.Instance.GetGoodsData(type);
        GoodsBase spawnedGoods = Instantiate(scriptable.prefab
                                    ,transform.position
                                    , Quaternion.identity);

        spawnedGoods.SetData(type);
        spawnedGoods.transform.SetParent(parent); 
        spawnedGoods.transform.localScale = new Vector3(1f,1f,1f);        
    }

    public void generateAmmoGoods(GoodsType type ){
        Transform parent = Ammo_area.transform;
        ScriptableGoodsBase scriptable =  ResourceSystem.Instance.GetGoodsData(type);
        GoodsBase spawnedGoods = Instantiate(scriptable.prefab
                                    ,transform.position
                                    , Quaternion.identity);

        spawnedGoods.SetData(type);
        spawnedGoods.mainIcon.sprite = scriptable.mainIcon;
        spawnedGoods.transform.SetParent(parent); 
        spawnedGoods.transform.localScale = new Vector3(1f,1f,1f);        
    }

    public void UpdateAmmoInstorehouse(WeaponType target , int num){
        foreach( Transform child in Weapon_area.transform ){
            
            WeaponType w = child.GetComponent<WeaponGoods>().weaponGoodsArgs.weaponType;
            if(w == target){
                child.Find("gun").
                Find("ammo_num").
                GetComponent<TextMeshProUGUI>().
                text = num.ToString(); 
            }
        }
        
    }
   
    private void Start() {
        
        switchPanel(0);
        generateGoods(GoodsType.machineGun, Weapon_area.transform );
        generateGoods(GoodsType.shotGun, Weapon_area.transform );
        generateGoods(GoodsType.sniper, Weapon_area.transform );
        generateGoods(GoodsType.flameThrower, Weapon_area.transform );
        generateGoods(GoodsType.laser, Weapon_area.transform );

        
        List<WeaponType> allWt =  new List<WeaponType>(gameDataRecorder.Instance.weaponRecords.Keys) ;
        foreach( WeaponType wt in allWt ){
            foreach( Transform child in Weapon_area.transform ){
                WeaponGoods goods =  child.GetComponent<WeaponGoods>(); 
                WeaponType w = goods.weaponGoodsArgs.weaponType;
                if(w == wt){
                    
                    goods.isbuyed_init();
                    
                    if( gameDataRecorder.Instance.weaponTypes.Contains(wt) ){ //on player` hand
                        goods.mainIcon.sprite = player.Instance.grey_panel ;
                        goods.weaponGoodsArgs.weaponType = WeaponType.none;
                        child.Find("gun").Find("ammo_num")
                        .GetComponent<TextMeshProUGUI>().text = ""; 
                    }
                    else{ 
                        GunWeapon g = (GunWeapon)gameDataRecorder.Instance.weaponRecords[wt];
                        goods.mainIcon.sprite = g.GetComponent<SpriteRenderer>().sprite;
                        goods.weaponGoodsArgs.weaponType = wt;
                         child.Find("gun").Find("ammo_num")
                        .GetComponent<TextMeshProUGUI>().text = g.currentAmmoNum.ToString(); 
                       
                    }
                   
             
                    
                    break;
                }
        }}
        open_shop?.Invoke();
        //gameObject.SetActive(false);
        //generateGoods(GoodsType.machineGunAmmo, Ammo_area.transform );
        //generateGoods(GoodsType.shotGunAmmo, Ammo_area.transform );
        
    }



    public void switchPanel(int num){
        
        panelList[currentPanelNumber].SetActive(false);
        tagList[currentPanelNumber].setUnpressed();

        panelList[num].SetActive(true);
        tagList[num].setPressed();
        currentPanelNumber = num;


    }
  


    public void open(){
       
        
        open_shop?.Invoke();
        shop_panel.SetActive(true);
        
        //updateUI();
    }
    public void close(){
        shop_panel.SetActive(false);
        Utils.Instance.SetTimer(()=>{level_manager.Instance.current_wave_generator.init_level();}, 0.05f);
        
    }

    

}
