using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clicky_btn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    public Image btn_img;
    public Sprite _default , _pressed , _disable;
    //[SerializeField] private AudioClip _compressClip , _uncompressClip;
    //[SerializeField] private AudioSource _source;
    public virtual void OnPointerDown(PointerEventData eventData){
     
        btn_img.sprite = _pressed;

        //_source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData){
        btn_img.sprite = _default;
        //_source.PlayOneShot(_uncompressClip);
    }
}