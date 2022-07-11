using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScreen : MonoBehaviour
{
    private static BloodScreen _instance;
    public static BloodScreen Instance
    {
        get { return _instance; }
    }
    private UISprite sprite;
    private TweenAlpha tweenAlpha;


    private void Awake()
    {
        _instance = this;
        sprite = this.GetComponent<UISprite>();
        tweenAlpha = this.GetComponent<TweenAlpha>();
    }

    public void Show()
    {
        sprite.alpha = 1;
        tweenAlpha.ResetToBeginning();
        tweenAlpha.PlayForward();
    }
}
