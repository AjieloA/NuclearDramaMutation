using UnityEditor;
using UnityEngine;

public class CreatUIPrefabScript : MonoBehaviour
{
    [MenuItem("Assets/Creat Widget Script", false, 50)]
    private static void GetNames()
    {
        var _selectGame = Selection.activeGameObject;
        if (_selectGame == null || !PrefabUtility.IsPartOfAnyPrefab(_selectGame))
        {
            Debug.Log("Please select a prefab.");
            return;
        }
        Transform[] _childrensTrans = _selectGame.transform.GetComponentsInChildren<Transform>();
        string[] _childrens = new string[_childrensTrans.Length];
        int _index = 0;
        foreach (Transform t in _childrensTrans)
        {
            _childrens[_index] = $"        {t.name}";
            _index++;
        }
        var enumValues = string.Join(",\n", _childrens);
        string _path = $"Assets/Resources/Scripts/UI/UI{_selectGame.name}.cs";
        string _scriptCode =
            $"using UnityEngine;\nusing UnityEngine.UI;\n\n" +
            $"public class UI{_selectGame.name} : UICore\n" +
            $"{{\n" +
            $"    enum Enums\n" +
            $"    {{\n" +
            $"{enumValues}\n" +
            $"    }}\n" +
            $"}}";
        System.IO.File.WriteAllText(_path, _scriptCode);
        AssetDatabase.LoadAssetAtPath<MonoScript>(_path);
    }
}
