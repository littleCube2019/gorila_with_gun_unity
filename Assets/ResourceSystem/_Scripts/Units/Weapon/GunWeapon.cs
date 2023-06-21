using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    [System.Serializable]
    public struct GunArgs
    {
        public string soundEffectName;
        public int maxAmmoAmount ;
        
        public float range;
        public int atk;
        public int ammoSpeed;

        public GoodsType ammoGoodsType;
        public Sprite ammoSprite;

        public float bias;
        public bool isPenetrate;

        [Header("Shotgun")]
        public bool isShotgun;
        public float shotgunAngle;
        public int NumOfShotgunAmmo;

        [Header("Time decay")]
        public bool isFadingOut ;
        public float lifeTime  ;

        [Header("Gravity")]
        public bool isGravity;
        public float gravityScale;
        public float initforceScale;

        [Header("Damage over time")]
        public bool isDot ;
        public float dotiInterval;

        [Header("Pistol")]
        public bool isRapid;
        public int shotNum;

        public int penerateNum;
        public bool small_explode;
    }
    [HideInInspector]
    public int currentAmmoNum ;
    public GunArgs gunArgs;
    public List<GameObject> ammoObject;
    [HideInInspector] public int ammo_level = 0;

    public WeaponType gun_type ;
    public GameObject fire_effect;
    public bool canDot = true;

    public GameObject getAmmo(){
        return ammoObject[ammo_level];
    }
    public void ammoLevelUp(){
        ammo_level += 1;
    }
    public virtual void fire(){
        if( currentAmmoNum == 0 && gun_type != WeaponType.pistol  ){
            return ;
        }
        //get ammo and fire it 
        AudioManager.instance.Play(gunArgs.soundEffectName);  

        if( fire_effect != null){
            fire_effect.GetComponent<Animator>().SetTrigger("fire");
        }

        if(gun_type != WeaponType.pistol){
            currentAmmoNum -= 1;
        }

        if(gunArgs.isDot){
            canDot = false;
            Utils.Instance.SetTimer(()=>{canDot = true;}, gunArgs.dotiInterval);
        }
      
    }
  
    public bool getAmmo(int num){
        if( currentAmmoNum >= gunArgs.maxAmmoAmount ) return false;
        currentAmmoNum = Mathf.Min( currentAmmoNum + num , gunArgs.maxAmmoAmount);
        return true;
    }

    public void fullAmmo(){
        currentAmmoNum = gunArgs.maxAmmoAmount;
    }

    public void rapidUp(){
        gunArgs.shotNum += 1;
    }


    new protected virtual void Start()
    {   
       base.Start();
       
    }


    public override void SetData(WeaponType type) { 
        base.SetData(type);
        ScriptableGunWeapon scriptable = (ScriptableGunWeapon)ResourceSystem.Instance.GetWeaponData(type);
        GetComponent<SpriteRenderer>().sprite = scriptable.sprite;
        gunArgs = scriptable.gunArgs;
        gun_type = scriptable.weaponType;
        currentAmmoNum = gunArgs.maxAmmoAmount;
    }
}
