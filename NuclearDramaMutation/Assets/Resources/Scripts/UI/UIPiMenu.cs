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
    private void Awake()
    {
        rectTransform = QueryComponent<RectTransform>(transform, Enums.PiMenu);

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
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"��ʼ";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"�˳�";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"����";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"�ɾ�";
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    EndAndAnim();
                    OpenUIShow("IrregularCircle", UIShowLayer.Default, (game) =>
                    {
                        if (game != null)
                        {
                            SceneCore.Singletons.GetAsyncLoadScene("FightOne");
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
                    SetShowTips(false, "������δ���ţ�");
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    SetShowTips(false, "������δ���ţ�");
                });
                break;
            case 1:
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"����";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"�˳�";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"����";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"�ر�";
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
                    SetShowTips(false, "������δ���ţ�");
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    HideAndAnim();
                });
                break;
            case 2:
                
                QueryComponent<Text>(transform, Enums.Btn01Txt).text = $"Ͷʯ��";
                QueryComponent<Text>(transform, Enums.Btn02Txt).text = $"����";
                QueryComponent<Text>(transform, Enums.Btn03Txt).text = $"����";
                QueryComponent<Text>(transform, Enums.Btn04Txt).text = $"����";
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    if(SceneCroeEntity.Singletons.NodeIdToDataDic[SceneCroeEntity.Singletons.onClickNode].ReadWriteNodeState!=0)
                    {
                        SetShowTips(false, "�õ��޷����죡");
                        HideAndAnim();
                        return;
                    }
                    locadGame=Resources.Load<GameObject>("Prefabs/Turrent/FattyCatapultG") as GameObject;
                    cloneObject=Instantiate(locadGame);
                    cloneObject.transform.position = SceneCroeEntity.Singletons.onClickVect;
                    SceneCroeEntity.Singletons.NodeIdToDataDic[SceneCroeEntity.Singletons.onClickNode].ReadWriteNodeState = TypeName.NodeTypeName.AttTurret;
                    HideAndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    if (SceneCroeEntity.Singletons.NodeIdToDataDic[SceneCroeEntity.Singletons.onClickNode].ReadWriteNodeState != 0)
                    {
                        SetShowTips(false, "�õ��޷����죡");
                        HideAndAnim();
                        return;
                    }
                    locadGame = Resources.Load<GameObject>("Prefabs/Turrent/FattyMissileG") as GameObject;
                    cloneObject = Instantiate(locadGame);
                    cloneObject.transform.position = SceneCroeEntity.Singletons.onClickVect;
                    HideAndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn03).onClick.AddListener(() =>
                {
                    if (SceneCroeEntity.Singletons.NodeIdToDataDic[SceneCroeEntity.Singletons.onClickNode].ReadWriteNodeState != 0)
                    {
                        SetShowTips(false, "�õ��޷����죡");
                        HideAndAnim();
                        return;
                    }
                    locadGame = Resources.Load<GameObject>("Prefabs/Turrent/FattyMissileGSingle") as GameObject;
                    cloneObject = Instantiate(locadGame);
                    cloneObject.transform.position = SceneCroeEntity.Singletons.onClickVect;
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
        Application.Quit();
#endif
    }
    private void OnEnable()
    {
        StartAnim();
    }
}
