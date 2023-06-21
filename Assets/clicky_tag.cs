using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clicky_tag : MonoBehaviour
{
    [SerializeField] private Image btn_img;
    [SerializeField] private Sprite _default , _pressed;
    //[SerializeField] private AudioClip _compressClip , _uncompressClip;
    //[SerializeField] private AudioSource _source;
    public void setPressed(){
        btn_img.sprite = _pressed;
        GetComponent<Button>().enabled = false;
        //_source.PlayOneShot(_compressClip);
    }

    public void setUnpressed(){
        btn_img.sprite = _default;
        GetComponent<Button>().enabled = true;
    }

    /*public void OnPointerUp(PointerEventData eventData){
        btn_img.sprite = _default;
        //_source.PlayOneShot(_uncompressClip);
    }*/
}