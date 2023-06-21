using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemEffect : MonoBehaviour
{   

    moving_path hline;
    public GameObject missile;
    // Start is called before the first frame update
    void Start()
    {
        hline = GameObject.Find("item_hline").GetComponent<moving_path>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    void UnlockFire(){
        player.Instance.canFire = true;
    }
    void LockFire(){
        player.Instance.canFire = false;
    }
}
