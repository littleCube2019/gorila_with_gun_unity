using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class shopGood 
{

    public event EventHandler<OnGoodBuyEventArgs> OnGoodBuy;

    public class OnGoodBuyEventArgs : EventArgs {
        public _GoodType goodType;
        public int price;
    }
    public enum _GoodType {
        None,
        hp_up,
        machineGunAmmo,
        shotGunAmmo,
        sniperAmmo,

        laserAmmo,
        bombAmmo,
        rocketAmmo,
        flame_throwerAmmo,

        allAmmo10,

    }
   
    private List<_GoodType> unlockedGoodTypeList;
    private playerSkill playerskill;

    public playerSkill.SkillType GetSkillRequirement(_GoodType goodType) {
        switch (goodType) {
        case _GoodType.machineGunAmmo:     return playerSkill.SkillType.machineGun;
        case _GoodType.shotGunAmmo:     return playerSkill.SkillType.shotGun;
        case _GoodType.sniperAmmo:     return playerSkill.SkillType.sniper;
        case _GoodType.laserAmmo:     return playerSkill.SkillType.laser;
        case _GoodType.bombAmmo:     return playerSkill.SkillType.bomb;
        case _GoodType.rocketAmmo:     return playerSkill.SkillType.rocket;
        case _GoodType.flame_throwerAmmo:     return playerSkill.SkillType.flamer_thrower;
        }
        return playerSkill.SkillType.None;
    }



    public shopGood(playerSkill playerskill) {
        this.playerskill = playerskill;
        unlockedGoodTypeList = new List<_GoodType>();
        playerskill.OnSkillUnlocked += Good_OnSkillUnlocked;
        unlockedGoodTypeList.Add(_GoodType.hp_up);
    }
    
    public void Good_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e){
        switch (e.skillType) {
            
            case playerSkill.SkillType.machineGun:
                unlockedGoodTypeList.Add( _GoodType.machineGunAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                
                break;
            case playerSkill.SkillType.sniper:
                unlockedGoodTypeList.Add( _GoodType.sniperAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                break;
            case playerSkill.SkillType.laser:
                unlockedGoodTypeList.Add( _GoodType.laserAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                break;
            case playerSkill.SkillType.shotGun:
                unlockedGoodTypeList.Add( _GoodType.shotGunAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);

                break;
            case playerSkill.SkillType.bomb:
                unlockedGoodTypeList.Add( _GoodType.bombAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                break;
            case playerSkill.SkillType.rocket:
                unlockedGoodTypeList.Add( _GoodType.rocketAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                break;
            case playerSkill.SkillType.flamer_thrower:
                unlockedGoodTypeList.Add( _GoodType.flame_throwerAmmo);
                unlockedGoodTypeList.Add( _GoodType.allAmmo10);
                break;

        }
    }
    public void buy(_GoodType g , int p){
        OnGoodBuy?.Invoke(this, new OnGoodBuyEventArgs { goodType = g ,  price=p  });
    }

    public bool IsGoodUnlocked(_GoodType goodType) {
        return unlockedGoodTypeList.Contains(goodType);
    }
    
    
    
}
