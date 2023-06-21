using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shop_UI : MonoBehaviour
{   
    [SerializeField] private Material goodLockedMaterial;
    [SerializeField] private Material goodUnlockableMaterial;
    
    private List<GoodButton> GoodButtonList;
    
    private playerSkill playerskill;
    private shopGood shopgood;
    
    void Start(){
        playerskill = player.Instance.playerSkills;
        GoodButtonList = new List<GoodButton>();
        shopgood = player.Instance.shopgood;
        SetGoodButtons(shopgood);
        
    }

    void SetGoodButtons(shopGood shopgood){
        GoodButtonList.Add(new GoodButton(transform.Find("hp_upBtn"), shopgood
        , shopGood._GoodType.hp_up, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("machineGunAmmoBtn"), shopgood
        // , shopGood._GoodType.machineGunAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("shotGunAmmoBtn"), shopgood
        // , shopGood._GoodType.shotGunAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("sniperAmmoBtn"), shopgood
        // , shopGood._GoodType.sniperAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("laserAmmoBtn"), shopgood
        // , shopGood._GoodType.laserAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("bombAmmoBtn"), shopgood
        // , shopGood._GoodType.bombAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        //  GoodButtonList.Add(new GoodButton(transform.Find("rocketAmmoBtn"), shopgood
        // , shopGood._GoodType.rocketAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        // GoodButtonList.Add(new GoodButton(transform.Find("flame_throwerAmmoBtn"), shopgood
        // , shopGood._GoodType.flame_throwerAmmo, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        
        GoodButtonList.Add(new GoodButton(transform.Find("allAmmo10btn"), shopgood
        , shopGood._GoodType.allAmmo10, goodLockedMaterial, goodUnlockableMaterial , 1 ));
        
        shopgood.OnGoodBuy += shopUI_OnGoodBuy;
        playerskill.OnSkillUnlocked += shopUI_OnSkillUnlocked;
        UpdateVisuals();

    }

    private void shopUI_OnGoodBuy(object sender, shopGood.OnGoodBuyEventArgs e) {
        UpdateVisuals();
    }
    private void shopUI_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e) {
        UpdateVisuals();
    }

    private void UpdateVisuals(){
        foreach (GoodButton btn in GoodButtonList) {
            btn.UpdateVisual();
        }
    }

     private class GoodButton {

        private Transform btnTransform; 
        private Image image;

        private shopGood shopgood;
        private shopGood._GoodType goodType;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        private int price;
        public GoodButton(Transform transform, shopGood shopgood, shopGood._GoodType goodType, Material skillLockedMaterial, Material skillUnlockableMaterial
        , int price) {
            this.btnTransform = transform;
            this.shopgood = shopgood;
            this.goodType = goodType;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;
            this.price = price;
            image = transform.GetComponent<Image>();
            //backgroundImage = transform.Find("background").GetComponent<Image>();
            
            transform.GetComponent<Button>().onClick.AddListener(() => {
                    if (player.Instance.gearNum >= price) {
                        shopgood.buy( goodType , price );
                    }
                    else{
                        print("No enough gear QQ!");
                    }
            });
        }

        public void UpdateVisual() {
            if (shopgood.IsGoodUnlocked(goodType)) {
                image.material = skillUnlockableMaterial;
                btnTransform.GetComponent<Button>().enabled = true;
                         
            } else {
               
                    image.material = skillLockedMaterial;
                    btnTransform.GetComponent<Button>().enabled = false;
            }
        }

            
    }

}

