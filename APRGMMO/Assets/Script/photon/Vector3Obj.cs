using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Obj
{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public Vector3Obj()
    {

    }
    public Vector3Obj(Vector3 temp)
    {
        x = temp.x;
        y = temp.y;
        z = temp.z;
    }

    public Vector3 ToVector3()
    {
        Vector3 temp = Vector3.zero;
        temp.x = (float)x;
        temp.y = (float)y;
        temp.z = (float)z;
        return temp;
    }
}
