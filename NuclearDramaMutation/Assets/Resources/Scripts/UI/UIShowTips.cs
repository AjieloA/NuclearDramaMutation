using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class UIShowTips : UICore
{
    enum Enums
    {
        ShowTips,
        BGImg,
        Head,
        HeadImg,
        HeadTxt,
        OkBtn,
        OkBtnTxt,
        TipsTxt
    }
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float DoSpeed = 0.8f;
    private Text text;
    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        canvasGroup = transform.GetComponent<CanvasGroup>();
        text = UICore.Singletons.QueryComponent<Text>(transform, Enums.TipsTxt);
    }
    private void Start()
    {
        StartAnim();
        InitOnclick();
    }
    public void Refresh(string _tips)
    {
        text.text= _tips;
    }
    public void StartAnim()
    {
        rectTransform.DOAnchorPos(new Vector2(0, 0), DoSpeed).SetUpdate(true);
        rectTransform.DOScale(new Vector3(1,1,1), DoSpeed).SetUpdate(true);
        canvasGroup.DOFade(1, DoSpeed);
    }
    public void EndAnim()
    {
        var _do = DOTween.Sequence();
        rectTransform.DOAnchorPos(new Vector2(0, 450), DoSpeed).SetUpdate(true);
        rectTransform.DOScale(new Vector3(0, 0, 0), DoSpeed).SetUpdate(true);
        canvasGroup.DOFade(0, DoSpeed).OnComplete(() => { CloseShow(); });
        void CloseShow()
        {
            //CloseShowUI("ShowTips");
        }
    }
    public void InitOnclick()
    {
        QueryComponent<Button>(transform, Enums.OkBtn).onClick.AddListener(() =>
        {
            EndAnim();
        });
        //Query(transform, Enum.OkBtn);
        //Query(transform, Enum.OkBtn).GetComponent<Button>().onClick.AddListener(() =>
        //{
        //    EndAnim();
        //});
    }
}
