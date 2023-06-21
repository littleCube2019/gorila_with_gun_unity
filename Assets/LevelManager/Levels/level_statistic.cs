using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_statistic 
{   
  public Dictionary<EnemyType, int > killedEnemy;
  public Dictionary<EnemyType, int > currentEnemy;  


  bool isUpdatingEnemy;
  public level_statistic() {
    killedEnemy = new Dictionary<EnemyType, int>();
    currentEnemy = new Dictionary<EnemyType, int>();
    IEnumerable<EnemyType> l = helper.GetValues<EnemyType>() ;
    foreach( EnemyType t in l){
        killedEnemy[t] = 0 ;
        currentEnemy[t] = 0;
    }
   
    isUpdatingEnemy = false;
  }



 



}
