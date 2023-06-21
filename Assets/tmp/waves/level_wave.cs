using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_wave : MonoBehaviour
{   
   [HideInInspector] public Vector3 zombie_pos = new Vector3(9.35f,-4f,0f);
   public void stopLevel(){
      StopAllCoroutines();
   }
}
