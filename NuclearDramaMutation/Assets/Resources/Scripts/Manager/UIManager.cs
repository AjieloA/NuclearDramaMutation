using UnityEngine;

public class UIManager : UICore
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        GetUiCroeEntity().mainCanvas=transform.GetComponent<Canvas>();
        OpenUIShow("Init", UIShowLayer.Default, (game) =>
        {
        });
    }
    private void Start()
    {
        OpenUIShow("PiMenu", UIShowLayer.DiaLog, (game) =>
        {
            game.GetComponent<UIPiMenu>().Refresh(0);
        });
        // SetShowTips("这是一个测试");
    }
}
