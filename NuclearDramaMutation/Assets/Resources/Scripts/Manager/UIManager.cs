using UnityEngine;

public class UIManager : UICore
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        GetUiCroeEntity().mainCanvas=transform.GetComponent<Canvas>();
        OpenUIWidget("Init", UIShowLayer.Default, (game) =>
        {
        });
    }
    private void Start()
    {
        OpenUIWidget("PiMenu", UIShowLayer.DiaLog, (game) =>
        {
            game.GetComponent<UIPiMenu>().Refresh(0);
        });
        // SetShowTips("����һ������");
    }
}
