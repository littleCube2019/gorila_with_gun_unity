using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitcher : Singleton<SceneSwitcher>
{
    public GameObject img;
    public Image black_curtain;
    public AnimationCurve curve;
    void Start(){
       
        StartCoroutine(FadeIn()); //for level start
    }

    void SaveDataInRecord(){
        
        player.Instance.endLevel();

        gameDataRecorder.Instance.weaponRecords = helper.DeepCloneDictionary< WeaponType, WeaponBase >(player.Instance.weaponRecords);
        gameDataRecorder.Instance.gear = player.Instance.gearNum;
        gameDataRecorder.Instance.weapons = helper.DeepCloneList<WeaponBase>(player.Instance.weapons);
        gameDataRecorder.Instance.weaponTypes = helper.DeepCloneList<WeaponType>(player.Instance.weapon_types);
    }

    public void SwitchSceneTo( string sceneName){
        if( player.Instance != null ){
            SaveDataInRecord();
        }
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn(){
        img.SetActive(true);
        float t = 1f;

        while(t > 0f){
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);

            black_curtain.color = new Color(0f,0f,0f,a);

            yield return 0; //next Frame
        }
        img.SetActive(false);
    }

    IEnumerator FadeOut(string sceneName){
        img.SetActive(true);
        float t = 0f;

        while(t < 1f){
            t += Time.deltaTime;
            float a = curve.Evaluate(t);

            black_curtain.color = new Color(0f,0f,0f,a);

            yield return 0;
        }
    
   
        SceneManager.LoadScene(sceneName);

    }
}
