using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MapGridManager : SceneCore
{
    private SceneCoreEntity sceneEntity;

    private void Awake()
    {
        Init();
        CreatHexGridForMap();
    }
    private void OnDestroy()
    {
        Debug.Log("end");
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneCore.Singletons.GetReAsyncLoadScene("FightOne", () =>
            {
                GetSceneEntity().NodeIdToDataDic.Clear();
                GetSceneEntity().NodeVecToDateDic.Clear();
                Application.Singletons.CreatMapManger();
            });
            Debug.LogError("重新加载");
        }
    }

    public void Init()
    {
        sceneEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCoreEntity;
        SetMapHorizontalCount(40);
        SetMapVerticalCount(20);
        SetHexLineLength(1);
        GetLinePrefab($"{TypeName.ResourcesTypeName.RPrefabs}Line", true);
        SetMeshMaterial($"{TypeName.ResourcesTypeName.RMaterials}Mesh", true);
        sceneEntity.meshParent = new GameObject("MeshParent");
    }

    public void CreatHexGridForMap()
    {
        sceneEntity.gridHeight = Mathf.Sqrt(sceneEntity.hexLineLength * sceneEntity.hexLineLength - (sceneEntity.hexLineLength / 2) * (sceneEntity.hexLineLength / 2)) * 2;//计算六边形高度；
        Vector3 _onePoint = new Vector3(sceneEntity.basePoint.x + sceneEntity.hexLineLength, sceneEntity.basePoint.y, sceneEntity.basePoint.z + sceneEntity.gridHeight / 2);//基于地图原点计算第一个六边形的中心坐标；
        for (int i = 0; i < sceneEntity.MapHorizontalCount; i++)
        {
            for (int j = 0; j < sceneEntity.MapVerticalCount; j++)
            {
                if (i % 2 == 0)
                {
                    CreatPoint(new Vector2(i, j), new Vector3(_onePoint.x + 1.5f * sceneEntity.hexLineLength * i, _onePoint.y, _onePoint.z + sceneEntity.gridHeight * j));
                }
                else
                {
                    CreatPoint(new Vector2(i, j), new Vector3(_onePoint.x + 1.5f * sceneEntity.hexLineLength * i, _onePoint.y, _onePoint.z + sceneEntity.gridHeight * j + sceneEntity.gridHeight / 2));
                }
            }
        }
        CreatLine();
    }

    //根据六边形中心点计算周边六个顶点的坐标；
    //_center为索引坐标；
    public void CreatPoint(Vector2 _center, Vector3 _point)
    {
        List<Vector3> _nodePoints = new List<Vector3>();
        _nodePoints.Add(_point);//中心点；
        _nodePoints.Add(new Vector3(_point.x - sceneEntity.hexLineLength / 2, _point.y, _point.z - sceneEntity.gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x - sceneEntity.hexLineLength, _point.y, _point.z));
        _nodePoints.Add(new Vector3(_point.x - sceneEntity.hexLineLength / 2, _point.y, _point.z + sceneEntity.gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x + sceneEntity.hexLineLength / 2, _point.y, _point.z + sceneEntity.gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x + sceneEntity.hexLineLength, _point.y, _point.z));
        _nodePoints.Add(new Vector3(_point.x + sceneEntity.hexLineLength / 2, _point.y, _point.z - sceneEntity.gridHeight / 2));
        sceneEntity.NodeIdToDataDic.Add($"{_center}", new MapNodeData(_center, _point, _nodePoints, TypeName.NodeTypeName.Empty));
        sceneEntity.NodeVecToDateDic.Add($"{_point}", new MapNodeData(_center, _point, _nodePoints, TypeName.NodeTypeName.Empty));
    }

    public void CreatLine()
    {
        GameObject _lineParent = new GameObject("LineParent");
        _lineParent.transform.position = new Vector3(0, 0, 0);
        foreach (var _item in sceneEntity.NodeIdToDataDic)
        {
            Vector3[] _vertives = new Vector3[7];
            _vertives[0] = sceneEntity.NodeIdToDataDic[_item.Key].GetNodePoints[0];
            GameObject _game = Instantiate(sceneEntity.linePrefab);
            LineRenderer _lineRenderer = _game.transform.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 6;
            for (int i = 1; i < sceneEntity.NodeIdToDataDic[_item.Key].GetNodePoints.Count; i++)
            {
                _lineRenderer.SetPosition(i - 1, sceneEntity.NodeIdToDataDic[_item.Key].GetNodePoints[i]);
                _vertives[i] = sceneEntity.NodeIdToDataDic[_item.Key].GetNodePoints[i];
            }
            _game.transform.parent = _lineParent.transform;
            CreatMesh(_vertives, StrToVector2(_item.Key));
        }
        //CreatStartAndEnd();
        CreatMonsterEndPoint();
        CreatMonsterStartPoint("MetalonGreen", 10, 2);
        EventCore.Instance.AddEventListener(TypeName.EventTypeName.CreatMonter, () =>
        {

            CreatMonsterStartPoint("MetalonRed", 10, 2);

        });
        //UICore.Singletons.OpenUIWidget("HeadMenu", UIShowLayer.DiaLog, (game) =>{});
        StartCoroutine(UICore.Singletons.AsyncLoadUIPrefabIE("HeadMenu", UIShowLayer.DiaLog, (game) => { }));
    }
    public void CreatMesh(Vector3[] _vertives, Vector2 _vec2)
    {
        Mesh _mesh;
        MeshFilter _filter;
        GameObject _game = new GameObject();
        _game.transform.parent = sceneEntity.meshParent.transform;
        _game.name = _vec2.ToString();
        _game.AddComponent<MeshFilter>();
        _game.AddComponent<MeshRenderer>();
        _filter = _game.GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _filter.mesh = _mesh;
        _mesh.name = "HexMesh";
        _mesh.vertices = _vertives;
        int[] triangles = new int[6 * 3]{
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,5,
            0,5,6,
            0,6,1
        };
        _mesh.triangles = triangles;
        _game.GetComponent<MeshRenderer>().material = sceneEntity.material;
        _game.AddComponent<MeshCollider>();
        _game.AddComponent<NodeMouseOnClick>();
        float _x = sceneEntity.NodeIdToDataDic[$"{_vec2}"].GetNodeVector.x;
        float _y = sceneEntity.NodeIdToDataDic[$"{_vec2}"].GetNodeVector.z;
    }
    public void CreatMonsterEndPoint()
    {
        //生成行数据
        int _rowEnd = CreatNum(sceneEntity.MapHorizontalCount);
        //生成列数据
        int _colEnd = CreatNum(sceneEntity.MapVerticalCount);
        if (!sceneEntity.NodeIdToDataDic[$"{new Vector2(_rowEnd, _colEnd)}"].GetIsCreat())
        {
            CreatMonsterEndPoint();
            return;
        }
        //存储数据
        sceneEntity.endPoint = $"{new Vector2(_rowEnd, _colEnd)}";
        sceneEntity.endVec = new Vector2(_rowEnd, _colEnd);

        GetVFXToScene("End", sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].GetNodeVector, (game) => { });

        sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].ReadWriteNodeState = TypeName.NodeTypeName.Point;


        //生成数据
        int CreatNum(int _section)
        {
            int _one = Random.Range(0, _section);
            return _one;
        }
    }
    public void CreatMonsterStartPoint(string _monsterName,int _monsterCount,float _executeTime)
    {
        Vector2 _vec = CreatNum(sceneEntity.endVec);
        if (!sceneEntity.NodeIdToDataDic[$"{_vec}"].GetIsCreat())
        {
            CreatMonsterStartPoint(_monsterName,_monsterCount,_executeTime);
            return;
        }
        //存储数据
        sceneEntity.startPoint = $"{_vec}";
        sceneEntity.startVec =_vec;

        GetVFXToScene("Start", sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].GetNodeVector, (game) => { });
        sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].ReadWriteNodeState = TypeName.NodeTypeName.Point;

        AStarMove(sceneEntity.startPoint, sceneEntity.endPoint, () =>
        {
            Application.Singletons.CreatRoleManager();
            GameObject _startPoint = new GameObject("StartPoint");
            GameObject _endPoint = new GameObject("EndPoint");
            _startPoint.transform.position = sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].GetNodeVector;
            _endPoint.transform.position = sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].GetNodeVector;
            _startPoint.transform.AddComponent<CreatMonsterManager>();
            CreatMonsterManager _creatMonster = _startPoint.transform.GetComponent<CreatMonsterManager>();
            _creatMonster.Init(_monsterName, _monsterCount, _executeTime);
        });

        //生成数据
        Vector2 CreatNum(Vector2 _vec)
        {
            int _eX = (int)_vec.x;
            int _eY = (int)_vec.y;
            int _x = 0;
            int _y = 0;
            return new Vector2(CreatNumXY(_eX, sceneEntity.MapHorizontalCount), CreatNumXY(_eY, sceneEntity.MapVerticalCount));

            int CreatNumXY(int _one,int _section)
            {
                int _two = 0;
                if (_one <= _section / 2)
                    if (_one + _section / 2 < _section)
                        _two = Random.Range(_one + _section / 2, _section);
                    else
                        _two = _section - 1;
                else
                {
                    if (_one - _section / 2 > 0)
                        _two = Random.Range(0, _one - _section / 2);
                    else
                        _two = 0;
                }
                return _two;
            }
        }
    }


    /// <summary>
    /// 创建怪物生成点和结束点
    /// </summary>
    public void CreatStartAndEnd()
    {

        //生成行数据
        (int, int) _rowData = CreatNum(sceneEntity.MapHorizontalCount);
        int _rowStart = _rowData.Item1;
        int _rowEnd = _rowData.Item2;
        //生成列数据
        (int, int) _colData = CreatNum(sceneEntity.MapVerticalCount);
        int _colStart = _colData.Item1;
        int _colEnd = _colData.Item2;
        if (!sceneEntity.NodeIdToDataDic[$"{new Vector2(_rowStart, _colStart)}"].GetIsCreat() || !sceneEntity.NodeIdToDataDic[$"{new Vector2(_rowEnd, _colEnd)}"].GetIsCreat())
        {
            CreatStartAndEnd();
            return;
        }
        //存储数据
        sceneEntity.startPoint = $"{new Vector2(_rowStart, _colStart)}";
        sceneEntity.endPoint = $"{new Vector2(_rowEnd, _colEnd)}";
        
        //Debug.LogError($"startPoint>>{sceneEntity.startPoint}******endPoint>>{sceneEntity.endPoint}");
        //加载特效
        GetVFXToScene("Start", sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].GetNodeVector, (game) => { });
        GetVFXToScene("End", sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].GetNodeVector, (game) => { });
        //更改节点状态
        sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].ReadWriteNodeState = TypeName.NodeTypeName.Point;
        sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].ReadWriteNodeState = TypeName.NodeTypeName.Point;
        //
        AStarMove(sceneEntity.startPoint, sceneEntity.endPoint, () =>
        {
            Application.Singletons.CreatRoleManager();
            GameObject _startPoint = new GameObject("StartPoint");
            GameObject _endPoint = new GameObject("EndPoint");
            _startPoint.transform.position = sceneEntity.NodeIdToDataDic[sceneEntity.startPoint].GetNodeVector;
            _endPoint.transform.position = sceneEntity.NodeIdToDataDic[sceneEntity.endPoint].GetNodeVector;
            _startPoint.transform.AddComponent<CreatMonsterManager>();
            CreatMonsterManager _creatMonster = _startPoint.transform.GetComponent<CreatMonsterManager>();
            _creatMonster.Init("MetalonGreen", 10, 2);
        });


        //生成数据
        (int, int) CreatNum(int _section)
        {
            int _one = Random.Range(0, _section);
            int _two = 0;
            if (_one <= _section / 2)
                if (_one + _section / 2 < _section)
                    _two = Random.Range(_one + _section / 2, _section);
                else
                    _two = _section - 1;
            else
            {
                if (_one - _section / 2 > 0)
                    _two = Random.Range(0, _one - _section / 2);
                else
                    _two = 0;
            }
            return (_one, _two);
        }
    }
    /// <summary>
    /// string转换成vector2
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public Vector2 StrToVector2(string _str)
    {
        string[] _atrArray = _str.Split(',');
        int _x = int.Parse(_atrArray[0].Substring(1).Split('.')[0]);
        int _y = int.Parse(_atrArray[1].Split('.')[0]);
        return new Vector2(_x, _y);

    }

}