using ARPGCommon.Model;
using System.Collections;
using System.Collections.Generic;
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
    public static List<Role> roleList = null;

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

    private LoginController loginController;
    private RegisterController registerController;
    private RoleController roleController;

    private void Awake()
    {
        _instance = this;
        loginController = this.GetComponent<LoginController>();
        registerController = this.GetComponent <RegisterController>();
        roleController = this.GetComponent <RoleController>();

        roleController.OnAddRole += OnAddRole;
        roleController.OnGetRole += OnGetRole;
        roleController.OnSelectRole += OnSelectRole;
    }

    private void Start()
    {
        //InitServerlist(); //初始化服务器列表
    }

    private void OnDestroy()
    {
        if(roleController != null)
        {
            roleController.OnAddRole -= OnAddRole;
            roleController.OnGetRole -= OnGetRole;
            roleController.OnSelectRole -= OnSelectRole;
        }
    }

    public void OnGetRole(List<Role> roleList)
    {
        StartMenuController.roleList = roleList;
        //得到了角色信息之后的处理
        if (roleList != null && roleList.Count > 0)
        {
            //得到角色显示界面
            Role role = roleList[0];
            ShowRole(role);
        }
        else
        {
            //进入角色创建界面
            ShowRoleAddPanel();
        }
    }

    public void OnAddRole(Role role)
    {
        if(roleList == null)
        {
            roleList = new List<Role>();
        }
        roleList.Add(role);
        ShowRole(role);
    }


    public void OnSelectRole()
    {
        characterselectTweenPos.gameObject.SetActive(false);
        AsyncOperation operation = Application.LoadLevelAsync(1);
        LoadSceneProgressBar._instance.Show(operation);
    }

    public void ShowRole(Role role)
    {
        PhotonEngine.Instance.role = role;
        ShowCharacterSelect();

        nameLabelCharacterselect.text = role.Name;
        levelLabelCharacterselect.text = "Lv." + role.Level.ToString();

        int index = -1;
        for (int i = 0; i < characterArray.Length; ++i)
        {
            if ((characterArray[i].name.IndexOf("boy") >= 0 && role.Isman) || (characterArray[i].name.IndexOf("girl") >= 0 && role.Isman == false))
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
    }

    public void OnGamePlay()
    {
        roleController.SelectRole(PhotonEngine.Instance.role);

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
        loginController.Login(username, password);

        //2.进入角色选择界面
        //startpanelTweenPos.PlayForward();
        //HidePanel(startpanelTweenPos.gameObject);
        //characterselectTweenPos.gameObject.SetActive(true);
        //characterselectTweenPos.PlayForward();
    }

    public void ShowCharacterSelect()
    {
        characterselectTweenPos.gameObject.SetActive(true);
        characterselectTweenPos.PlayForward();
    }

    public void HideStartPanel()
    {
        startpanelTweenPos.PlayForward();
        StartCoroutine(HidePanel(startpanelTweenPos.gameObject));
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
        username = usernameInputRegister.value;
        password = passwordInputRegister.value;
        string repassword = repasswordInputRegister.value;

        if (username == null || username.Length < 3)
        {
            MessageManage._instance.ShowMessage("用户名不能少于三个字符");
            return;
        }
        if(password == null || password.Length < 3)
        {
            MessageManage._instance.ShowMessage("密码不能少于三个字符");
            return;
        }

        if(password != repassword)
        {
            MessageManage._instance.ShowMessage("密码输入不一致");
            return;
        }

        registerController.Register(username, password, this);
        //3.连接成功
        //保存用户名和密码
        
        //返回到开始界面
        
        

        //usernameLableStart.text = username;
    }

    public void HideRegisterPanel()
    {
        registerPanelTween.PlayReverse();
        StartCoroutine(HidePanel(registerPanelTween.gameObject));
    }

    public void ShowStartPanel()
    {
        startPanelTween.gameObject.SetActive(true);
        startPanelTween.PlayReverse();
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

        //判断当前选择的角色模型是否已经创建，用名字判断
        foreach(var role in roleList)
        {
            if((role.Isman && go.name.IndexOf("boy") >= 0) || (role.Isman == false && go.name.IndexOf("girl") >= 0))
            {
                characternameInput.value = role.Name;
            }
        }
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

    public void ShowRoleAddPanel()
    {
        charactershowTweenPos.gameObject.SetActive(true);
        charactershowTweenPos.PlayForward();
    }

    public void OnCharactershowButtonSureClick()
    {
        //判断姓名输入的是否正确
        if(characternameInput.value.Length < 3)
        {
            MessageManage._instance.ShowMessage("角色的名字不能少于三个字符");
            return;
        }

        //判断当前角色是否已经创建
        Role role = null;
        foreach (var roleTemp in roleList)
        {
            if ((roleTemp.Isman && characterSelected.name.IndexOf("boy") >= 0) || (roleTemp.Isman == false && characterSelected.name.IndexOf("girl") >= 0))
            {
                characternameInput.value = roleTemp.Name;
                role = roleTemp;
            }
        }
        if(role == null)
        {
            Role roleAdd = new Role();
            roleAdd.Isman = characterSelected.name.IndexOf("boy") >= 0 ? true : false;
            roleAdd.Name = characternameInput.value;
            roleAdd.Level = 1;
            roleAdd.Exp = 0;
            roleAdd.Coin = 20000;
            roleAdd.Diamond = 1000;
            roleAdd.Energy = 100;
            roleAdd.Toughen = 50;
            roleController.AddRole(roleAdd);
        }
        else
        {
            ShowRole(role);
        }

        OnCharactershowButtonBackClick();
    }
    public void OnCharactershowButtonBackClick()
    {
        charactershowTweenPos.PlayReverse();
        HidePanel(charactershowTweenPos.gameObject);

        if(characterSelected != null)
            characterSelected.gameObject.SetActive(true);
        characterselectTweenPos.PlayForward();
    }
}
