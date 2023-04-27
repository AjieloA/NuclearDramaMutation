using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapGridManager : SceneCore
{
    private SceneCroeEntity sceneEntity;

    private void Awake()
    {
        Init();
        CreatHexGridForMap();
    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void Init()
    {
        sceneEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCroeEntity;
        SetMapHorizontalCount(20);
        SetMapVerticalCount(20);
        SetHexLineLength(1);
        GetLinePrefab($"{TypeName.ResourcesTypeName.ResourcesPrefabs}Line", true);
        SetMeshMaterial($"{TypeName.ResourcesTypeName.ResourcesMaterials}Mesh", true);
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
    public Vector2 StrToVector2(string _str)
    {
        string[] _atrArray = _str.Split(',');
        int _x = int.Parse(_atrArray[0].Substring(1).Split('.')[0]);
        int _y = int.Parse(_atrArray[1].Split('.')[0]);
        return new Vector2(_x, _y);

    }
}