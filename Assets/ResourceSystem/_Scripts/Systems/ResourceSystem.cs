using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{


    List<ScriptableEnemyBase> enemyList;
    Dictionary<EnemyType, ScriptableEnemyBase> enemyDict;

    List<ScriptableWeaponBase> weaponList;
    Dictionary<WeaponType, ScriptableWeaponBase> weaponDict;

    List<ScriptableRewardBase> rewardList;
    Dictionary<RewardType, ScriptableRewardBase> rewardDict;

    List<ScriptableGoodsBase> goodsList;
    Dictionary<GoodsType, ScriptableGoodsBase>  goodsDict;

   
    
    protected override void Awake()
    {   
        DontDestroyOnLoad(gameObject);
        base.Awake();
        Assemble();
    }

    void Assemble()
    {
       

        enemyList = Resources.LoadAll<ScriptableEnemyBase>("Enemies").ToList();
        enemyDict = enemyList.ToDictionary(d => d.enemyType, d => d);

        weaponList = Resources.LoadAll<ScriptableWeaponBase>("Weapons").ToList();
        weaponDict = weaponList.ToDictionary(d => d.weaponType, d => d);

        rewardList = Resources.LoadAll<ScriptableRewardBase>("Rewards").ToList();
        rewardDict = rewardList.ToDictionary(d => d.rewardType, d => d);

        goodsList = Resources.LoadAll<ScriptableGoodsBase>("Goods").ToList();
        goodsDict = goodsList.ToDictionary(d => d.goodsType, d => d);

       
      
     
    }
    public ScriptableRewardBase GetRewardData(RewardType t) => rewardDict[t];
    public ScriptableEnemyBase GetEnemyData(EnemyType t) => enemyDict[t];
    public ScriptableWeaponBase GetWeaponData(WeaponType t) => weaponDict[t];

    public ScriptableGoodsBase GetGoodsData(GoodsType t) => goodsDict[t];

    
 
}
