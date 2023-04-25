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
        Btn04
    }
    private RectTransform rectTransform;
    public void Refresh(int _pattern)
    {
        OnclickInstall(_pattern);
        StartAnim();
    }
    public void OnclickInstall(int _pattern)
    {
        switch (_pattern)
        {
            case 0:
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    EndAnim();
                    OpenUIShow("IrregularCircle", UIShowLayer.Default, (game) => { });
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    EndAnim();
                    float _timeCount=0;
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
                QueryComponent<Button>(transform, Enums.Btn01).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                    EndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn02).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                    EndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn03).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                    EndAnim();
                });
                QueryComponent<Button>(transform, Enums.Btn04).onClick.AddListener(() =>
                {
                    SetShowTips(false, "功能暂未开放！");
                    EndAnim();
                });
                break;
        }
    }
    public void StartAnim()
    {
        rectTransform = QueryComponent<RectTransform>(transform, Enums.PiMenu);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.4f);
    }
    public void EndAnim()
    {
        transform.DOScale(new Vector3(0, 0, 0), 0.4f).OnComplete(() =>
        {
            CloseShowUI(transform);
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
}
