using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCroeEntity : BaseEntity
{
    public Dictionary<string, MapNodeData> NodeVecToDateDic = new Dictionary<string, MapNodeData>();
    public Dictionary<string, MapNodeData> NodeIdToDataDic = new Dictionary<string, MapNodeData>();
    public Vector3 onClickVect;
    public string onClickNode;
    [Tooltip("��ͼԭ��")]
    public Vector3 basePoint;

    [Tooltip("�����α߳�")]
    public float hexLineLength;

    [Tooltip("������������������")]
    public int MapHorizontalCount;

    [Tooltip("������������������")]
    public int MapVerticalCount;

    public float gridHeight;



    public GameObject meshParent;
    //��ҪLoad����Դ
    public GameObject linePrefab;
    public Material material;
}
