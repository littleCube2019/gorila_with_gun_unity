using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : EnemyBase
{
     [System.Serializable]
    public struct JumpArgs
    {
        public float attackPower;
        public float attackRange;
        public float cooldown;

        public float jumpForce;
        public float downForce;
    }
    public JumpArgs jumpArgs;
    Vector3 target ;
    bool isground;

    public override void SetData(EnemyType type)
    {
        base.SetData(type);
        ScriptableJumpEnemy scriptable = (ScriptableJumpEnemy)ResourceSystem.Instance.GetEnemyData(type);
        jumpArgs = scriptable.jumpArgs;
    }

    
    new void Start(){
        base.Start();
        isground = false;
        target = base.Player.position;
        if(baseArgs.isGround){
            target.y = -4.3f + GetComponent<BoxCollider2D>().size.y/2 ;
            //isground = true;
        }
    }
  
    
    // Update is called once per frame
    void FixedUpdate()
    {   

        if(Vector3.Distance(transform.position, target) > 0.9f){
            var step =  baseArgs.moveSpeed * Time.fixedDeltaTime; // calculate distance to move
            transform.position -= Vector3.right * step;
            //transform.position = Vector3.MoveTowards(transform.position, target, step);
            
            //Vector2 relative = new Vector2(  target.x - transform.position.x , target.y - transform.position.y ); 

            if(isground){
                Jump();
            }
            else{
                GetComponent<Rigidbody2D>().velocity += Vector2.down*jumpArgs.downForce ;
            }   
            //transform.rotation = Quaternion.FromToRotation(Vector3.left, relative);
            
            
            //Vector3 direction = player.position - transform.position;
        }
        
    }

    void Jump(){
        isground = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up*jumpArgs.jumpForce , ForceMode2D.Impulse);
    }

     private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.gameObject.CompareTag("ground")){
            isground = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
 
}
