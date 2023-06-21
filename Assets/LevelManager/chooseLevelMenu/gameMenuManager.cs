using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Utils.Instance.SetTimer( ()=>{setBtn();}, 0.05f );
    }
    void setBtn()
    {   
        List<int> unlock_levels = gameDataRecorder.Instance.unlock_levels;
        GameObject [] all_choose_btns = GameObject.FindGameObjectsWithTag("choose_level_btn");
        
        foreach( GameObject g in all_choose_btns){
        
            clicky_choose_level_btn btn = g.transform.GetComponent<clicky_choose_level_btn>(); 

            if(unlock_levels.Contains( btn.level_id )){
                btn.unLock();
            }
            else{
                btn.Lock();
            }
        }

        //gameDataRecorder.Instance.unlock_levels;


    }

   
}
