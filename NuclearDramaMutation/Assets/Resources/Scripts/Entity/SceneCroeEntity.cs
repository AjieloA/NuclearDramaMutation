using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCroeEntity : Singleton<SceneCroeEntity>
{
    public Dictionary<string, MapNodeData> NodeVecToDateDic = new Dictionary<string, MapNodeData>();
    public Dictionary<string, MapNodeData> NodeIdToDataDic = new Dictionary<string, MapNodeData>();
    public Vector3 onClickVect;
    public string onClickNode;
}
