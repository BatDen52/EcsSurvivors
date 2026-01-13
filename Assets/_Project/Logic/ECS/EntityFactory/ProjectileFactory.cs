using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileFactory : BaseFactory<ProjectileTag>
{
    private readonly Transform _prefab;

    public ProjectileFactory(GameConfig config, Transform prefab)
        : base(config)
    {
        _prefab = prefab;
    }

    public int Create(EcsWorld world, Vector3 position, Vector3 direction)
    {
        Transform projectileTransform;
        if (PoolService.Instance?.ProjectilePool != null)
        {
            var projectileGo = PoolService.Instance.ProjectilePool.Get(position, Quaternion.LookRotation(direction));
            projectileGo.name = "Projectile";
            projectileTransform = projectileGo.transform;
        }
        else
        {
            var projectileGo = Object.Instantiate(_prefab, position, Quaternion.LookRotation(direction));
            projectileGo.name = "Projectile";
            projectileTransform = projectileGo.transform;
        }

        var entity = world.NewEntity();
        SetupTransform<ProjectileTag>(world, entity, projectileTransform);

        world.GetPool<Damage>().Add(entity).Amount = _config.ProjectileDamage;
        world.GetPool<Direction>().Add(entity).Value = direction;
        world.GetPool<ProjectileLifetime>().Add(entity).Value = _config.ProjectileLifetime;

        var rigidbody = projectileTransform.GetComponent<Rigidbody>();
        rigidbody.linearVelocity = direction * _config.ProjectileSpeed;

        var entityLink = projectileTransform.GetComponent<EntityLink>() ?? projectileTransform.gameObject.AddComponent<EntityLink>();
        entityLink.Entity = entity;

        projectileTransform.GetComponent<CollisionTrigger>()?.Initialize(world);

        return entity;
    }

    public override int Create(EcsWorld world)
    {
        return Create(world, Vector3.zero, Vector3.forward);
    }
}
