using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoGoods : GoodsBase
{
    // Start is called before the first frame update
     [System.Serializable]
    public struct AmmoGoodsArgs
    {
        public int num;

        public WeaponType weaponType;
    }
    public AmmoGoodsArgs ammoGoodsArgs;
    public Text AmmoNum;
    public virtual void buyAmmo( ){



        if( player.Instance.gearNum >= baseArgs.price ){
            
            //GetComponent<Button>().enabled = false;

            player.Instance.gearNum -= baseArgs.price; 
            player.Instance.equipAmmo( ammoGoodsArgs.weaponType , ammoGoodsArgs.num);
            player.Instance.updateGearUI();
            
            
        }
     
    }

    public override void SetData(GoodsType type)
    {
        base.SetData(type);
        ScriptableAmmoGoods scriptable = (ScriptableAmmoGoods)ResourceSystem.Instance.GetGoodsData(type);
        ammoGoodsArgs = scriptable.ammoGoodsArgs;

        AmmoNum.text = "X" + ammoGoodsArgs.num.ToString();
    }

    
    new void Start(){
     
    }
}
