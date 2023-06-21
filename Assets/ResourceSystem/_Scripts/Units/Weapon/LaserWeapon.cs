using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class LaserWeapon : GunWeapon
{

    public override void fire(){
        if( currentAmmoNum == 0 ){
            return ;
        }
        //get ammo and fire it 
        AudioManager.instance.Play("shot");
        currentAmmoNum -= 1;
    }
  
    
}
