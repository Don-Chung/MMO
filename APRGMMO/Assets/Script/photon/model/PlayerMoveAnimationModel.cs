using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAnimationModel
{
    public bool IsMove { get; set; }
    public DateTime Time { get; set; }

    public string time;

    public void SetTime(DateTime dateTime)
    {
        time = dateTime.ToString("yyyyMMddHHmmssffff");
    }

    public DateTime GetTime()
    {
        DateTime dt = DateTime.ParseExact(time, "yyyyMMddHHmmssffff", System.Globalization.CultureInfo.CurrentCulture);
        return dt;
    }
}
