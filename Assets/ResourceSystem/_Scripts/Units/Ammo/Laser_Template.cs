using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;


public class Laser_Template : Ammo_Template
{
    
    new void Awake(){
        base.Awake();
        timer = 0;
        GetComponent<LineRenderer>().SetPosition(0, original_point.position);
        GetComponent<LineRenderer>().SetPosition(1, original_point.position + (Vector3)( range*direction));
    }
    
    float atk_coolDown = 0.1f;
    bool is_CoolDown = false;
    new void Update()
    {   

        if(isFadingOut){
                if(timer >= lifeTime){
                
                    if(isEndingEvent){
                            EndingEvent.Invoke();
                    }
                    Destroy(gameObject);
                }
        }   
        timer += Time.deltaTime;   
        if(!is_CoolDown){
            is_CoolDown=true;
            Utils.Instance.SetTimer(()=>{is_CoolDown = false;}, atk_coolDown);
        
                RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(GetComponent<LineRenderer>().GetPosition(0), direction, range);
            
            foreach (RaycastHit2D hit in hits)
            {
                if(hit.collider.gameObject.GetComponent<IDamagable>()!=null ){
                    
                        
                        hit.collider.GetComponent<IDamagable>().TakeDamage( atk );
                    }
                    
            }   
        }  
        
    }
        
        
    
    
  
    private void OnTriggerEnter2D(Collider2D other){
        
    }


}

