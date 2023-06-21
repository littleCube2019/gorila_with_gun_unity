using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponGoods : GoodsBase
{
    // Start is called before the first frame update
     [System.Serializable]
    public struct WeaponGoodsArgs
    {
    

        public int AmmoNum;
        public int AmmoPrice;
        public WeaponType weaponType;
        
    }
    public WeaponGoodsArgs weaponGoodsArgs;
    public GameObject switch_btn_area;
    public Button switch_1;
    public Button switch_2 ;
    public Button buyAmmoBtn;
    public TextMeshProUGUI ammo_text;

    GunWeapon gun;
   
    public virtual void buyWeapon( ){
        if(isSold){
           // player.Instance.equipWeapon( weaponGoodsArgs.weaponType);
        }
        else if( player.Instance.gearNum >= baseArgs.price ){
            player.Instance.gearNum -= baseArgs.price; 
            isbuyed();
        }
    }


    public void isbuyed_init(){

        foreach( GoodsType g in baseArgs.unlockedGoods){
            shop_manager.Instance.generateAmmoGoods(g);
        }
        
        gun = (GunWeapon)gameDataRecorder.Instance.weaponRecords[weaponGoodsArgs.weaponType];

        priceText.text = "X";
        //player.Instance.updateGearUI();
        base.isSold = true;
        switch_btn_area.SetActive(true);


        switch_1.onClick.AddListener(() => switchWeapon(1));
        switch_2.onClick.AddListener(() => switchWeapon(2));
        buyAmmoBtn.onClick.AddListener(()=>buyAmmo() );
        
    }

    public void isbuyed(){

        foreach( GoodsType g in baseArgs.unlockedGoods){
            shop_manager.Instance.generateAmmoGoods(g);
        }
        
       
        priceText.text = "X";
        player.Instance.updateGearUI();
        base.isSold = true;
        switch_btn_area.SetActive(true);


        switch_1.onClick.AddListener(() => switchWeapon(1));
        switch_2.onClick.AddListener(() => switchWeapon(2));
        buyAmmoBtn.onClick.AddListener(()=>buyAmmo() );

        GunWeapon targetWeapon = (GunWeapon)player.Instance.createWeapon(weaponGoodsArgs.weaponType);
        shop_manager.Instance.UpdateAmmoInstorehouse( weaponGoodsArgs.weaponType, targetWeapon.gunArgs.maxAmmoAmount);
   
       
        gun = (GunWeapon)player.Instance.weaponRecords[weaponGoodsArgs.weaponType];
    }

    public void buyAmmo(){
        
        if( gun.currentAmmoNum < gun.gunArgs.maxAmmoAmount && player.Instance.gearNum >= weaponGoodsArgs.AmmoPrice ){
            player.Instance.gearNum -= weaponGoodsArgs.AmmoPrice;
            player.Instance.updateGearUI();
            gun.getAmmo( weaponGoodsArgs.AmmoNum  );

            player.Instance.updateAmmoUI();
        }
       
    }

    public void switchWeapon(int num){
        WeaponType tmp = weaponGoodsArgs.weaponType;
      
        base.mainIcon.sprite = player.Instance.weapon_icon[num].sprite;
        ammo_text.text = player.Instance.ammo_num[num].text;

        weaponGoodsArgs.weaponType = player.Instance.weapon_types[num];
        //print(weaponGoodsArgs.weaponType);
        player.Instance.equipWeapon(tmp , num );

        
    }

    public override void SetData(GoodsType type)
    {
        base.SetData(type);
        ScriptableWeaponGoods scriptable = (ScriptableWeaponGoods)ResourceSystem.Instance.GetGoodsData(type);
        weaponGoodsArgs = scriptable.weaponGoodsArgs;
    }

    void Update(){
        if(base.isSold){
            if( gun.currentAmmoNum >= gun.gunArgs.maxAmmoAmount){
                buyAmmoBtn.interactable = false;
            }
            else{
                buyAmmoBtn.interactable = true;
            }
        }
    }
    new void Start(){
     
    }
}
