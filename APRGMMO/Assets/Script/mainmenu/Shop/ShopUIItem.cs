using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIItem : MonoBehaviour
{
    public int diamondChangeCount = 0;
    public int coinChangeCount = 0;

    public void OnBuyButtonClick()
    {
        ShopUI.Instance.OnBuy(coinChangeCount, diamondChangeCount);
    }
}
