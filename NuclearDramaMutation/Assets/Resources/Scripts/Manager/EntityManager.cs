using System.Collections.Generic;
using UnityEngine;

public enum Entity
{
    GLOBAL_UICROEENTITY,
    GLOBAL_SCENECROENTITY,
    GLOBAL_ROLEENETITY,
}
public class EntityManager : Singleton<EntityManager>
{
    public Dictionary<Entity, BaseEntity> entityManagers = new Dictionary<Entity, BaseEntity>();
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        entityManagers.Add(Entity.GLOBAL_SCENECROENTITY, new SceneCoreEntity());
        entityManagers.Add(Entity.GLOBAL_UICROEENTITY, new UICroeEntity());
        entityManagers.Add(Entity.GLOBAL_ROLEENETITY, new RoleEntity());
    }
}
