using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCroeEntity : BaseEntity
{
    public Dictionary<string, MapNodeData> NodeVecToDateDic = new Dictionary<string, MapNodeData>();
    public Dictionary<string, MapNodeData> NodeIdToDataDic = new Dictionary<string, MapNodeData>();
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



    public GameObject meshParent;
    //需要Load的资源
    public GameObject linePrefab;
    public Material material;
}
