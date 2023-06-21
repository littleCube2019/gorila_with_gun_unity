using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameDataRecorder : Singleton<gameDataRecorder>
{

    //game 
    public List<int> unlock_levels;

    //player
    public Dictionary< WeaponType, WeaponBase > weaponRecords;
    public List<WeaponBase> weapons;
    public List<WeaponType> weaponTypes;
    public int gear;



    // Weapon1 
    // Weapon2 
    // Weapon3 
    // all weapon list 
    // gear 
    // what buied at store
    void Start(){
        unlock_levels = new List<int>();
        unlock_levels.Add(0); 
        unlock_levels.Add(1);
        unlock_levels.Add(2);

        DontDestroyOnLoad(gameObject);
        weaponRecords = new Dictionary<WeaponType, WeaponBase>();   
        weaponTypes = new List<WeaponType>(){WeaponType.flame_thrower , WeaponType.cannon, WeaponType.laser};
        weapons = new List<WeaponBase>();   
        gear = 10000;

        for(int i = 0 ; i < 3 ; i ++){
            if(weaponTypes[i] == WeaponType.none){
                continue;
            }
            ScriptableWeaponBase scriptable =  ResourceSystem.Instance.GetWeaponData(weaponTypes[i]);
            WeaponBase new_weapon = Instantiate(scriptable.prefab, transform.position + -100*Vector3.right, Quaternion.identity);
            new_weapon.transform.SetParent(transform);
            new_weapon.SetData(weaponTypes[i]);
            weaponRecords[weaponTypes[i]] = new_weapon;
            weapons.Add(new_weapon);
        }
        
    }


}
