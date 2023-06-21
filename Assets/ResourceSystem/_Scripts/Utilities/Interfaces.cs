using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//判斷是否可以攻擊
public interface IDamagable
{
    void TakeDamage(int damage);
}

public interface IWeapon
{
    void Use(Vector3 direction);
    void Reload();
}



// GetComponet<IDamagable>() is null or not
// interface就是提供一個通用的大類別供繼承，裡面有個Function繼承者都必須定義