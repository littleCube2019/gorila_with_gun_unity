using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class skillSlot_UI : MonoBehaviour
{   
    public List<Image> star_slots;
    public List<int> price_list;
    public List<playerSkill_v2.SkillType> skillType_list;
    public Sprite null_star;
    public Sprite full_star;


    public TextMeshProUGUI price_text;
    public Image icon;    
    int lv = 0;

    

    void Start(){
        transform.GetComponent<Button>().onClick.AddListener(upgrade);
    }

    public void upgrade(){
        
        if( lv == 3) return ;


        player.Instance.playerSkills_v2.TryUnlockSkill( skillType_list[lv] , price_list[lv] , this );
        
    }

    public void updateVisual(){
        
        for(int i = 0 ; i < lv ; i ++){
            
            star_slots[i].sprite = full_star;
        }
        for(int i = lv ; i < 3 ; i ++ ){
            star_slots[i].sprite = null_star;
        }
        if(lv < 3){
            price_text.text = price_list[lv].ToString();
        }
        else{
            price_text.text = "Sold!";
        }
        // price_text = xxx
        // icon.sprite = xxx
    }
    public void level_up(){
        
        lv += 1;
       
    }
    public int get_level(){
        return lv;
    }
    
}
