using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class level_statistic_row : MonoBehaviour
{
    public Image icon ;
    public TextMeshProUGUI num_text;
    public void setData(EnemyType e, int num){
        ScriptableEnemyBase eb = ResourceSystem.Instance.GetEnemyData(e);
        icon.sprite = eb.sprite;
        num_text.text = num.ToString();
    }
}
