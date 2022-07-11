using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel Instance
    {
        get
        {
            return _instance;
        }
    }
    private static GameOverPanel _instance;
    private TweenScale tween;
    private UILabel label;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        tween = GetComponent<TweenScale>();
        label = transform.Find("Label").GetComponent<UILabel>();
    }

    public void Show(string str)
    {
        label.text = str;
        tween.PlayForward();
    }

    public void Hide()
    {
        tween.PlayReverse();
    }

    public void OnBackButtonClick()
    {
        Hide();
        Destroy(GameController.Instance.gameObject);
        AsyncOperation operation = Application.LoadLevelAsync(1);
        LoadSceneProgressBar._instance.Show(operation);
    }
}
