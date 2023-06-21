using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fake_flying_zombie_generator_in_sub_level_1 : MonoBehaviour
{
    // Start is called before the first frame update
    bezier_curve curve;
    public Transform door_point;
    IEnumerator flying(){
        int counter = 0;    
        for(float t = 1 ; t >= 0 ; t-= 0.01f){
            transform.position = curve.get_position(t);
 
            if(counter == 25 || counter==75 || counter==10){
                int scale  =  UnityEngine.Random.Range(3, 5)  ;
                Vector2 force = scale*Vector2.up;
                float r = UnityEngine.Random.Range(-10f, 10f) ;
                force = Quaternion.AngleAxis(r, Vector3.forward) * force;
                monster_generator.Instance.generateEnemy(EnemyType.zombie, door_point.position , force);
            }
            counter += 1;
            yield return new WaitForSeconds(0.05f) ;
        }
        
        GetComponent<EnemyBase>()._Destroy(false);
    }
    void Start(){
        curve = GameObject.FindGameObjectWithTag("bezier_curve_1").GetComponent<bezier_curve>();   
        StartCoroutine(flying());
    }


}
