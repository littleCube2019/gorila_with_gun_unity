using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseReward", menuName = "ScriptableObjs/Reward/Base")]
public class ScriptableRewardBase : ScriptableObject
{
    public RewardType rewardType;
    public Sprite sprite;
    public RewardBase.BaseArgs baseArgs;
    public RewardBase prefab;

    //public RuntimeAnimatorController anim;

}

public enum RewardType
{
    gear,
}
