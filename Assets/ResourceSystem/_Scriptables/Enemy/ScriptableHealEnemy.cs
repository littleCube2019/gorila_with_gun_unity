using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEnemy", menuName = "ScriptableObjs/Enemy/Heal")]
public class ScriptableHealEnemy : ScriptableMeleeEnemy
{
    public HealEnemy.HealArgs healArgs;
}
