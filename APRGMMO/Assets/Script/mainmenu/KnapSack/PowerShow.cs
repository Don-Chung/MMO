using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShow : MonoBehaviour
{
    private float startValue = 0;
    private int endValue = 0;
    private bool isStart = false;
    private UILabel numLabel;
    private bool isUp = true;
    private TweenAlpha tween;
    public int speed = 460;

    private void Awake()
    {
        numLabel = transform.Find("Label").GetComponent<UILabel>();
        tween = GetComponent<TweenAlpha>();
        EventDelegate ed = new EventDelegate(this, "OnTweenFinished");
        tween.onFinished.Add(ed);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isStart)
        {
            if (isUp)
            {
                startValue += speed * Time.deltaTime;
                if(startValue > endValue)
                {
                    isStart = false;
                    startValue = endValue;
                    tween.PlayReverse();
                }
            }
            else
            {
                startValue -= speed * Time.deltaTime;
                if (startValue < endValue)
                {
                    isStart = false;
                    startValue = endValue;
                    tween.PlayReverse();
                }
            }
            numLabel.text = ((int)startValue).ToString();
        }
    }

    public void OnTweenFinished()
    {
        if(isStart == false)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowPowerChange(int startValue, int endValue)
    {
        gameObject.SetActive(true);
        tween.PlayForward();
        this.startValue = startValue;
        this.endValue = endValue;
        if(endValue > startValue)
        {
            isUp = true;
        }
        else
        {
            isUp = false;
        }
        isStart = true;
    }
}
