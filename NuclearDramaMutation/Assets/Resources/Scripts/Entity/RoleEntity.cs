using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEntity : BaseEntity
{
    public int monsterOId = 20000;
    /// <summary>
    /// ���Ｏ��
    /// </summary>
    public Dictionary<int, MonsterEntity> monsterEntityDic = new Dictionary<int, MonsterEntity>();
    public GameObject metalonGreen;

}
