using System.Collections.Generic;
using UnityEngine;

public class UICroeEntity : Singleton<UICroeEntity>
{
    public Dictionary<string, Dictionary<string, Transform>> QueryUI = new Dictionary<string, Dictionary<string, Transform>>();
}
