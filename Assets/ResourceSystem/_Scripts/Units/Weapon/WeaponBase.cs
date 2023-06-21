using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [System.Serializable]
    public struct BaseArgs
    {
        public int buyPrice ;

        public int level ;
        
        public int coolDown;
    }
    
    public BaseArgs baseArgs;
   
   
  
    


    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
     
    }


    public virtual void SetData(WeaponType type) { 
        ScriptableWeaponBase scriptable = (ScriptableWeaponBase)ResourceSystem.Instance.GetWeaponData(type);
        GetComponent<SpriteRenderer>().sprite = scriptable.sprite;
        baseArgs = scriptable.baseArgs;
       
    }
}
