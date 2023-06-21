using System;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public GameObject audioSourceTemplate;
    public static AudioManager instance ;
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return ;
        }
        DontDestroyOnLoad(gameObject);
        
        // foreach(Sound s  in sounds){
        //     s.source = gameObject.AddComponent<AudioSource>();
        //     s.source.clip = s.clip;
        //     s.source.volume = s.Volumn;
        //     s.source.pitch = s.pitch;
        //     s.source.loop = s.loop;

        // }
    }



    // Update is called once per frame
    public void Play(string name)
    {   
        Sound s = Array.Find(sounds , sound=>sound.name == name);
        if( s == null){
            return ;
        }

        GameObject copy_gameObj = Instantiate(audioSourceTemplate);
        s.source = copy_gameObj.GetComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.Volumn;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.Play();

        Destroy(copy_gameObj, s.clip.length);
            
        
    }
}
