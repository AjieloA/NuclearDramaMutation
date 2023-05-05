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
    public UICroeEntity GetUiCroeEntity()
    {
        return EntityManager.Singletons.entityManagers[Entity.GLOBAL_UICROEENTITY] as UICroeEntity;
    }
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
        Dictionary<string, Transform> dic = new Dictionary<string, Transform>();

        //Debug.LogError($"{_name}>>>Query>>>");
        //加载资源
        ResourceRequest _prefab = Resources.LoadAsync(TypeName.ResourcesTypeName.ResourcesUIPrefabes + _name);
        while (!_prefab.isDone)
        {
            yield return null;
        }
        //实例化资源
        GameObject _game = Instantiate(_prefab.asset as GameObject, GameObject.Find("UI").transform.GetChild((int)_uIShowLayer));
        _game.name = _name;

        Transform[] _allGame = _game.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _allGame.Length; i++)
        {
            dic.Add(_allGame[i].name, _allGame[i]);
        }
        if (GetUiCroeEntity().QueryUI.ContainsKey(_name))
            CloseShowUI(_name);
        GetUiCroeEntity().QueryUI.Add(_name, new Dictionary<string, Transform>(dic));
        dic.Clear();
        //获取对应脚本
        Type _type = Type.GetType($"UI{_name}");
        //添加脚本
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_name))
            foreach (var _item in GetUiCroeEntity().QueryUI[_name])
            {
                Destroy(_item.Value.gameObject);
                GetUiCroeEntity().QueryUI.Remove(_name);
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_name.name))
            foreach (var _item in GetUiCroeEntity().QueryUI[_name.name])
            {
                Destroy(_item.Value.gameObject);
                GetUiCroeEntity().QueryUI.Remove(_name.name);
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform.name))
            if (GetUiCroeEntity().QueryUI[_transform.name].ContainsKey(_name))
                _UI = GetUiCroeEntity().QueryUI[_transform.name][_name];
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform.name))
            if (GetUiCroeEntity().QueryUI[_transform.name].ContainsKey($"{_name}"))
                _UI = GetUiCroeEntity().QueryUI[_transform.name][$"{_name}"];
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform.name))
            if (GetUiCroeEntity().QueryUI[_transform.name].ContainsKey(_name))
                _UIComponent = GetUiCroeEntity().QueryUI[_transform.name][_name].GetComponent<T>();
            else
                Debug.LogError($"{_name}>>>Query>>>Null");
        else
            Debug.LogError($"{_name}>>>Query>>>Null");
        return _UIComponent;
    }
    /// <summary>
    /// 获取已经打开物体的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_transform"></param>
    /// <returns></returns>
    public T Query<T>(string _transform)
    {
        T _UIComponent = default(T);
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform))
            if (GetUiCroeEntity().QueryUI[_transform].ContainsKey(_transform))
                _UIComponent = GetUiCroeEntity().QueryUI[_transform][_transform].GetComponent<T>();
            else
                Debug.LogError($"{_transform}>>>Query>>>Null");
        else
            Debug.LogError($"{_transform}>>>Query>>>Null");
        return _UIComponent;
    }
    public T QueryComponent<T>(string _transform, string _name)
    {
        T _UIComponent = default(T);
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform))
            if (GetUiCroeEntity().QueryUI[_transform].ContainsKey(_name))
                _UIComponent = GetUiCroeEntity().QueryUI[_transform][_name].GetComponent<T>();
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
        if (GetUiCroeEntity().QueryUI.ContainsKey(_transform.name))
            if (GetUiCroeEntity().QueryUI[_transform.name].ContainsKey($"{_name}"))
                _UIComponent = GetUiCroeEntity().QueryUI[_transform.name][$"{_name}"].GetComponent<T>();
            else
                Debug.LogError($"{_name}>>>QueryComponent>>>Null");
        else
            Debug.LogError($"{_name}>>>QueryComponent>>>Null");
        return _UIComponent;
    }
    public void ShowHide(string _name,System.Action<Transform> _action)
    {
        GameObject _game = Query<Transform>(_name).gameObject;
        if (_game != null)
            _game.SetActive(false);
        else
            Debug.LogError($"{_name}>>>ShowHide>>>Null");
        _action?.Invoke(_game.transform);
    }
    public void UnShowHide(string _name,System.Action<Transform> _action)
    {
        GameObject _game = Query<Transform>(_name).gameObject;
        if (_game != null)
            _game.SetActive(true);
        else
            Debug.LogError($"{_name}>>>ShowHide>>>Null");
        _action?.Invoke(_game.transform);
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
    public bool GetTransState(string _name)
    {
        bool _isState=false;
        if (GetUiCroeEntity().QueryUI.ContainsKey(_name))
            if (GetUiCroeEntity().QueryUI[_name].ContainsKey($"{_name}"))
                 _isState=GetUiCroeEntity().QueryUI[_name][$"{_name}"].GetComponent<Transform>().gameObject.activeInHierarchy;
            else
                Debug.LogError($"{_name}>>>QueryComponent>>>Null");
        else
            Debug.LogError($"{_name}>>>QueryComponent>>>Null");
        return _isState;
    }

}