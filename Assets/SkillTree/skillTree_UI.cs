using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class skillTree_UI : MonoBehaviour
{   

    [SerializeField] private Material skillLockedMaterial;
    [SerializeField] private Material skillUnlockableMaterial;
    [SerializeField] private SkillUnlockPath[] skillUnlockPathArray;
    [SerializeField] private Sprite pathAvaliable;
    [SerializeField] private Sprite pathAlready;

    private playerSkill playerSkills;
    private List<SkillButton> skillButtonList;

    void Start(){
        playerSkills = player.Instance.playerSkills;
        SetPlayerSkills(playerSkills);
    }

    public void SetPlayerSkills(playerSkill playerSkills) {
        this.playerSkills = playerSkills;

        skillButtonList = new List<SkillButton>();
        
        skillButtonList.Add(new SkillButton(transform.Find("doubleShotBtn"), playerSkills, playerSkill.SkillType.doubleShot, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("tripleShotBtn"), playerSkills, playerSkill.SkillType.tripleShot, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("machineGunBtn"), playerSkills, playerSkill.SkillType.machineGun, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("spread_2Btn"), playerSkills, playerSkill.SkillType.spread_2, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("spread_3Btn"), playerSkills, playerSkill.SkillType.spread_3, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("shotGunBtn"), playerSkills, playerSkill.SkillType.shotGun, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("penetrate_1Btn"), playerSkills, playerSkill.SkillType.penetrate_1, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("penetrate_2Btn"), playerSkills, playerSkill.SkillType.penetrate_2, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("sniperBtn"), playerSkills, playerSkill.SkillType.sniper, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("laserBtn"), playerSkills, playerSkill.SkillType.laser, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("atk_upBtn"), playerSkills, playerSkill.SkillType.atk_up, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("small_explosionBtn"), playerSkills, playerSkill.SkillType.small_explosion, skillLockedMaterial, skillUnlockableMaterial));   
        skillButtonList.Add(new SkillButton(transform.Find("rocketBtn"), playerSkills, playerSkill.SkillType.rocket, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("bombBtn"), playerSkills, playerSkill.SkillType.bomb, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("max_hp_2Btn"), playerSkills, playerSkill.SkillType.max_hp_2, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("max_hp_3Btn"), playerSkills, playerSkill.SkillType.max_hp_3, skillLockedMaterial, skillUnlockableMaterial));
        skillButtonList.Add(new SkillButton(transform.Find("flamer_throwerBtn"), playerSkills, playerSkill.SkillType.flamer_thrower, skillLockedMaterial, skillUnlockableMaterial));




        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        //playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointsChanged;

        UpdateVisuals();
        //UpdateSkillPoints();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, playerSkill.OnSkillUnlockedEventArgs e) {
        UpdateVisuals();
    }

    private void UpdateVisuals(){
        foreach (SkillButton skillButton in skillButtonList) {
            skillButton.UpdateVisual();
        }

        foreach (SkillUnlockPath skillUnlockPath in skillUnlockPathArray) {
            //print(skillUnlockPath);
            skillUnlockPath.linkImage.color = new Color(1f,1f,1f,1f);
            if(playerSkills.IsSkillUnlocked(skillUnlockPath.nextSkill)){
                skillUnlockPath.linkImage.sprite = pathAlready;
            }
            else if(playerSkills.CanUnlock(skillUnlockPath.nextSkill)){
                skillUnlockPath.linkImage.sprite = pathAvaliable;
            }
            else{
                skillUnlockPath.linkImage.color = new Color(1f,1f,1f,0f);
            }
               // skillUnlockPath.linkImage.sprite = 
            
        }
    }

    private class SkillButton {

        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private playerSkill playerSkills;
        private playerSkill.SkillType skillType;
        private Material skillLockedMaterial;
        private Material skillUnlockableMaterial;

        public SkillButton(Transform transform, playerSkill playerSkills, playerSkill.SkillType skillType, Material skillLockedMaterial, Material skillUnlockableMaterial) {
            this.transform = transform;
            this.playerSkills = playerSkills;
            this.skillType = skillType;
            this.skillLockedMaterial = skillLockedMaterial;
            this.skillUnlockableMaterial = skillUnlockableMaterial;
           
            image = transform.GetComponent<Image>();
            //backgroundImage = transform.Find("background").GetComponent<Image>();
            
            transform.GetComponent<Button>().onClick.AddListener(() => {
                if (!playerSkills.IsSkillUnlocked(skillType)) {
                    // Skill not yet unlocked
                    if (!playerSkills.TryUnlockSkill(skillType)) {
                        print("Cannot unlock " + skillType + "!");
                       // Tooltip_Warning.ShowTooltip_Static("Cannot unlock " + skillType + "!");
                    }
                }
            });
        }

        public void UpdateVisual() {
            if (playerSkills.IsSkillUnlocked(skillType)) {
                Color tmp = image.color;
                tmp.a = 0;
                image.color = tmp; 
                                //backgroundImage.material = null;
            } else {
                if (playerSkills.CanUnlock(skillType)) {
                    image.material = skillUnlockableMaterial;
                    //backgroundImage.color = UtilsClass.GetColorFromString("4B677D");
                    transform.GetComponent<Button>().enabled = true;
                } else {
                    image.material = skillLockedMaterial;
                    //backgroundImage.color = new Color(.3f, .3f, .3f);
                    transform.GetComponent<Button>().enabled = false;
                }
            }

            
        }

    }

    [System.Serializable]
    public class SkillUnlockPath {
        public playerSkill.SkillType nextSkill;
        public Image linkImage;
    }
}
