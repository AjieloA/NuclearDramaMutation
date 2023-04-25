using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITips : UICore
{
    enum Enums
    {
        TipsTxt
    }
    public void Refresh(string _tips)
    {
        UICore.Singletons.QueryComponent<Text>(transform, Enums.TipsTxt).text = _tips;
        Anim();
    }
    public void Anim()
    {
        var _do = DOTween.Sequence();
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        _do.Append(rectTransform.DOAnchorPos(new Vector2(0, 0), 0.3f));
        _do.AppendInterval(0.3f);
        _do.Append(rectTransform.DOAnchorPos(new Vector2(0, Screen.height+100), 0.2f).OnComplete(() =>
        {
            CloseShowUI(transform);
        }));

    }
}
