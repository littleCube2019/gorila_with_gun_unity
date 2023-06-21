using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;


public class Ammo_Template : MonoBehaviour
{
    // Start is called before the first frame update
    public enum FireMode // your custom enumeration
    {
       OrgRight, 
       OrgToMouse,
       UserDefine,

       OrgToTouch,
    };
    

    public FireMode fireMode;
    public Transform original_point;
    public float speed;
    public float range;
    public int atk;
    
    public bool isboomerage;
    public bool selfRotation;

    public Transform ref_point;
   

    public UnityEvent TriggerEnterEvent;
    public List<string> TriggerEnterTags;
    
    public bool isEndingEvent;
    public UnityEvent EndingEvent;
    
   
    public bool isShotgun;
    public int NumOfShotgunAmmo;
    public float angleShotgun;
   
   
    public bool isGravity;
    public float gravityScale;

    public float forceScale;
    public float lifeTime;

    public bool isPenetrate;
    public bool isFadingOut;
    [HideInInspector]
    public int curNumOfShotgunAmmo;
    public float timer;
    protected Vector2 targetPosition;
    protected float relativeLength;
    float step;
    float bias;
    public Vector2 direction ; 
    
    public Transform ground; 
    
    bool isDot;
    float dotInterval;
    int penerateNum;
    bool small_explode;
    GunWeapon gunParent;

    public void setDirection(Vector2 dir){
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.zero;
        transform.RotateAround(transform.position, Vector3.forward, angle);
        direction = dir;
    }
    // Update is called once per frame
    
    public virtual void SetArgs( GunWeapon.GunArgs args , GunWeapon gun){
        speed = args.ammoSpeed;
        range = args.range;
        isShotgun = args.isShotgun;
        angleShotgun = args.shotgunAngle;
        NumOfShotgunAmmo = args.NumOfShotgunAmmo;
        atk = args.atk;
        isPenetrate = args.isPenetrate;
        transform.GetComponent<SpriteRenderer>().sprite = args.ammoSprite;
        bias = args.bias;
        lifeTime = args.lifeTime;
        isFadingOut = args.isFadingOut;
        isGravity = args.isGravity;
        gravityScale = args.gravityScale;
        forceScale = args.initforceScale;
        isDot =args.isDot;
        dotInterval = args.dotiInterval;
        penerateNum = args.penerateNum;
        small_explode = args.small_explode;
        gunParent = gun;
    }

    public void Awake(){
        
        ground = GameObject.FindGameObjectWithTag("ground").transform;
        ref_point = GameObject.Find("ammo_ref").transform;
        //original_point = GameObject.Find("fire_pos").transform;
       
        if(selfRotation){
            int init_rotate= UnityEngine.Random.Range(0, 181);
            transform.RotateAround(transform.position, Vector3.forward, init_rotate);
        }

        if(fireMode == FireMode.OrgRight){
            direction = original_point.transform.right;
        }
        else if(fireMode == FireMode.OrgToMouse){
            Vector2 mousePos;
            mousePos.x  = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            mousePos.y  = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

      
            Vector2 relative = new Vector2(mousePos.x - ref_point.transform.position.x, mousePos.y - ref_point.transform.position.y  ); 
            relativeLength = relative.magnitude;
            relative = range*relative.normalized;
            setDirection(relative);
    
            Vector3 normal = Quaternion.AngleAxis( 90f , Vector3.forward) *  relative ;
            normal =  normal.normalized;
            transform.position = original_point.position + UnityEngine.Random.Range(-bias, bias) * normal;  
            //float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            //direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;
        }
        else if(fireMode == FireMode.OrgToTouch){
            Vector2 mousePos;
            mousePos.x  = Camera.main.ScreenToWorldPoint(Input.touches[0].position).x;
            mousePos.y  = Camera.main.ScreenToWorldPoint(Input.touches[0].position).y;
            Vector2 relative = new Vector2(mousePos.x - ref_point.transform.position.x, mousePos.y - ref_point.transform.position.y  ); 
            relative = range*relative.normalized;
            setDirection(relative);
            Vector3 normal = Quaternion.AngleAxis( 90f , Vector3.forward) *  relative ;
            normal = 0.1f * normal.normalized;
            transform.position = original_point.position + UnityEngine.Random.Range(-bias, bias) * normal; 
            //float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            //direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;

        }

        targetPosition = (Vector2)original_point.transform.position + range* direction;
        timer=0;
        if(isGravity){
            transform.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            transform.GetComponent<Rigidbody2D>().velocity = forceScale*direction*relativeLength;
        }
        if(isShotgun){
            this.gameObject.SetActive(false);
            float unitAngle = angleShotgun / NumOfShotgunAmmo;
            
            for (int i = -NumOfShotgunAmmo/2 ; i <= NumOfShotgunAmmo/2  ; i++){
                    if(i==0 && NumOfShotgunAmmo %2 == 0) continue;
                    GameObject ammo = Instantiate(this.gameObject );
                    ammo.GetComponent<Ammo_Template>().SetArgs( gunParent.gunArgs, gunParent );
                    
                    
                    ammo.GetComponent<Ammo_Template>().isShotgun = false;
                    ammo.GetComponent<Ammo_Template>().fireMode = FireMode.UserDefine;

                    Vector2 tmp = direction;
                    tmp = Quaternion.AngleAxis(i*unitAngle, Vector3.forward) * tmp;
                    ammo.GetComponent<Ammo_Template>().setDirection(tmp);
                    
                    ammo.SetActive(true);
            }
            

            Destroy(this.gameObject);
        }
        
        
    }
    public void Update()
    {   

        if(isFadingOut){
            if(timer >= lifeTime){
                
                    if(isEndingEvent){
                            EndingEvent.Invoke();
                    }
                Destroy(gameObject);
            }
               
        }   
       
        if(!isGravity){
            if(Vector2.Distance(transform.position, targetPosition) > 0.01f){
                if(selfRotation){
                    transform.RotateAround(transform.position, Vector3.forward, 360 * Time.deltaTime);
                }
                transform.position= Vector2.MoveTowards(transform.position,targetPosition ,speed * Time.deltaTime );
                
            }

               
            else{
                if(isboomerage){
                    targetPosition = (Vector2)original_point.position;
                    isboomerage = false;
                }
                
                else{
                    if(isEndingEvent){
                        EndingEvent.Invoke();
                    }
                    Destroy(gameObject);
                }
            }
        }
        else{
            if(selfRotation){
                    transform.RotateAround(transform.position, Vector3.forward, 360 * Time.deltaTime);
            }
            if(isFadingOut){
                if(timer >= lifeTime){
                    
                    if(isEndingEvent){
                            EndingEvent.Invoke();
                    }
                    Destroy(gameObject);
                }
                
              
            }        

            if( ground.position.y >= transform.position.y  ){
                
                OnGroundEvent();
            }

        }
        timer += Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
         
        if(other.gameObject.GetComponent<IDamagable>()!=null ){
            penerateNum-=1;
            
            if(!isDot || gunParent.canDot){ 
                if(GetComponent<Explosion_Template>() == null){
                    other.GetComponent<IDamagable>().TakeDamage(atk);
                }
            }
            
            
            if(!isPenetrate && penerateNum<0){
                HitEnemyEvent();
             
            }
        }   
    }


    protected virtual void HitEnemyEvent(){
        if(small_explode){
            effectManager.Instance.explode(transform.position , 0.5f * Vector3.one);
        }
        Destroy(gameObject);
    }

    protected virtual void OnGroundEvent(){
        Destroy(gameObject);
    }
}

