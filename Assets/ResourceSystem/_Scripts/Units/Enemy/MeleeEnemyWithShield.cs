using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyWithShield : MeleeEnemy
{
     [System.Serializable]
    public struct ShieldArgs
    {
        public float shieldHp;

        
    }
    public GameObject shield;
    public ShieldArgs shieldArgs;
    float shieldHp ;    

    Material shield_material ;
    Color shield_color ;
    protected override void Awake()
    {
        base.Awake();
        shield_material  = shield.transform.GetComponent<SpriteRenderer>().material;
        shield_color = shield_material.GetColor("_tint");
        shield_color.a = 0 ;
    }   

    protected override void Update(){
        base.Update();
        shield_color.a = Mathf.Clamp01(shield_color.a - Time.deltaTime);
        shield_material.SetColor("_tint" , shield_color );  
    }

    public override void TakeDamage(int d){
        
        if( shieldHp > 0){
           shieldHp -= d;
           
           shield_color.a = 1;

           if(shieldHp <= 0){
              Destroy(shield);
           }
        }
        else{
            health -= d;
            
            base.tint_color.a = 1 ;
            if(health < 0){
                _Destroy();
            }
        }
 
    }

    public override void SetData(EnemyType type)
    {
        base.SetData(type);
        ScriptableShieldMeleeEnemy scriptable = (ScriptableShieldMeleeEnemy)ResourceSystem.Instance.GetEnemyData(type);
        shieldArgs = scriptable.shieldArgs;
        shieldHp  = shieldArgs.shieldHp;
    }


}
