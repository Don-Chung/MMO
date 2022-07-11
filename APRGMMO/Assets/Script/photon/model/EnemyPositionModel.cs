using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionModel
{
    public List<EnemyPositionProperty> list = new List<EnemyPositionProperty>();
}

public class EnemyPositionProperty
{
    public string guid;
    public Vector3Obj position;
    public Vector3Obj eulerAngles;
}