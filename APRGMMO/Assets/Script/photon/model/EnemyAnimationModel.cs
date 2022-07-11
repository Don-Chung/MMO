using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationModel
{
    public List<EnemyAnimationProperty> list = new List<EnemyAnimationProperty>();
}

public class EnemyAnimationProperty
{
    public string guid;
    public bool isIdle;
    public bool isWalk;
    public bool isAttack;
    public bool isDie;
    public bool isTakeDamage;
}
