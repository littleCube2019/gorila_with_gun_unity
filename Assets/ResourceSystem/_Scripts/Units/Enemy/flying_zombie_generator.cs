using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying_zombie_generator : EnemyBase
{
    // Start is called before the first frame update
    moving_path curve;
    public bool isShow;
    public Transform door_point;

    float generateCooldown = 2f;
    float animation_time = 2f;
    bool canGenerate = true;

    bool alreadyEnhanced = false;
    IEnumerator flying_in_sub_level1(){
        int counter = 0;    
        for(float t = 1 ; t >= 0 ; t-= 0.01f){
            transform.position = curve.get_position(t);
 
            if(counter == 10 || counter==20 || counter==30){
                anim.SetTrigger("open");
                generateZombie();
            }
            counter += 1;
            yield return new WaitForSeconds(0.05f) ;
        }
        
        GetComponent<EnemyBase>()._Destroy(false);
    }

    public override void EventduringHurt(){
        if( !alreadyEnhanced && (float) health / (float)baseArgs.maxHealth < 0.5f){
            generateCooldown = generateCooldown * 0.5f;
            baseArgs.moveSpeed = baseArgs.moveSpeed * 1.5f;
            alreadyEnhanced = true;
        }
      
    }

    IEnumerator flying(){
        //int counter = 0;    
        for(float t = 1 ; t >= 0 ; t-= 0.01f){
            Vector2 target = curve.get_position(t);
            while( Vector2.Distance( target , transform.position) > 0.01f){
                transform.position = Vector3.MoveTowards(transform.position, target, baseArgs.moveSpeed * Time.fixedDeltaTime);
                float r = UnityEngine.Random.Range(0f,1f);
                    if( r < 0.1f && canGenerate){
                        canGenerate = false;
                        
                        
                        anim.SetTrigger("open");
                        int n =  (int)((1  - (base.health/base.baseArgs.maxHealth)) * 4) ;
                        //print(n+1);
                        Utils.Instance.SetTimer(()=>{generateZombie(n+1);
                                                    anim.SetTrigger("close");
                                                    },  animation_time);
                        
                        Utils.Instance.SetTimer(()=>{canGenerate = true;
                                                    },  2*animation_time );
                        
                    }
                    
                        
                    
                yield return new WaitForSeconds(0.1f);
            }
           
                        
          
            yield return new WaitForSeconds(0.05f) ;
        }
        
        //GetComponent<EnemyBase>()._Destroy(false);
    }

    void generateZombie(int n = 1){
        StartCoroutine(IgenerateZombie(n));
    }

    IEnumerator IgenerateZombie(int n = 1){
        for(int i = 0 ; i < n ; i ++){
            int scale  =  UnityEngine.Random.Range(3, 5)  ;
            Vector2 force = scale*Vector2.up;
            float r1 = UnityEngine.Random.Range(-10f, 10f) ;
            force = Quaternion.AngleAxis(r1, Vector3.forward) * force;
            monster_generator.Instance.generateEnemy(EnemyType.zombie, door_point.position , force);
            yield return new WaitForSeconds(0.5f);
        }
    }

    new void Start(){
        base.Start();
        
        if(isShow){
            curve = GameObject.Find("curve").GetComponent<moving_path>();   
            StartCoroutine(flying_in_sub_level1());
        }
        else{
            curve = GameObject.Find("random_point").GetComponent<moving_path>();   
             StartCoroutine(flying());
        }
    }


}
