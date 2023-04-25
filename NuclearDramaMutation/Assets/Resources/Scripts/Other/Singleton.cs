using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Singletons
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject _singleton = new GameObject("(Singleton)" + typeof(T).ToString());
                    instance = _singleton.AddComponent<T>();
                    DontDestroyOnLoad(_singleton);
                }
            }

            return instance;
        }
    }
}