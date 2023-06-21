using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectManager : Singleton<effectManager>
{
    // Start is called before the first frame update

    public GameObject explosion;
    public GameObject heal_effect;
    public void explode( Vector3 pos ){
        
        GameObject e = Instantiate(explosion , pos , transform.rotation);
       
        e.SetActive(true);
        Utils.Instance.SetTimer( ()=>{Destroy(e);} , 2);
    }
    public void explode( Vector3 pos , Vector3 scale ){
        
        GameObject e = Instantiate(explosion , pos , transform.rotation);
        e.transform.localScale = scale ;
        e.SetActive(true);
        //Utils.Instance.SetTimer( ()=>{Destroy(e);} , 2);  Animation event!!
    }
    public void heal(Vector3 pos , Vector3 scale){
        GameObject e = Instantiate(heal_effect , pos , transform.rotation);
        float life_time = e.GetComponent<ParticleSystem>().main.startLifetime.constant;
        e.transform.localScale = scale ;
        e.SetActive(true);
        Destroy(e, life_time);
        
    }
}
