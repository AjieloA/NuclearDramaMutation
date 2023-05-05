using UnityEngine;

public class Application : Singleton<Application>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        Init();
    }
    public void Init()
    {
        CreatComponent<EntityManager>(true, "EntityManager", (game) => { });
        CreatUiManager((game) => { });


    }
    public void CreatComponent<T>(bool _isPersistence, string _name, System.Action<GameObject> _action, params object[] _params) where T : Component
    {
        GameObject _game = new GameObject(_name);
        if (_isPersistence)
            _game.transform.parent = transform;
        _game.AddComponent<T>();
        _action?.Invoke(_game);
    }
    public void CreatMapManger()
    {
        CreatComponent<MapGridManager>(false, "MapGridManager", (game) =>{ });
    }
    public void CreatUiManager(System.Action<GameObject> _action, params object[] _params)
    {
        GameObject _uiManger = GameObject.Find("UI");
        _uiManger.AddComponent<UIManager>();
        _action?.Invoke(_uiManger);
    }
    public void CreatUiCroe()
    {
        CreatComponent<UICore>(true, "UICore", (game) =>
        {
            game.GetComponent<UICore>().GetUiCroeEntity();
        });
    }
    public void CreatRoleManager()
    {
        CreatComponent<RoleManager>(true, "RoleManager", (game) =>
        {

        });
    }
}
