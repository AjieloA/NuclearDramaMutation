using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIIrregularCircle : UICore
{
    enum Enums
    {
        ProgressTxt
    }
    void Start()
    {
        transform.GetComponent<CanvasGroup>().DOFade(1, 0.8f).OnComplete(() => { });
        Query(transform, Enums.ProgressTxt).DOScale(new Vector3(1, 1, 1), 0.8f);
    }
    public void RefreshProgressTxt(float _num)
    {
        QueryComponent<Text>(transform, Enums.ProgressTxt).text = $"{_num}";
    }
}
