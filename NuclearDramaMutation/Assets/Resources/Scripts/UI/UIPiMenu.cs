using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPiMenu : UICore
{
    enum Enums
    {
        PiMenu,
        BGImg,
        Btn01,
        Btn02,
        Btn03,
        Btn04,
        Btn01Txt,
        Btn02Txt,
        Btn03Txt,
        Btn04Txt
    }
    private int pattern;
    private RectTransform rectTransform;
    private GameObject locadGame;
    private GameObject cloneObject;
    private SceneCroeEntity sceneEntity;
    private void Awake()
    {
        rectTransform = QueryComponent<RectTransform>(transform, Enums.PiMenu);

    }
    private void Start()
    {
        sceneEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCroeEntity;
    }
    public void Refresh(int _pattern)
    {
        pattern = _pattern;
        OnclickInstall(_pattern);
        StartAnim();
    }
    public void OnclickInstall(int _pattern)
    {
        QueryComponent<Button>(transform, Enums.Btn01).onClick.RemoveAllListeners();
        QueryComponent<Button>(transform, Enums.Btn02).onClick.RemoveAllListeners();
        QueryComponent<Button>(transform, Enums.Btn03).onClick.RemoveAllListeners();
        QueryComponent<Button>(transform, Enums.Btn04).onClick.RemoveAllListeners();
        switch (_pattern)

        {
            
            case 0:
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"开始";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"退出";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"静音";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"成就";
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {

                    EndAndAnim();
                    OpenUIShow("IrregularCircle", UIShowLayer.Default, (game) =>
                    {
                        if (game != null)
                        {
                            SceneCore.Singletons.GetAsyncLoadScene("FightOne", () =>
                            {
                                Application.Singletons.CreatMapManger();
                            });
                        }
                    });
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    EndAndAnim();
                    float _timeCount = 0;
                    DOTween.To(() => _timeCount, a => _timeCount = a, 1, 0.5f).OnComplete(() => EndGame());
                });
                QueryComponent<Button>(transform, Enums.Btn03).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                });
                break;
            case 1:
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"建造";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"退出";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"静音";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"关闭";
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    RefreshMenu(2);
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    EndAndAnim();
                    float _timeCount = 0;
                    DOTween.To(() => _timeCount, a => _timeCount = a, 1, 0.5f).OnComplete(() => EndGame());
                });
                QueryComponent<Button>(transform, Enums.Btn03).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    HideAndAnim();
                });
                break;
            case 2:
                
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"投石器";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"导弹";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"炮塔";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"返回";
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    if(sceneEntity.NodeIdToDataDic[sceneEntity.onClickNode].ReadWriteNodeState!=0)
                    {
                        SetShowTips(false, "该点无法建造！");
                        HideAndAnim();
                        return;
                    }
                    locadGame=Resources.Load<GameObject>("Prefabs/Turrent/FattyCatapultG") as GameObject;
                    cloneObject=Instantiate(locadGame);
                    cloneObject.transform.position = sceneEntity.onClickVect;
                    sceneEntity.NodeIdToDataDic[sceneEntity.onClickNode].ReadWriteNodeState = TypeName.NodeTypeName.Turret;
                    cloneObject.AddComponent<TurrentFight>();
                    HideAndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    if (sceneEntity.NodeIdToDataDic[sceneEntity.onClickNode].ReadWriteNodeState != 0)
                    {
                        SetShowTips(false, "该点无法建造！");
                        HideAndAnim();
                        return;
                    }
                    locadGame = Resources.Load<GameObject>("Prefabs/Turrent/FattyMissileG") as GameObject;
                    cloneObject = Instantiate(locadGame);
                    cloneObject.transform.position = sceneEntity.onClickVect;
                    HideAndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn03).onClick.AddListener(() =>
                {
                    if (sceneEntity.NodeIdToDataDic[sceneEntity.onClickNode].ReadWriteNodeState != 0)
                    {
                        SetShowTips(false, "该点无法建造！");
                        HideAndAnim();
                        return;
                    }
                    locadGame = Resources.Load<GameObject>("Prefabs/Turrent/FattyMissileGSingle") as GameObject;
                    cloneObject = Instantiate(locadGame);
                    cloneObject.transform.position = sceneEntity.onClickVect;
                    HideAndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    RefreshMenu(1);
                });
                break;
        }
    }
    public void StartAnim()
    {
        if (pattern == 0)
            transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.4f);
        else
            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.4f);
    }
    public void EndAndAnim()
    {
        transform.DOScale(new Vector3(0, 0, 0), 0.4f).OnComplete(() =>
        {
            CloseShowUI(transform);
        });
    }
    public void HideAndAnim()
    {
        transform.DOScale(new Vector3(0, 0, 0), 0.4f).OnComplete(() =>
        {
            ShowHide(transform.name, (trans) => {});
            
        });
    }
    public void RefreshMenu(int _menu)
    {
        transform.DOScale(new Vector3(0, 0, 0), 0.4f).OnComplete(() =>
        {
            Refresh(_menu);
            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.4f);
        });
    }
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
    private void OnEnable()
    {
        StartAnim();
    }
}
