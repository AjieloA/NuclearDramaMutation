using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject GetLinePrefab(string _path,bool _isReinstall)
    {
        if (GetSceneEntity().linePrefab == null||_isReinstall)
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
    protected Material SetMeshMaterial(string _path,bool _isReinstall)
    {
        if (GetSceneEntity().material == null||_isReinstall)
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
        StartCoroutine(IEAsyncLoadScene(_sceneName,_action));
        return true;
    }

    public void GetQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }

    private IEnumerator IEAsyncLoadScene(string _sceneName, System.Action _action, params object[] _params)
    {
        Debug.Log("AsyncLoadScene_Button");
        AsyncOperation _async = SceneManager.LoadSceneAsync(_sceneName);
        _async.allowSceneActivation = false;
        while (!_async.isDone)
        {
            Debug.Log("_async.progress" + _async.progress);
            UICore.Singletons.QueryComponent<UIIrregularCircle>("IrregularCircle", "IrregularCircle").RefreshProgressTxt(_async.progress*100);
            _async.allowSceneActivation = true;
            yield return null;
        }
        UICore.Singletons.Query<UIIrregularCircle>("IrregularCircle").RefreshProgressTxt(100);

        UICore.Singletons.OpenUIShow("PiMenu", UIShowLayer.DiaLog, (game) =>
        {
            UICore.Singletons.ShowHide("PiMenu",(trans)=> { });
            game.GetComponent<UIPiMenu>().Refresh(1);
        });

        yield return new WaitForSeconds(1f);
        UICore.Singletons.CloseShowUI("IrregularCircle");
        UICore.Singletons.CloseShowUI("Init");
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
}
public class MapNodeData
{
    private Vector2 nodeId;
    private Vector3 nodeVector = new Vector3(0, 0, 0);
    private List<Vector3> nodePoints = new List<Vector3>();
    private List<Vector2> nodeAdjacentPoints = new List<Vector2>();
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

        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x, _nodeVector.y + 1));
        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x + 1, _nodeVector.y));
        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x + 1, _nodeVector.y - 1));
        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x, _nodeVector.y - 1));
        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x - 1, _nodeVector.y + 1));
        nodeAdjacentPoints.Add(new Vector2(_nodeVector.x - 1, _nodeVector.y));
    }

    public Vector3 GetNodeVector
    {
        get { return nodeVector; }
    }
    public List<Vector3> GetNodePoints
    {
        get { return nodePoints; }
    }
    public List<Vector2> GetNodeNeighborsPoints
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
        if (nodeState == TypeName.NodeTypeName.Empty || nodeState == TypeName.NodeTypeName.GreeCrystal || nodeState == TypeName.NodeTypeName.PurpleCrystal
                || nodeState == TypeName.NodeTypeName.RedCrystal || nodeState == TypeName.NodeTypeName.Gold || nodeState == TypeName.NodeTypeName.Iron
                || nodeState == TypeName.NodeTypeName.Copper || nodeState == TypeName.NodeTypeName.Silver)
        {
            _isMove = true;
        }
        return _isMove;
    }
    public Vector2 GetNodeId
    {
        get { return nodeId; }
    }
}