using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoodsBase : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct BaseArgs
    {
        public int price;  

        public GoodsType goodsType;
        public List<GoodsType> unlockedGoods;
 
    }
    
    public BaseArgs baseArgs;
   
    public Image mainIcon;
    public Text priceText;
    public bool isSold ;
  
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
       isSold = false;
    }

 



   

    public virtual void SetData(GoodsType type) { 
        ScriptableGoodsBase scriptable = (ScriptableGoodsBase)ResourceSystem.Instance.GetGoodsData(type);
        
        mainIcon.GetComponent<Image>().sprite = scriptable.mainIcon;
       
        baseArgs = scriptable.baseArgs;

        priceText.text = "X" + baseArgs.price.ToString();
   
    }
}
