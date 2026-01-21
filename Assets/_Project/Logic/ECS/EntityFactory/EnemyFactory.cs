using Leopotam.EcsLite;
using UnityEngine;

public class EnemyFactory : BaseFactory<EnemyTag>
{
    private const string DefaultName = "Enemy";

    private readonly Camera _camera;
    private readonly ObjectPool<EntityLink> _enemyPool;
    private readonly EnemyConfig _config;

    public EnemyFactory(EnemyConfig config, Camera camera, ObjectPool<EntityLink> enemyPool)
    {
        _config = config;
        _camera = camera;
        _enemyPool = enemyPool;
    }

    public int Create(EcsWorld world, Vector3 position)
    {
        var entity = world.NewEntity();

        var enemyGo = _enemyPool.Get(position, Quaternion.identity);
        enemyGo.name = DefaultName;
        enemyGo.Entity = entity;

        SetupTransform(world, entity, enemyGo.transform);

        ref var health = ref world.GetPool<Health>().Add(entity);
        health.Current = _config.MaxHealth;
        health.Max = _config.MaxHealth;

        world.GetPool<MoveSpeed>().Add(entity).Value = _config.MoveSpeed;
        world.GetPool<EnemyDamageCooldown>().Add(entity).Max = _config.SpawnInterval;

        var healthBar = Object.Instantiate(_config.HealthBarPrefab, Vector3.zero, Quaternion.identity);
        var canvas = healthBar.GetComponent<Canvas>();
        canvas.worldCamera = _camera;

        world.GetPool<UIHealthBar>().Add(entity).HealthBarCanvas = canvas;

        return entity;
    }

    public override int Create(EcsWorld world)
    {
        return Create(world, Vector3.zero);
    }
}
