using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "ScriptableObjs/Enemy/Base")]
public class ScriptableEnemyBase : ScriptableObject
{
    public EnemyType enemyType;
    public Sprite sprite;
    public EnemyBase.BaseArgs baseArgs;
    public EnemyBase prefab;

    public RuntimeAnimatorController anim;

}

public enum EnemyType
{
    
    zombie,
    missile,
    brownMissile,
    boss1,
    bomb_low,
    bomb_mid,
    bomb_high,
    shield_zombie,
    big_robot_arm,
    boss2,
    flying_zombie_generator_fake,
    flying_zombie_generator,
    bomb_knight_big,
    bomb_knight_mid,

    boss3,

    gorilla_hand,

    BossFinal,
    missileBullet,

    ThrowingBomb,

    FasterMissile,
    HealMissile
}