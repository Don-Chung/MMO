using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI : MonoBehaviour
{
    public static SystemUI Instance
    {
        get
        {
            return _instance;
        }
    }

    private static SystemUI _instance;
    private TweenPosition tween;
    private UIButton audioButton;
    private UILabel audioLabel;
    public bool isAudioOpen = true;


    void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        audioButton = transform.Find("AudioButton").GetComponent<UIButton>();
        audioLabel = transform.Find("AudioLabel").GetComponent<UILabel>();
    }

    public void Show()
    {
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnAudioButtonClick()
    {
        if (isAudioOpen)
        {
            isAudioOpen = false;
            audioButton.normalSprite = "pic_音效关闭";
            audioLabel.text = "音效关闭";
        }
        else
        {
            isAudioOpen = true;
            audioButton.normalSprite = "pic_音效开启";
            audioLabel.text = "音效开启";
        }
    }

    public void OnContactButtonClick()
    {
        Application.OpenURL("http://www.baidu.com");
    }

    public void OnChangeRoleButtonClick()
    {
        Destroy(PhotonEngine.Instance.gameObject);
        AsyncOperation operation = Application.LoadLevelAsync(0);
        LoadSceneProgressBar._instance.Show(operation);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnCloseButtonClick()
    {
        Hide();
    }
}
