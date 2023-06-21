using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seq_point : moving_path
{

    public Vector2 get_position(int t){
        return pointers[t].position;
    }
}
