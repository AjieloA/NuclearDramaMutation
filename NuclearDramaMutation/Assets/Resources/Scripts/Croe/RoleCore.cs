using System.Collections;
using UnityEngine;

public class RoleCore : Singleton<UICore>
{
    public RoleEntity GetRoleEntity()
    {
        return EntityManager.Singletons.entityManagers[Entity.GLOBAL_ROLEENETITY] as RoleEntity;

    }
    /// <summary>
    /// 异步加载怪物数据
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_action"></param>
    /// <param name="_params"></param>
    /// <returns></returns>
    public void AsyncLoadMonster(string _name, System.Action<GameObject> _action, params object[] _params)
    {
        GameObject _game = null;
        StartCoroutine(AsyncLoadMonsterIE(_name, (_object) =>
        {
            _game = _object;
            _action?.Invoke(_game);
        }));
    }
    private IEnumerator AsyncLoadMonsterIE(string _name, System.Action<GameObject> _action, params object[] _params)
    {
        ResourceRequest _request = Resources.LoadAsync(TypeName.ResourcesTypeName.RMonster + _name);
        while (_request.isDone)
            yield return null;
        _action?.Invoke(_request.asset as GameObject);
        yield return null;
    }
    public void ExecuteTime(float _time, System.Action _action)
    {
        float _timer = 0;
        while (true)
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer >= _time)
            {
                _action?.Invoke();
                break;
            }
        }
    }
}
