using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterShow : MonoBehaviour
{
    public void OnPress(bool isPress)
    {
        if (isPress == false)
        StartMenuController._instance.OnCharacterClick(transform.parent.gameObject);
    }
}
