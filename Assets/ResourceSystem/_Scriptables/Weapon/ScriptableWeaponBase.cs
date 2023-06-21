using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseWeapon", menuName = "ScriptableObjs/Weapon/Base")]
public class ScriptableWeaponBase : ScriptableObject
{
    public WeaponType weaponType;
    public Sprite sprite;
    public WeaponBase.BaseArgs baseArgs;
    public WeaponBase prefab;
}

public enum WeaponType
{
    none,
    machineGun,
    shotGun,
    snipper,
    pistol,

    flame_thrower,

    laser,
    rocket,
    cannon,

}