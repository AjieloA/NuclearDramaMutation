using System.Collections.Generic;
using UnityEngine;

public class UICroeEntity : BaseEntity
{
    public Dictionary<string, Dictionary<string, Transform>> QueryUI = new Dictionary<string, Dictionary<string, Transform>>();
    public Canvas mainCanvas;
}
