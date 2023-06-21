using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class playerSkill 
{
  
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;

    public class OnSkillUnlockedEventArgs : EventArgs {
        public SkillType skillType;
    }

    public enum SkillType {
        None,
        doubleShot,
        tripleShot,
        machineGun,

        spread_2,
        spread_3,

        shotGun,

        penetrate_1,
        penetrate_2,

        sniper,
        laser,

        atk_up,
        small_explosion,

        rocket,
        bomb,

        max_hp_2,
        max_hp_3,

        flamer_thrower,


    }

    private List<SkillType> unlockedSkillTypeList;
    
    public playerSkill() {
        unlockedSkillTypeList = new List<SkillType>();
    }
    public bool IsSkillUnlocked(SkillType skillType) {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public SkillType GetSkillRequirement(SkillType skillType) {
        switch (skillType) {
        case SkillType.tripleShot:     return SkillType.doubleShot;
        case SkillType.machineGun:     return SkillType.tripleShot;
        case SkillType.spread_3:    return SkillType.spread_2;
        case SkillType.shotGun:    return SkillType.spread_3;
        case SkillType.penetrate_2:     return SkillType.penetrate_1;
        case SkillType.sniper:     return SkillType.penetrate_2;
        case SkillType.laser:     return SkillType.sniper;
        case SkillType.small_explosion:     return SkillType.atk_up;
        case SkillType.rocket:     return SkillType.small_explosion;
        case SkillType.bomb:     return SkillType.rocket;
         case SkillType.max_hp_3:     return SkillType.max_hp_2;
         case SkillType.flamer_thrower:       return SkillType.max_hp_3;
        }
        return SkillType.None;
    }

    public bool CanUnlock(SkillType skillType) {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None) {
            if (IsSkillUnlocked(skillRequirement)) {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }
    }
    
    public bool TryUnlockSkill(SkillType skillType) {
        if (CanUnlock(skillType)) {
            UnlockSkill(skillType);
            return true;
        }
        else{
            return false;
        }
        //     if (skillPoints > 0) {
        //         skillPoints--;
        //         OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
        //         UnlockSkill(skillType);
        //         return true;
        //     } else {
        //         return false;
        //     }
        // } else {
        //     return false;
        // }
    }
    
    private void UnlockSkill(SkillType skillType) {
        if (!IsSkillUnlocked(skillType)) {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType });
        }
    }
}
