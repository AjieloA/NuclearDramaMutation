using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCore : Singleton<SceneCore>
{
    public SceneCroeEntity GetSceneEntity()
    {
        return EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCroeEntity;
    }
    protected bool SetMapHorizontalCount(int _count)
    {
        GetSceneEntity().MapHorizontalCount = _count;
        return GetSceneEntity().MapHorizontalCount == _count;
    }

    protected bool SetMapVerticalCount(int _count)
    {
        GetSceneEntity().MapVerticalCount = _count;
        return GetSceneEntity().MapVerticalCount == _count;
    }
    /// <summary>
    /// 获取和加载LinePrefab
    /// </summary>
    /// <param name="_path">资源路径</param>
    /// <param name="_isReinstall">是否需要重新加载</param>
    /// <returns></returns>
    public GameObject GetLinePrefab(string _path, bool _isReinstall)
    {
        if (GetSceneEntity().linePrefab == null || _isReinstall)
        {
            GetSceneEntity().linePrefab = Resources.Load<GameObject>(_path);
            if (GetSceneEntity().linePrefab != null)
            {
                return GetSceneEntity().linePrefab;
            }
            else
            {
                Debug.LogError($"{_path}该路径下不存在指定加载的资源。");
                return null;
            }
        }
        else
        {
            return GetSceneEntity().linePrefab;
        }
    }
    /// <summary>
    /// 获取和加载MeshMaterial
    /// </summary>
    /// <param name="_path">资源路径</param>
    /// <param name="_isReinstall">是否需要重新加载</param>
    /// <returns></returns>
    protected Material SetMeshMaterial(string _path, bool _isReinstall)
    {
        if (GetSceneEntity().material == null || _isReinstall)
        {
            GetSceneEntity().material = Resources.Load<Material>(_path);
            if (GetSceneEntity().material != null)
            {
                return GetSceneEntity().material;
            }
            else
            {
                Debug.LogError($"{_path}该路径下不存在指定加载的资源。");
                return null;
            }
        }
        else
        {
            return GetSceneEntity().material;
        }
    }

    protected bool SetHexLineLength(int _length)
    {
        GetSceneEntity().hexLineLength = _length;
        return GetSceneEntity().hexLineLength == _length;
    }

    public bool GetAsyncLoadScene(string _sceneName, System.Action _action, params object[] _params)
    {
        StartCoroutine(IEAsyncLoadScene(_sceneName, _action));
        return true;
    }
    public bool GetReAsyncLoadScene(string _sceneName, System.Action _action, params object[] _params)
    {
        StartCoroutine(IEReAsyncLoadScene(_sceneName, _action));
        return true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    private IEnumerator IEAsyncLoadScene(string _sceneName, System.Action _action, params object[] _params)
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync(_sceneName);
        _async.allowSceneActivation = false;
        while (!_async.isDone)
        {
            //Debug.Log("_async.progress" + _async.progress);
            UICore.Singletons.QueryComponent<UIIrregularCircle>("IrregularCircle", "IrregularCircle").RefreshProgressTxt(_async.progress * 100);
            _async.allowSceneActivation = true;
            yield return null;
        }
        UICore.Singletons.Query<UIIrregularCircle>("IrregularCircle").RefreshProgressTxt(100);

        UICore.Singletons.OpenUIShow("PiMenu", UIShowLayer.DiaLog, (game) =>
        {
            UICore.Singletons.ShowHide("PiMenu", (trans) => { });
            game.GetComponent<UIPiMenu>().Refresh(1);
        });

        yield return new WaitForSeconds(1f);
        UICore.Singletons.CloseShowUI("IrregularCircle");
        UICore.Singletons.CloseShowUI("Init");
        _action?.Invoke();
        yield return null;
    }
    private IEnumerator IEReAsyncLoadScene(string _sceneName, System.Action _action, params object[] _params)
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync(_sceneName);
        _async.allowSceneActivation = false;
        while (!_async.isDone)
        {
            _async.allowSceneActivation = true;
            yield return null;
        }
        _action?.Invoke();
        yield return null;
    }
    protected Vector2 NameToVector(string _name)
    {
        Vector2 _vector = new Vector2(0, 0);
        string[] _nums = _name.Split(new char[4] { '(', '.', ',', ')' });
        for (int i = 0; i < _nums.Length; i++)
        {
            int _num = 0;
            if (i == 1)
            {
                int.TryParse(_nums[i], out _num);
                _vector.x = _num;
            }
            if (i == 3)
            {
                int.TryParse(_nums[i], out _num);
                _vector.y = _num;
            }

        }
        return _vector;
    }
    public void GetVFXToScene(string _name, Vector3 _vector, System.Action<GameObject> _action)
    {
        StartCoroutine(GetVFXToSceneIE(_name, _vector, _action));
    }
    public IEnumerator GetVFXToSceneIE(string _name, Vector3 _vector, System.Action<GameObject> _action)
    {
        ResourceRequest _request = Resources.LoadAsync(TypeName.ResourcesTypeName.ResourcesSceneVFX + _name);
        while (!_request.isDone)
        {
            yield return null;
        }
        GameObject _object = Instantiate(_request.asset as GameObject);
        _object.transform.position = _vector;
        _action?.Invoke(_object);
        yield return null;

    }
    /// <summary>
    /// A星寻路算法
    /// </summary>
    /// <param name="_key"></param>
    /// <param name="_endVec"></param>
    public void AStarMove(string _key, string _endVec,System.Action _action)
    {
        Dictionary<string, NodeDate> nodeDataDic = new Dictionary<string, NodeDate>();
        GameObject linePrefab = Instantiate(SceneCore.Singletons.GetLinePrefab($"{TypeName.ResourcesTypeName.ResourcesPrefabs}Line", true));
        LineRenderer lineRenderer = linePrefab.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.loop = false;
        lineRenderer.transform.position = new Vector3(0, 0, 0);
        InstallNodeData(nodeDataDic);

        Stack<string> _roadStack = new Stack<string>();
        string _currVec = _key;

        while (_currVec != _endVec)
        {
            string _vector = GetBestNode(_currVec);
            if (_vector == $"{new Vector3(9999, 9999, 9999)}")
            {
                Debug.LogError($"没有前往{{_endVec}}的路径");
                break;
            }
            _currVec = _vector;
        }
        _roadStack.Push(_currVec);
        lineRenderer.positionCount = _roadStack.Count;
        int _i = 0;
        GetSceneEntity().movePath = new Vector3[_roadStack.Count];
        while (_roadStack.Count > 0)
        {
            string _vec = _roadStack.Pop();
            lineRenderer.SetPosition(_i, GetSceneEntity().NodeIdToDataDic[_vec].GetNodeVector);
            GetSceneEntity().movePath[_i] = GetSceneEntity().NodeIdToDataDic[_vec].GetNodeVector;
            //Debug.Log($"_roadStack:{_vec}");
            _i++;
        }
        _action?.Invoke();


        //加载每个节点的相关信息
        void InstallNodeData(Dictionary<string, NodeDate> _nodeDataDic)
        {
            _nodeDataDic.Clear();
            foreach (var item in GetSceneEntity().NodeIdToDataDic)
            {
                _nodeDataDic.Add(item.Key, new NodeDate());
                _nodeDataDic[item.Key].isGoTo = GetSceneEntity().NodeIdToDataDic[item.Key].GetIsMove();
                _nodeDataDic[item.Key].nodeVector = item.Value.GetNodeVector;
                List<string> _neighborsPoints = GetSceneEntity().NodeIdToDataDic[item.Key].GetNodeNeighborsPoints;
                for (int i = 0; i < _neighborsPoints.Count; i++)
                    if (GetSceneEntity().NodeIdToDataDic.ContainsKey(_neighborsPoints[i]))
                        _nodeDataDic[item.Key].nodeNeighbors.Add(_neighborsPoints[i]);
            }
        }

        //获取当前节点的权值
        float GetF(float _g, float _h)
        {
            return _g + _h;
        }
        //计算当前节点邻节点的权值，并返回权值最小的节点
        string GetBestNode(string _currNodeVec)
        {
            string _bestNode = "";
            float _tempF = float.MaxValue;
            NodeDate _nodeDate = nodeDataDic[_currNodeVec];
            bool _isToEnd = false;
            float _lastF = _nodeDate.F;
            for (int i = 0; i < _nodeDate.nodeNeighbors.Count; i++)
            {
                //是否可以前往
                if (nodeDataDic[_nodeDate.nodeNeighbors[i]].isGoTo == false)
                {
                    nodeDataDic[_nodeDate.nodeNeighbors[i]].isDiscover = true;
                    continue;
                }
                //是否已经被探索
                if (nodeDataDic[_nodeDate.nodeNeighbors[i]].isDiscover == true)
                    continue;
                //计算当前节点的权值
                nodeDataDic[_nodeDate.nodeNeighbors[i]].F = GetF(_lastF, Vector3.Distance(nodeDataDic[_nodeDate.nodeNeighbors[i]].nodeVector, nodeDataDic[_endVec].nodeVector));
                if (nodeDataDic[_nodeDate.nodeNeighbors[i]].F < _tempF)
                {
                    _tempF = nodeDataDic[_nodeDate.nodeNeighbors[i]].F;
                    _bestNode = _nodeDate.nodeNeighbors[i];
                    _isToEnd = true;
                }
            }
            if (_isToEnd)
            {
                _nodeDate.isDiscover = true;
                _roadStack.Push(_currNodeVec);
                return _bestNode;
            }
            else
            {
                if (_roadStack.Count > 0)
                {
                    _nodeDate.isDiscover = true;
                    return _roadStack.Pop();
                }
                else
                    return $"{new Vector3(9999, 9999, 9999)}";
            }

        }







    }

    
}
public class NodeDate
{
    public bool isGoTo = true;//是否可以前往
    public bool isDiscover = false;//是否已经被探测过
    public Vector3 nodeVector;
    public List<string> nodeNeighbors = new List<string>();
    public float F = 0;
}
public class MapNodeData
{
    private Vector2 nodeId;
    private Vector3 nodeVector = new Vector3(0, 0, 0);
    private List<Vector3> nodePoints = new List<Vector3>();
    private List<string> nodeAdjacentPoints = new List<string>();
    private TypeName.NodeTypeName nodeState;
    public MapNodeData(Vector2 _nodeId, Vector3 _nodeVector, List<Vector3> _nodePoints, TypeName.NodeTypeName _nodeState)
    {
        nodePoints.Clear();
        nodeAdjacentPoints.Clear();
        nodeId = _nodeId;
        nodeVector = _nodeVector;
        nodePoints = _nodePoints;
        InstallAdjacentPoints(_nodeId);
        nodeState = _nodeState;
    }
    private void InstallAdjacentPoints(Vector2 _nodeVector)
    {

        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x, _nodeVector.y + 1)}");
        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x + 1, _nodeVector.y)}");
        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x + 1, _nodeVector.y - 1)}");
        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x, _nodeVector.y - 1)}");
        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x - 1, _nodeVector.y + 1)}");
        nodeAdjacentPoints.Add($"{new Vector2(_nodeVector.x - 1, _nodeVector.y)}");
    }

    public Vector3 GetNodeVector
    {
        get { return nodeVector; }
    }
    public List<Vector3> GetNodePoints
    {
        get { return nodePoints; }
    }
    public List<string> GetNodeNeighborsPoints
    {
        get { return nodeAdjacentPoints; }
    }
    public TypeName.NodeTypeName ReadWriteNodeState
    {
        get { return nodeState; }
        set { nodeState = value; }
    }
    public bool GetIsMove()
    {
        bool _isMove = false;
        if (nodeState == TypeName.NodeTypeName.Empty || nodeState == TypeName.NodeTypeName.Point)
        {
            _isMove = true;
        }
        return _isMove;
    }
    public bool GetIsCreat()
    {
        bool _isCreat = false;
        if (nodeState == TypeName.NodeTypeName.Empty)
        {
            _isCreat = true;
        }
        return _isCreat;
    }
    public Vector2 GetNodeId
    {
        get { return nodeId; }
    }
}
