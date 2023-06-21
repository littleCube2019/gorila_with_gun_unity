using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombKnight : JumpEnemy
{
    public override void BeforeDied(){
        
        if (enemyType == EnemyType.bomb_knight_big ){
            for(int i = 0 ; i < 2 ; i++){
                
                
                monster_generator.Instance.generateEnemy(
                EnemyType.bomb_knight_mid
                , transform.position
                , helper.getRandomForce( Vector2.right, new Vector2Int(5,10), new Vector2(5,10) ));
            }
           
        }
        
        if( enemyType == EnemyType.bomb_knight_mid){
             for(int i = 0 ; i < 2 ; i++){
                  monster_generator.Instance.generateEnemy(
                    EnemyType.bomb_low
                , transform.position
                , helper.getRandomForce( Vector2.right, new Vector2Int(5,10), new Vector2(5,10) ));
            }
        }
    }
}