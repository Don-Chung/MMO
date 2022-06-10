using System.Collections;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public static StartMenuController _instance;

    public TweenScale startPanelTween;
    public TweenScale loginPanelTween;
    public TweenScale registerPanelTween;
    public TweenScale serverPanelTween;
    public TweenPosition startpanelTweenPos;
    public TweenPosition characterselectTweenPos;
    public TweenPosition charactershowTweenPos;

    public UIInput usernameInputLogin;
    public UIInput passwordInputLogin;

    public UILabel usernameLableStart;
    public UILabel servernameLableStart;

    public static string username;
    public static string password;
    public static ServerProperty sp;

    public UIInput usernameInputRegister;
    public UIInput passwordInputRegister;
    public UIInput repasswordInputRegister;

    public UIGrid serverlistGrid;
    public GameObject serverItemRed;
    public GameObject serverItemGreen;

    public GameObject serverSelectedGo;

    private bool haveInitServerlist = false;

    public GameObject[] characterArray;
    public GameObject[] characterSelectArray;

    private GameObject characterSelected; // 当前选择的角色

    public UIInput characternameInput;
    public Transform characterSelectedParent;

    public UILabel nameLabelCharacterselect;
    public UILabel levelLabelCharacterselect;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        InitServerlist(); //初始化服务器列表
    }

    public void OnUsernameClick()
    {
        //输入账号进行登录
        startPanelTween.PlayForward();
        StartCoroutine(HidePanel(startPanelTween.gameObject));
        loginPanelTween.gameObject.SetActive(true);
        loginPanelTween.PlayForward();
    }

    public void OnServerClick()
    {
        //选择服务器
        startPanelTween.PlayForward();
        StartCoroutine(HidePanel(startPanelTween.gameObject));
        serverPanelTween.gameObject.SetActive(true);
        serverPanelTween.PlayForward();
    }

    public void OnEnterGameClick()
    {
        //1.连服务器，验证用户名和服务器
        //TODO

        //2.进入角色选择界面
        startpanelTweenPos.PlayForward();
        HidePanel(startpanelTweenPos.gameObject);
        characterselectTweenPos.gameObject.SetActive(true);
        characterselectTweenPos.PlayForward();
    }

    //隐藏面板
    IEnumerator HidePanel(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        go.SetActive(false);
    }

    public void OnLoginClick()
    {
        //得到用户名和密码 存储起来
        username = usernameInputLogin.value;
        password = passwordInputLogin.value;

        //返回开始界面
        loginPanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginPanelTween.gameObject));
        startPanelTween.gameObject.SetActive(true);
        startPanelTween.PlayReverse();

        usernameLableStart.text = username;
    }

    public void OnRegisterShowClick()
    {
        //隐藏当前面板，显示注册面板
        loginPanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginPanelTween.gameObject));
        registerPanelTween.gameObject.SetActive(true);
        registerPanelTween.PlayForward();
    }

    public void OnLoginCloseClick()
    {
        //返回开始界面
        loginPanelTween.PlayReverse();
        StartCoroutine(HidePanel(loginPanelTween.gameObject));
        startPanelTween.gameObject.SetActive(true);
        startPanelTween.PlayReverse();
    }

    public void OnCancelClick()
    {
        //隐藏当前注册面板
        registerPanelTween.PlayReverse();
        StartCoroutine(HidePanel(registerPanelTween.gameObject));
        //显示登录面板
        loginPanelTween.gameObject.SetActive(true);
        loginPanelTween.PlayForward();
    }
    public void OnRegisterCloseClick()
    {
        OnCancelClick();
    }
    public void OnRegisterAndLoginClick()
    {
        //1.本地校验，连接服务器进行验证
        //TODO
        //2.连接失败
        //TODO
        //3.连接成功
        //保存用户名和密码
        username = usernameInputRegister.value;
        password = passwordInputRegister.value;
        //返回到开始界面
        registerPanelTween.PlayReverse();
        StartCoroutine(HidePanel(registerPanelTween.gameObject));
        startPanelTween.gameObject.SetActive(true);
        startPanelTween.PlayReverse();

        usernameLableStart.text = username;
    }

    public void InitServerlist()
    {
        if (haveInitServerlist) return;

        //1.连接服务器 取得游戏服务器列表信息
        //TODO

        //2.根据上面信息 添加服务器列表

        for(int i = 0; i < 20; ++i)
        {
            //public string ip = "127.0.0.1:9080";
            //public string Name = "1区 水帘洞";
            //public int count = 120;
            string ip = "127.0.0.1:9080";
            string name = (i + 1).ToString() + "区 花果山水帘洞";
            int count = Random.Range(0, 100);
            GameObject go;
            if (count > 50)
            {
                //火爆
                go = NGUITools.AddChild(serverlistGrid.gameObject, serverItemRed);

            }
            else
            {
                //流畅
                go = NGUITools.AddChild(serverlistGrid.gameObject, serverItemGreen);
            }
            ServerProperty sp = go.GetComponent<ServerProperty>();
            sp.ip = ip;
            sp.Name = name;
            sp.count = count;
            serverlistGrid.AddChild(go.transform);
        }
        
    }

    public void OnServerselect(GameObject serverGo)
    {
        sp = serverGo.GetComponent<ServerProperty>();
        serverSelectedGo.GetComponent<UISprite>().spriteName = serverGo.GetComponent<UISprite>().spriteName;
        serverSelectedGo.transform.Find("Label").GetComponent<UILabel>().text = sp.Name;
    }

    public void OnServerpanelClose()
    {
        //隐藏服务器列表
        serverPanelTween.PlayReverse();
        StartCoroutine(HidePanel(serverPanelTween.gameObject));
        //显示开始界面
        startPanelTween.gameObject.SetActive(true);
        startPanelTween.PlayReverse();
        servernameLableStart.text = sp.Name;
    }

    public void OnCharacterClick(GameObject go)
    {
        if(go == characterSelected)
        {
            return;
        }
        iTween.ScaleTo(go, new Vector3(1.2f, 1.2f, 1.2f), 0.5f);

        if(characterSelected != null)
        {
            iTween.ScaleTo(characterSelected, new Vector3(1f, 1f, 1f), 0.5f);
        }
        characterSelected = go;
    }

    //点击了角色切换按钮
    public void OnButtonChangecharacterClick()
    {
        //隐藏自身面板
        characterselectTweenPos.PlayReverse();
        HidePanel(characterselectTweenPos.gameObject);
        //
        charactershowTweenPos.gameObject.SetActive(true);
        charactershowTweenPos.PlayForward();
    }

    public void OnCharactershowButtonSureClick()
    {
        //判断姓名输入的是否正确
        //判断是否选择角色
        //TODO

        int index = -1;
        for(int i = 0; i < characterArray.Length; ++i)
        {
            if(characterArray[i] == characterSelected)
            {
                index = i; 
                break;
            }
        }
        if (index == -1)
        {
            return;
        }
        GameObject.Destroy(characterSelectedParent.GetComponentInChildren<Animation>().gameObject);//销毁现有角色
        //创建新选择的角色
        GameObject go = GameObject.Instantiate(characterSelectArray[index], Vector3.zero, Quaternion.identity);
        go.transform.parent = characterSelectedParent;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        //更新角色名字和等级
        nameLabelCharacterselect.text = characternameInput.value;
        levelLabelCharacterselect.text = "Lv.1";

        OnCharactershowButtonBackClick();
    }
    public void OnCharactershowButtonBackClick()
    {
        charactershowTweenPos.PlayReverse();
        HidePanel(charactershowTweenPos.gameObject);

        characterSelected.gameObject.SetActive(true);
        characterselectTweenPos.PlayForward();
    }
}
