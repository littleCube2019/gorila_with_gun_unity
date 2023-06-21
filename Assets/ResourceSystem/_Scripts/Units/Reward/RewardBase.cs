using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBase : MonoBehaviour
{
    
    [System.Serializable]
    public struct BaseArgs
    {
        public int gearNum ;
        
    }
    public RewardType rewardType;      
    public float moveSpeed;
    public BaseArgs baseArgs;
    public Transform Player;
    Vector3 target ;
    bool isground = false;
    public Rigidbody2D rb ;

    public Transform ground;
   
    protected virtual void Awake()
    {
        player p = FindObjectOfType<player>();
        Player = p.transform;
        target = Player.position;
        
        rb = GetComponent<Rigidbody2D>();


        ground = GameObject.FindGameObjectWithTag("ground").transform;
        target.y = ground.position.y + GetComponent<BoxCollider2D>().size.y/2 ;
        
        transform.SetParent(level_manager.Instance.Rewards);
    }

     



    void FixedUpdate()
    {   
        
        if(Vector3.Distance(transform.position, target) > 1f){
            var step = moveSpeed * Time.fixedDeltaTime; // calculate distance to move
            if( target.x < transform.position.x){
                transform.position -= Vector3.right * step;
            }
            else{
                transform.position += Vector3.right * step;
            }
            
            //Vector2 relative = new Vector2(  target.x - transform.position.x , target.y - transform.position.y ); 
            
           
            //transform.rotation = Quaternion.FromToRotation(Vector3.left, relative);
            
            
            //Vector3 direction = player.position - transform.position;
        }
        
        if (transform.position.y <= target.y ){
            transform.position = new Vector3(transform.position.x, target.y, transform.position.z );
            isground = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }


        if(!isground){
            
            GetComponent<Rigidbody2D>().velocity += Vector2.down*1 ;
        }
    }

     public virtual void SetData(RewardType type) { 
        ScriptableRewardBase scriptable = (ScriptableRewardBase)ResourceSystem.Instance.GetRewardData(type);
        GetComponent<SpriteRenderer>().sprite = scriptable.sprite;
        // if(scriptable.anim != null){
        //     anim.runtimeAnimatorController = scriptable.anim;
        // }
        // else{
        //     anim = null;
        // }
        baseArgs = scriptable.baseArgs;
        rewardType = scriptable.rewardType;
        //health = baseArgs.maxHealth;
    }
    IEnumerator Absorb( Vector3 target ){
        
        while(Vector3.Distance(transform.position, target) > 1f){
            var step = moveSpeed * Time.fixedDeltaTime; // calculate distance to move
         
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            
            Vector2 relative = new Vector2(  target.x - transform.position.x , target.y - transform.position.y ); 
            
           
            transform.rotation = Quaternion.FromToRotation(Vector3.left, relative);
            transform.localScale = 0.95f * transform.localScale;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        
        
        player.Instance.getReward(rewardType , baseArgs.gearNum );
        AudioManager.instance.Play("pick"); 

        Destroy(gameObject);
    }
     private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<player>() != null){
            StartCoroutine(Absorb(other.gameObject.transform.position));
        }
        
    }
}
