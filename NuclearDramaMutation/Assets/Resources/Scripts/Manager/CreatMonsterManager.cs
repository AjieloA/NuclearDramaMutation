using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatMonsterManager : RoleCore
{
    SceneCoreEntity scenerEntity;
    private string monsterName;
    private int monsterCount;
    private float executeTime;

    public void Init(string _monsterName,int _monsterCount,float _executeTime)
    {
        this.monsterName = _monsterName;
        this.monsterCount = _monsterCount;
        this.executeTime = _executeTime;
        CreatMetalonGreen(monsterName, monsterCount, executeTime);
    }
    private void Awake()
    {
        scenerEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCoreEntity;
    }
    public void CreatMetalonGreen(string _name, int _count, float _time)
    {
        AsyncLoadMonster(_name, (_object) =>
        {
            GetRoleEntity().metalonGreen = _object;
            StartCoroutine(IECreatMonster(_count, _time));
        });

    }
    public IEnumerator IECreatMonster(int _count, float _time)
    {
        int _monsterCount = 0;
        while (_monsterCount < _count)
        {
            Creat();
            yield return new WaitForSeconds(_time);
        }
        void Creat()
        {
            GameObject _game = Instantiate(GetRoleEntity().metalonGreen);
            _game.transform.position = scenerEntity.NodeIdToDataDic[scenerEntity.startPoint].GetNodeVector;
            int _oId = GetRoleEntity().monsterOId++;
            _game.name = $"{_oId}";
            _game.AddComponent<MonsterManager>();
            MonsterEntity _monster = new MonsterEntity(_oId, _game, 200, 1f, scenerEntity.movePath, _game.GetComponent<MonsterManager>());
            GetRoleEntity().monsterEntityDic.Add(_oId, _monster);
            _game.SetActive(true);
            _monsterCount++;
        }
        yield return null;
    }
}
