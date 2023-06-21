using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random_point : moving_path
{

    public override Vector2 get_position(float t){
        int r = UnityEngine.Random.Range(0, pointers.Count);
        return pointers[r].position;
    }
}
