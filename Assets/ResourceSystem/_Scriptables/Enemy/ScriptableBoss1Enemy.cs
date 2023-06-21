using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss1Enemy", menuName = "ScriptableObjs/Enemy/Boss1")]
public class ScriptableBoss1Enemy : ScriptableEnemyBase
{
    public Boss1.Boss1Args boss1Args;
    public GameObject hpBarObj;
}
