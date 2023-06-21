using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class skillTree_UI_v2 : MonoBehaviour
{   

    [SerializeField] private Material skillLockedMaterial;
    [SerializeField] private Material skillUnlockableMaterial;
    [SerializeField] private SkillUnlockPath[] skillUnlockPathArray;
    [SerializeField] private Sprite pathAvaliable;
    [SerializeField] private Sprite pathAlready;

    private playerSkill_v2 playerSkills;
    private List<skillSlot_UI> skillButtonList;

    void Start(){
        playerSkills = player.Instance.playerSkills_v2;
        SetPlayerSkills(playerSkills);
    }

    public void SetPlayerSkills(playerSkill_v2 playerSkills) {
        this.playerSkills = playerSkills;

        skillButtonList = new List<skillSlot_UI>();
        
        skillButtonList.Add(transform.Find("machineGunBtn").GetComponent<skillSlot_UI>());
        skillButtonList.Add(transform.Find("shotGunBtn").GetComponent<skillSlot_UI>());
        skillButtonList.Add(transform.Find("sniperBtn").GetComponent<skillSlot_UI>());

        




        playerSkills.OnSkillUnlocked_v2 += UI_OnSkillUnlocked;
        //playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointsChanged;

        UpdateVisuals();
        //UpdateSkillPoints();
    }

    private void UI_OnSkillUnlocked(object sender, playerSkill_v2.OnSkillUnlockedEventArgs_v2 e) {
        UpdateVisuals();
    }

    private void UpdateVisuals(){
        foreach ( skillSlot_UI skillButton in skillButtonList) {
            skillButton.updateVisual();
        }
    }

   

    [System.Serializable]
    public class SkillUnlockPath {
        public playerSkill.SkillType nextSkill;
        public Image linkImage;
    }
}
