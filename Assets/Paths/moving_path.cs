using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_path : MonoBehaviour
{
    public List<Transform> pointers;


    public int get_num_points(){
        return pointers.Count;
    }

    public virtual void set_position(int index , Vector3 pos){
        pointers[index].position = pos;
    }

    public virtual Vector2 get_position(float t){
        return Vector2.zero;
    }
    private void OnDrawGizmos() {
        for(float t = 0 ;  t <= 1 ; t += 0.05f){
            Gizmos.DrawSphere(get_position(t),0.25f);
        }

       
    }
}
