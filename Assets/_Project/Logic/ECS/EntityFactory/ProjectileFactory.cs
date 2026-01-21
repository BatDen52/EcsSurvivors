using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileFactory : BaseFactory<ProjectileTag>
{
    private const string DefaultName = "Projectile";

    private readonly ProjectileConfig _config;
    private readonly ObjectPool<EntityLink> _projectilePool;

    public ProjectileFactory(ProjectileConfig config, ObjectPool<EntityLink> projectilePool)
    {
        _config = config;
        _projectilePool = projectilePool;
    }

    public int Create(EcsWorld world, Vector3 position, Vector3 direction)
    {
        var entity = world.NewEntity();

        var projectileGo = _projectilePool.Get(position, Quaternion.LookRotation(direction));
        projectileGo.name = DefaultName;
        projectileGo.Entity = entity;
      
        SetupTransform(world, entity, projectileGo.transform);

        world.GetPool<Damage>().Add(entity).Amount = _config.Damage;
        world.GetPool<Direction>().Add(entity).Value = direction;
        world.GetPool<ProjectileLifetime>().Add(entity).Value = _config.Lifetime;

        var rigidbody = projectileGo.GetComponent<Rigidbody>();
        rigidbody.linearVelocity = direction * _config.Speed;

        projectileGo.GetComponent<CollisionTrigger>()?.Initialize(world);

        return entity;
    }

    public override int Create(EcsWorld world)
    {
        return Create(world, Vector3.zero, Vector3.forward);
    }
}
