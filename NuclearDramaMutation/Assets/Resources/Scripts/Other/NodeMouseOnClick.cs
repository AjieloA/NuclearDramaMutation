using UnityEngine;

public class NodeMouseOnClick : MonoBehaviour
{
    private UICroeEntity _croeEntity;
    private SceneCoreEntity _sceneEntity;
    private void OnMouseDown()
    {
        _sceneEntity=EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCoreEntity;
        _croeEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_UICROEENTITY] as UICroeEntity;

        if (UICore.Singletons.GetTransState("PiMenu"))
            return;
        Vector2 _uiPos;
        _sceneEntity.onClickVect = _sceneEntity.NodeIdToDataDic[transform.name].GetNodePoints[0];
        _sceneEntity.onClickNode = transform.name;
        _uiPos = WorldPointToUILocalPoint(_sceneEntity.onClickVect);
        UICore.Singletons.Query<RectTransform>("PiMenu").anchoredPosition = _uiPos;
        UICore.Singletons.Query<UIPiMenu>("PiMenu").Refresh(1);
        UICore.Singletons.UnShowHide("PiMenu",(trans)=> { });
    }

    private Vector3 WorldPointToUILocalPoint(Vector3 position)
    {
        //世界坐标转换为屏幕坐标
        Vector2 _screenPoint = Camera.main.WorldToScreenPoint(position);
        Vector2 _screenSize = new Vector2(Screen.width, Screen.height);
        //将屏幕坐标变换为以屏幕中心为原点
        _screenPoint -= _screenSize / 2;
        //缩放得到UGUI坐标
        Vector2 _anchorPos = _screenPoint / _screenSize * _croeEntity.mainCanvas.transform.GetComponent<RectTransform>().sizeDelta;
        return _anchorPos;

    }
}
