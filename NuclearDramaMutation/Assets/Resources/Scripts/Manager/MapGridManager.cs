using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapGridManager : SceneCore
{
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                EventCore.Instance.TiggerEventListener("Test",NodeIdToDataDic[NameToVector(hit.transform.name)].GetNodeVector);

                //Debug.Log(NodeIdToDataDic[NameToVector(hit.transform.name)].GetNodeVector);
                //foreach (var item in NodeIdToDataDic[NameToVector(hit.transform.name)].GetNodeNeighborsPoints)
                //{
                //    Debug.Log("Ad:" + item);
                //}
            }
        }
    }

    public void Init()
    {
        SetMapHorizontalCount(20);
        SetMapVerticalCount(20);
        SetHexLineLength(1);
        GetLinePrefab($"{TypeName.ResourcesTypeName.ResourcesPrefabs}Line", true);
        SetMeshMaterial($"{TypeName.ResourcesTypeName.ResourcesMaterials}Mesh", true);
        meshParent = new GameObject("MeshParent");
    }

    public void CreatHexGridForMap()
    {
        gridHeight = Mathf.Sqrt(hexLineLength * hexLineLength - (hexLineLength / 2) * (hexLineLength / 2)) * 2;//计算六边形高度；
        Vector3 _onePoint = new Vector3(basePoint.x + hexLineLength, basePoint.y, basePoint.z + gridHeight / 2);//基于地图原点计算第一个六边形的中心坐标；
        for (int i = 0; i < MapHorizontalCount; i++)
        {
            for (int j = 0; j < MapVerticalCount; j++)
            {
                if (i % 2 == 0)
                {
                    CreatPoint(new Vector2(i, j), new Vector3(_onePoint.x + 1.5f * hexLineLength * i, _onePoint.y, _onePoint.z + gridHeight * j));
                }
                else
                {
                    CreatPoint(new Vector2(i, j), new Vector3(_onePoint.x + 1.5f * hexLineLength * i, _onePoint.y, _onePoint.z + gridHeight * j + gridHeight / 2));
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
        _nodePoints.Add(new Vector3(_point.x - hexLineLength / 2, _point.y, _point.z - gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x - hexLineLength, _point.y, _point.z));
        _nodePoints.Add(new Vector3(_point.x - hexLineLength / 2, _point.y, _point.z + gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x + hexLineLength / 2, _point.y, _point.z + gridHeight / 2));
        _nodePoints.Add(new Vector3(_point.x + hexLineLength, _point.y, _point.z));
        _nodePoints.Add(new Vector3(_point.x + hexLineLength / 2, _point.y, _point.z - gridHeight / 2));
        NodeIdToDataDic.Add(_center, new MapNodeData(_center, _point, _nodePoints, TypeName.NodeTypeName.Empty));
        NodeVecToDateDic.Add(_point, new MapNodeData(_center, _point, _nodePoints, TypeName.NodeTypeName.Empty));
    }

    public void CreatLine()
    {
        GameObject _lineParent = new GameObject("LineParent");
        _lineParent.transform.position = new Vector3(0, 0, 0);
        foreach (var _item in NodeIdToDataDic)
        {
            Vector3[] _vertives = new Vector3[7];
            _vertives[0] = NodeIdToDataDic[_item.Key].GetNodePoints[0];
            GameObject _game = Instantiate(linePrefab);
            LineRenderer _lineRenderer = _game.transform.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 6;
            for (int i = 1; i < NodeIdToDataDic[_item.Key].GetNodePoints.Count; i++)
            {
                _lineRenderer.SetPosition(i - 1, NodeIdToDataDic[_item.Key].GetNodePoints[i]);
                _vertives[i] = NodeIdToDataDic[_item.Key].GetNodePoints[i];
            }
            _game.transform.parent = _lineParent.transform;
            CreatMesh(_vertives, _item.Key);
        }
    }

    public void CreatMesh(Vector3[] _vertives, Vector2 _vec2)
    {
        Mesh _mesh;
        MeshFilter _filter;
        GameObject _game = new GameObject();
        _game.transform.parent = meshParent.transform;
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
        _game.GetComponent<MeshRenderer>().material = material;
        _game.AddComponent<MeshCollider>();
        GameObject _text = Instantiate(textPrefab, textParent);
        float _x = NodeIdToDataDic[_vec2].GetNodeVector.x;
        float _y = NodeIdToDataDic[_vec2].GetNodeVector.z;
        _text.GetComponent<RectTransform>().localPosition = new Vector3(_x, _y, 0);
        _text.GetComponent<TMP_Text>().text = NodeIdToDataDic[_vec2].GetNodeVector.ToString();
    }
}