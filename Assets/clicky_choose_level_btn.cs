using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clicky_choose_level_btn : clicky_btn
{
    public GameObject _lock;
    public int level_id;

    public void Lock(){
        GetComponent<Button>().interactable = false;
        _lock.SetActive(true);
    }

    public void unLock(){
        GetComponent<Button>().interactable = true;
        _lock.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData){
        if(_lock.activeInHierarchy) return ;
        btn_img.sprite = _pressed;

        //_source.PlayOneShot(_compressClip);
    }


}