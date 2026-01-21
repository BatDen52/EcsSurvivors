using Leopotam.EcsLite;
using UnityEngine;

public class EnemyFactory : BaseFactory<EnemyTag>
{
    private readonly Transform _prefab;
    private readonly Camera _camera;
    private readonly GameObject _healthBarPrefab;
    private readonly EnemyConfig _config;

    public EnemyFactory(EnemyConfig config, Transform prefab, Camera camera, GameObject healthBarPrefab)
    {
        _config = config;
        _prefab = prefab;
        _camera = camera;
        _healthBarPrefab = healthBarPrefab;
    }

    public int Create(EcsWorld world, Vector3 position)
    {
        Transform enemyTransform;
        if (PoolService.Instance?.EnemyPool != null)
        {
            var enemyGo = PoolService.Instance.EnemyPool.Get(position, Quaternion.identity);
            enemyGo.name = "Enemy";
            enemyTransform = enemyGo.transform;
        }
        else
        {
            var enemyGo = Object.Instantiate(_prefab, position, Quaternion.identity);
            enemyGo.name = "Enemy";
            enemyTransform = enemyGo.transform;
        }

        var entity = world.NewEntity();
        SetupTransform<EnemyTag>(world, entity, enemyTransform);

        ref var health = ref world.GetPool<Health>().Add(entity);
        health.Current = _config.MaxHealth;
        health.Max = _config.MaxHealth;

        world.GetPool<MoveSpeed>().Add(entity).Value = _config.MoveSpeed;

        world.GetPool<EnemyDamageCooldown>().Add(entity).Max = 1f;

        var entityLink = enemyTransform.GetComponent<EntityLink>() ?? enemyTransform.gameObject.AddComponent<EntityLink>();
        entityLink.Entity = entity;

        var healthBar = Object.Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity);
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
