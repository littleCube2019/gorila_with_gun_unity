using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reward_manager : Singleton<reward_manager>
{
   

    public void generateRewards(RewardType type , int num , Vector3 pos ){
        for (int i = 0 ; i < num ; i ++){
            Vector3 epsilon = new Vector3( UnityEngine.Random.Range(-0.1f, 0.1f) 
                                        , 0
                                        , 0);
            ScriptableRewardBase scriptable =  ResourceSystem.Instance.GetRewardData(type);
            RewardBase spawnedReward = Instantiate(scriptable.prefab
                                    , pos + epsilon
                                    , Quaternion.identity);
            Vector2 tmp = 10*Vector2.up;
            
            
            float r = UnityEngine.Random.Range(-22.5f, 22.5f) ;
           
            tmp = Quaternion.AngleAxis(r, Vector3.forward) * tmp;
            spawnedReward.rb.AddForce(tmp , ForceMode2D.Impulse);
            spawnedReward.SetData(type);
        }
    }
}
