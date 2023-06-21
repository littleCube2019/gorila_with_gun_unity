using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_panel : Singleton<shop_panel>
{
    // Start is called before the first frame update
    public List<GameObject> Panels;
    public GameObject pannel ; 
    public List<clicky_tag> toggleTags;
    public void Start(){
        player.Instance.playerSkills.OnSkillUnlocked += shopPannel_OnSkillUnlocked ;
        togglePanel(1);
    }
    public void shopPannel_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e){
        close();
    }
    public void close(){
        pannel.SetActive(false);
    }
    public void open(){
        togglePanel(1);
        pannel.SetActive(true);
    }
    public void togglePanel (int id){
        for(int i = 0 ; i < toggleTags.Count ; i++){
            toggleTags[i].setUnpressed();
        }
        
        toggleTags[id].setPressed();

        for(int i = 0 ; i < toggleTags.Count ; i++){
            Panels[i].SetActive(false);
        }
        Panels[id].SetActive(true);
       
    }



    
}   
