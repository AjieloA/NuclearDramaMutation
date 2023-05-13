using System.Collections.Generic;
using UnityEngine;

public class SceneCoreEntity : BaseEntity
{
    public Dictionary<string, MapNodeData> NodeVecToDateDic = new Dictionary<string, MapNodeData>();
    public Dictionary<string, MapNodeData> NodeIdToDataDic = new Dictionary<string, MapNodeData>();
    public Vector3[] movePath;
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


    /// <summary>
    /// ��ͼmesh�ĸ�����
    /// </summary>
    public GameObject meshParent;
    /// <summary>
    /// lineԤ����
    /// </summary>
    public GameObject linePrefab;
    public Material material;
    /// <summary>
    /// �������ɵ���յ�
    /// </summary>
    public string endPoint, startPoint;
    public Vector2 endVec, startVec;
    public int coin = 10;
    public int killCount = 0;
    public UIHeadMenu headMenu;
    //todo ��ʱ�߼�
    public void SetKillCount(int _killCount)
    {
        coin += 2;
        killCount += _killCount;
        if (headMenu == null)
            headMenu = UICore.Singletons.QueryComponent<UIHeadMenu>("HeadMenu", "HeadMenu");
        headMenu.Refresh();
    }
}
