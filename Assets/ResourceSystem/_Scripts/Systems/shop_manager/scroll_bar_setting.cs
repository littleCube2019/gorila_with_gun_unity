using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class scroll_bar_setting : MonoBehaviour
{
    // Start is called before the first frame update

    void Start(){
        shop_manager.open_shop += settingScrollBar;
    }
    public void settingScrollBar(){
       
        GetComponent<Scrollbar>().value = 1;
    }

    private void OnDestroy() {
        shop_manager.open_shop -= settingScrollBar;
    }
}
