using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIShowLayer
{
    Default,
    Center,
    DiaLog,
    Top
}
public class UICore : Singleton<UICore>
{
    Dictionary<string, Transform> dic = new Dictionary<string, Transform>();

    /// <summary>
    /// 异步加载UI预制体到场景中
    /// </summary>
    /// <param name="_name"></param>
    public void OpenUIShow(string _name, UIShowLayer _uIShowLayer, System.Action<GameObject> _action)
    {
        StartCoroutine(AsyncLoadUIPrefabIE(_name, _uIShowLayer, _action));
    }
    public IEnumerator AsyncLoadUIPrefabIE(string _name, UIShowLayer _uIShowLayer, System.Action<GameObject> _action)
    {
        //Debug.LogError($"{_name}>>>Query>>>");
        dic.Clear();
        //加载资源
        ResourceRequest _prefab = Resources.LoadAsync(TypeName.ResourcesTypeName.ResourcesUIPrefabes + _name);
        //获取对应脚本
        while (!_prefab.isDone)
        {
            yield return null;
        }
        //实例化资源
        GameObject _game = Instantiate(_prefab.asset as GameObject, GameObject.Find("UI").transform.GetChild((int)_uIShowLayer));
        _game.name = _name;
        //添加脚本

        Transform[] _allGame = _game.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _allGame.Length; i++)
        {
            dic.Add(_allGame[i].name, _allGame[i]);
        }
        UICroeEntity.Singletons.QueryUI.Add(_name, new Dictionary<string, Transform>(dic));
        dic.Clear();
        Type _type = Type.GetType($"UI{_name}");
        if (_type != null)
            _game.AddComponent(_type);
        _action?.Invoke(_game);
        yield return null;
    }
    /// <summary>
    /// 删除场景中打开的UI物体
    /// </summary>
    /// <param name="_name"></param>
    public void CloseShowUI(string _name)
    {
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_name))
            foreach (var _item in UICroeEntity.Singletons.QueryUI[_name])
            {
                Destroy(_item.Value.gameObject);
                UICroeEntity.Singletons.QueryUI.Remove(_name);
                break;
            }
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
    }
    /// <summary>
    /// 关闭打开的UI
    /// </summary>
    /// <param name="_name"></param>
    public void CloseShowUI(Transform _name)
    {
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_name.name))
            foreach (var _item in UICroeEntity.Singletons.QueryUI[_name.name])
            {
                Destroy(_item.Value.gameObject);
                UICroeEntity.Singletons.QueryUI.Remove(_name.name);
                break;
            }
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
    }
    /// <summary>
    /// 如果ui界面被打开则可以获取到对应UI下的子物体
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public Transform Query(Transform _transform, string _name)
    {
        Transform _UI = null;
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_transform.name))
            if (UICroeEntity.Singletons.QueryUI[_transform.name].ContainsKey(_name))
                _UI = UICroeEntity.Singletons.QueryUI[_transform.name][_name];
            else
                Debug.LogError($"{_name}>>>Query>>>Null");
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
        return _UI;
    }
    /// <summary>
    /// 如果ui界面被打开则可以获取到对应UI下的子物体
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public Transform Query(Transform _transform, System.Enum _name)
    {
        Transform _UI = null;
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_transform.name))
            if (UICroeEntity.Singletons.QueryUI[_transform.name].ContainsKey($"{_name}"))
                _UI = UICroeEntity.Singletons.QueryUI[_transform.name][$"{_name}"];
            else
                Debug.LogError($"{_name}>>>Query>>>Null");
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
        return _UI;
    }
    /// <summary>
    /// 获取子物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_transform"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T QueryComponent<T>(Transform _transform, string _name)
    {
        T _UIComponent = default(T);
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_transform.name))
            if (UICroeEntity.Singletons.QueryUI[_transform.name].ContainsKey(_name))
                _UIComponent = UICroeEntity.Singletons.QueryUI[_transform.name][_name].GetComponent<T>();
            else
                Debug.LogError($"{_name}>>>Query>>>Null");
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
        return _UIComponent;
    }
    /// <summary>
    /// 获取子物体组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_transform"></param>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T QueryComponent<T>(Transform _transform, System.Enum _name)
    {
        T _UIComponent = default(T);
        if (UICroeEntity.Singletons.QueryUI.ContainsKey(_transform.name))
            if (UICroeEntity.Singletons.QueryUI[_transform.name].ContainsKey($"{_name}"))
                _UIComponent = UICroeEntity.Singletons.QueryUI[_transform.name][$"{_name}"].GetComponent<T>();
            else
                Debug.LogError($"{_name}>>>Query>>>Null");
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
        return _UIComponent;
    }
    /// <summary>
    /// 全局提示框
    /// </summary>
    /// <param name="_tips"></param>
    public void SetShowTips(bool _isResident, string _tips)
    {
        if (_isResident)
            OpenUIShow("ShowTips", UIShowLayer.Top, (game) =>
            {
                if (game != null)
                    game.GetComponent<UIShowTips>().Refresh(_tips);
            });
        else
            OpenUIShow("Tips", UIShowLayer.Top, (game) =>
            {
                if (game != null)
                    game.GetComponent<UITips>().Refresh(_tips);
            });
    }

}