using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;


public class Explosion_Template : Ammo_Template
{
    
    new void Awake(){
        base.Awake();
        
    }
    
    void Explosion(){
        effectManager.Instance.explode(transform.position , 2 * Vector3.one);
        Collider2D[] hits;
        hits = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D hit in hits)
        {
            if(hit.gameObject.GetComponent<IDamagable>()!=null ){

                
                hit.GetComponent<IDamagable>().TakeDamage( atk );
            }   
        }  

        Destroy(gameObject);
    }


    
 

    protected override  void HitEnemyEvent(){
        Explosion();
    }
    
    protected override void OnGroundEvent(){
        Explosion();
    }
    
}

