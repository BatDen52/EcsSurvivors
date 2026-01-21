using Leopotam.EcsLite;
using UnityEngine;

public class PoolReturnService
{
    private readonly ObjectPool<EntityLink> _coinPool;
    private readonly ObjectPool<EntityLink> _projectilePool;
    private readonly ObjectPool<EntityLink> _enemyPool;
    private EcsWorld _world;

    public PoolReturnService(ObjectPool<EntityLink> enemyPool, ObjectPool<EntityLink> projectilePool,
        ObjectPool<EntityLink> coinPool)
    {
        _coinPool = coinPool;
        _projectilePool = projectilePool;
        _enemyPool = enemyPool;
    }

    public void SetWorld(EcsWorld world)
    {
        _world = world;
    }

    public void ReturnEntityToPool(int entity)
    {
        if (_world == null || !_world.GetPool<TransformRef>().Has(entity))
            return;

        var transform = _world.GetPool<TransformRef>().Get(entity).Value;
        var entityLink = transform.GetComponent<EntityLink>();

        if (entityLink == null)
            return;

        if (_world.GetPool<ProjectileTag>().Has(entity) && _projectilePool != null)
        {
            ResetProjectile(entity, transform);
            _projectilePool.ReturnToPool(entityLink);
        }
        else if (_world.GetPool<EnemyTag>().Has(entity) && _enemyPool != null)
        {
            ResetEnemy(entity, transform);
            _enemyPool.ReturnToPool(entityLink);
        }
        else if (_world.GetPool<CoinTag>().Has(entity) && _coinPool != null)
        {
            ResetCoin(entity, transform);
            _coinPool.ReturnToPool(entityLink);
        }
        else
        {
            Object.Destroy(transform.gameObject);
        }
    }

    private void ResetProjectile(int entity, Transform transform)
    {
        var rb = transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_world.GetPool<Direction>().Has(entity))
        {
            ref var direction = ref _world.GetPool<Direction>().Get(entity);
            direction.Value = Vector3.forward;
        }
    }

    private void ResetEnemy(int entity, Transform transform)
    {
        var rb = transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (_world.GetPool<Health>().Has(entity))
        {
            ref var health = ref _world.GetPool<Health>().Get(entity);
            health.Current = health.Max;

            if (_world.GetPool<UIHealthBar>().Has(entity))
            {
                ref var uiBar = ref _world.GetPool<UIHealthBar>().Get(entity);
                if (uiBar.HealthBarCanvas != null)
                {
                    var enemyHealthBar = uiBar.HealthBarCanvas.GetComponent<EnemyHealthBar>();
                    if (enemyHealthBar != null)
                    {
                        enemyHealthBar.UpdateHealth(health.Max, health.Max);
                        uiBar.HealthBarCanvas.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void ResetCoin(int entity, Transform transform)
    {
        var rb = transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
