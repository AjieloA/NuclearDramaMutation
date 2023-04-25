public class UIManager : UICore
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        OpenUIShow("PiMenu", UIShowLayer.DiaLog, (game) =>
        {
            game.GetComponent<UIPiMenu>().Refresh(0);
        });
        // SetShowTips("����һ������");
    }
}
