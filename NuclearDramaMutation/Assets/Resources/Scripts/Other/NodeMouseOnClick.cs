using UnityEngine;

public class NodeMouseOnClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (UICore.Singletons.GetTransState("PiMenu"))
            return;
        Vector2 _uiPos;
        SceneCroeEntity.Singletons.onClickVect = SceneCroeEntity.Singletons.NodeIdToDataDic[transform.name].GetNodePoints[0];
        SceneCroeEntity.Singletons.onClickNode = transform.name;
        _uiPos = WorldPointToUILocalPoint(SceneCroeEntity.Singletons.onClickVect);
        UICore.Singletons.Query<RectTransform>("PiMenu").anchoredPosition = _uiPos;
        UICore.Singletons.Query<UIPiMenu>("PiMenu").Refresh(1);
        UICore.Singletons.UnShowHide("PiMenu",(trans)=> { });
    }

    private Vector3 WorldPointToUILocalPoint(Vector3 position)
    {
        //��������ת��Ϊ��Ļ����
        Vector2 _screenPoint = Camera.main.WorldToScreenPoint(position);
        Vector2 _screenSize = new Vector2(Screen.width, Screen.height);
        //����Ļ����任Ϊ����Ļ����Ϊԭ��
        _screenPoint -= _screenSize / 2;
        //���ŵõ�UGUI����
        Vector2 _anchorPos = _screenPoint / _screenSize * UICroeEntity.Singletons.mainCanvas.transform.GetComponent<RectTransform>().sizeDelta;
        return _anchorPos;

    }
}
