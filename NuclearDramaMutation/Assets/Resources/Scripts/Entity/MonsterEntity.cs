using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntity : BaseEntity
{
    public int oId;
    public GameObject game;
    public float hp;
    public float Speed;
    public Vector3[] path;
    public MonsterManager monsterManager;

    public MonsterEntity(int _oId,GameObject _game, float _hp, float _speed, Vector3[] _path, MonsterManager _monsterManager)
    {
        this.oId = _oId;
        this.game = _game;
        this.hp = _hp;
        this.Speed = _speed;
        this.path = _path;
        this.monsterManager = _monsterManager;
    }
    public void SetHp(float _hp)
    {
        this.hp = _hp;
        monsterManager.KillDestory();
    }
}
