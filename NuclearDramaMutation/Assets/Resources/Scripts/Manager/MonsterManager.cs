using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MonsterManager : RoleCore
{
    public MonsterEntity monsterEntity;
    private SceneCoreEntity scenerEntity;
    public Animator animator;
    private void Awake()
    {
        monsterEntity = GetRoleEntity().monsterEntityDic[int.Parse(transform.name)];
        scenerEntity = EntityManager.Singletons.entityManagers[Entity.GLOBAL_SCENECROENTITY] as SceneCoreEntity;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(IEMove());
    }
    public IEnumerator IEMove()
    {
        int _count = monsterEntity.path.Length - 1;
        while (_count >= 0)
        {
            animator.SetFloat("Speed", 3.1f);
            Vector3 _vector = monsterEntity.path[_count];
            transform.DOLookAt(_vector, 0.5f);
            while (transform.position != _vector)
            {
                transform.position = Vector3.MoveTowards(transform.position, _vector, monsterEntity.Speed * Time.deltaTime);
                yield return null;
            }
            _count--;
            yield return null;
        }
        DestoryThis();
        yield return null;
    }
    public void DestoryThis()
    {
        GetRoleEntity().monsterEntityDic.Remove(int.Parse(transform.name));
        Destroy(this.gameObject);
    }
    public void KillDestory()
    {
        EventCore.Instance.TiggerEventListener(TypeName.EventTypeName.CreatMonter);
        EventCore.Instance.ClearEvents();
        scenerEntity.SetKillCount(1);
        animator.SetBool("Die", true);
        transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.3f).OnComplete(() =>
        {
            GetRoleEntity().monsterEntityDic.Remove(int.Parse(transform.name));
            Destroy(this.gameObject);
        });

    }

}
