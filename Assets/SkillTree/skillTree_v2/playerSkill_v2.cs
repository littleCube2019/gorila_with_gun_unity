using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class playerSkill_v2 
{
  
    public event EventHandler<OnSkillUnlockedEventArgs_v2> OnSkillUnlocked_v2;

    public class OnSkillUnlockedEventArgs_v2 : EventArgs {
        public SkillType skillType;
    }

    public enum SkillType {
        None,
        machineGun_v1,
        machineGun_v2,
        machineGun_v3,
        shotGun_v1,
        shotGun_v2,
        shotGun_v3,
        sniper_v1,
        sniper_v2,
        sniper_v3,



    }

    private List<SkillType> unlockedSkillTypeList;
    
    public playerSkill_v2() {
        unlockedSkillTypeList = new List<SkillType>();
    }
    public bool IsSkillUnlocked(SkillType skillType) {
        return unlockedSkillTypeList.Contains(skillType);
    }

    // public SkillType GetSkillRequirement(SkillType skillType) {
    //     switch (skillType) {
       
    //     }
    //     return SkillType.None;
    // }

    public bool CanUnlock(SkillType skillType, int price) {
        if( player.Instance.gearNum < price){
            return false;
        }
        player.Instance.GearUpdate(price);
        return true;

        // SkillType skillRequirement = GetSkillRequirement(skillType);

        // if (skillRequirement != SkillType.None) {
        //     if (IsSkillUnlocked(skillRequirement)) {
        //         return true;
        //     } else {
        //         return false;
        //     }
        // } else {
        //     return true;
        // }
    }
    
    public bool TryUnlockSkill(SkillType skillType , int price , skillSlot_UI slot) {
        if (CanUnlock(skillType,price)) {
            slot.level_up();
            UnlockSkill(skillType);
            return true;
        }
        else{
            return false;
        }
       
    }
    
    private void UnlockSkill(SkillType skillType) {
        if (!IsSkillUnlocked(skillType)) {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked_v2?.Invoke(this, new OnSkillUnlockedEventArgs_v2 { skillType = skillType });
        }
    }
}
