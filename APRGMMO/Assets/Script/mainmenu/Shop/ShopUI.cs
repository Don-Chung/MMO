using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;
    private TweenPosition tween;
    private GameObject coinScrollView;
    private GameObject diamondScrollView;
    void Awake()
    {
        Instance = this;
        tween = this.GetComponent<TweenPosition>();
        coinScrollView = transform.Find("CoinScrollView").gameObject;
        diamondScrollView = transform.Find("DiamondScrollView").gameObject;
        diamondScrollView.SetActive(false);
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnCloseClick()
    {
        Hide();
    }

    public void OnBuyCoinButtonClick()
    {
        coinScrollView.SetActive(true);
        diamondScrollView.SetActive(false);
    }

    public void OnBuyDiamondButtonClick()
    {
        diamondScrollView.SetActive(true);
        coinScrollView.SetActive(false);
    }

    public void OnBuy(int coinChangeCount, int diamondChangeCount)
    {
        bool isSuccess = PlayerInfo._instance.Exchange(coinChangeCount, diamondChangeCount);
        if (isSuccess)
        {
        }
        else
        {
            if (coinChangeCount < 0)
            {
                MessageManage._instance.ShowMessage("金币不足");
            }
            else
            {
                MessageManage._instance.ShowMessage("钻石不足");
            }
        }
    }
}
