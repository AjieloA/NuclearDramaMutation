using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeadMenu : UICore
{
    enum Enums
    {
        CoinTxt,
        QuitBtn,
        KillCountTxt

    }
    private SceneCroeEntity croeEntity;
    private Text coinTxt;
    private Text killCountTxt;
    private Button quitBtn;
    private void Awake()
    {
        croeEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCroeEntity;
        Init();
    }
    public void Refresh()
    {
        coinTxt.text = $"    Coin:{croeEntity.coin}";
        killCountTxt.text = $"击杀数量：{croeEntity.killCount}";
    }
    public void Init()
    {
        coinTxt = QueryComponent<Text>(transform, Enums.CoinTxt);
        killCountTxt = QueryComponent<Text>(transform, Enums.KillCountTxt);
        quitBtn = QueryComponent<Button>(transform, Enums.QuitBtn);
        coinTxt.text = $"    Coin:{croeEntity.coin}";
        killCountTxt.text = $"击杀数量：{0}";
        quitBtn.onClick.AddListener(() => {
            SceneCore.Singletons.QuitGame();
        });
    }
}
