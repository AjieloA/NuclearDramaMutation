using System.Collections.Generic;
using UnityEngine;

public class SceneCoreEntity : BaseEntity
{
    public Dictionary<string, MapNodeData> NodeVecToDateDic = new Dictionary<string, MapNodeData>();
    public Dictionary<string, MapNodeData> NodeIdToDataDic = new Dictionary<string, MapNodeData>();
    public Vector3[] movePath;
    public Vector3 onClickVect;
    public string onClickNode;
    [Tooltip("地图原点")]
    public Vector3 basePoint;

    [Tooltip("六边形边长")]
    public float hexLineLength;

    [Tooltip("横向排列六边形数量")]
    public int MapHorizontalCount;

    [Tooltip("纵向排列六边形数量")]
    public int MapVerticalCount;

    public float gridHeight;


    /// <summary>
    /// 地图mesh的父物体
    /// </summary>
    public GameObject meshParent;
    /// <summary>
    /// line预制体
    /// </summary>
    public GameObject linePrefab;
    public Material material;
    /// <summary>
    /// 怪物生成点和终点
    /// </summary>
    public string endPoint, startPoint;
    public Vector2 endVec, startVec;
    public int coin = 10;
    public int killCount = 0;
    public UIHeadMenu headMenu;
    //todo 临时逻辑
    public void SetKillCount(int _killCount)
    {
        coin += 2;
        killCount += _killCount;
        if (headMenu == null)
            headMenu = UICore.Singletons.QueryComponent<UIHeadMenu>("HeadMenu", "HeadMenu");
        headMenu.Refresh();
    }
}
