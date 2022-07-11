using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHPProgressBar : MonoBehaviour
{
    public static BossHPProgressBar Instance { get { return _instance; } }
    private static BossHPProgressBar _instance;
    private UISlider hpSlider;
    private UILabel hpLabel;
    private int maxHP;

    void Awake()
    {
        _instance = this;
        hpSlider = GetComponent<UISlider>();
        hpLabel = transform.Find("HPLabel").GetComponent<UILabel>();
        this.gameObject.SetActive(false);
    }


    public void Show(int maxHP)
    {
        this.maxHP = maxHP;
        this.gameObject.SetActive(true);
        UpdateShow(maxHP);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void UpdateShow(int currentHP)
    {
        if (currentHP <= 0) currentHP = 0;
        hpSlider.value = currentHP / (float)maxHP;
        hpLabel.text = currentHP + "/" + maxHP;
    }
}
