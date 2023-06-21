using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseGoods", menuName = "ScriptableObjs/Goods/Base")]
public class ScriptableGoodsBase : ScriptableObject
{
    public GoodsType goodsType;
    public Sprite mainIcon;
    public GoodsBase.BaseArgs baseArgs;
    public GoodsBase prefab;

   

}

public enum GoodsType
{
    machineGun,
    sniper,
    shotGun,

    machineGunAmmo,
    sniperAmmo,
    shotGunAmmo, 
    None,
    flameThrower,
    laser,    
}
