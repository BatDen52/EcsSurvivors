using Leopotam.EcsLite;
using UnityEngine;

public class PoolReturnService
{
    private readonly ObjectPool<EntityLink> _coinPool;
    private readonly ObjectPool<EntityLink> _projectilePool;
    private readonly ObjectPool<EntityLink> _enemyPool;
    private EcsWorld _world;

    public PoolReturnService(ObjectPool<EntityLink> enemyPool, ObjectPool<EntityLink> projectilePool,
        ObjectPool<EntityLink> coinPool, EcsWorld world)
    {
        _coinPool = coinPool;
        _projectilePool = projectilePool;
        _enemyPool = enemyPool;
        _world = world;
    }

    public void ReturnEntityToPool(int entity)
    {
        if (_world == null || !_world.GetPool<RigidbodyRef>().Has(entity))
            return;

        var rigidbody = _world.GetPool<RigidbodyRef>().Get(entity).Value;
        var entityLink = rigidbody.GetComponent<EntityLink>();

        if (entityLink == null)
            return;

        if (_world.GetPool<ProjectileTag>().Has(entity) && _projectilePool != null)
        {
            ResetProjectile(entity, rigidbody);
            _projectilePool.ReturnToPool(entityLink);
        }
        else if (_world.GetPool<EnemyTag>().Has(entity) && _enemyPool != null)
        {
            ResetEnemy(entity, rigidbody);
            _enemyPool.ReturnToPool(entityLink);
        }
        else if (_world.GetPool<CoinTag>().Has(entity) && _coinPool != null)
        {
            ResetCoin(entity, rigidbody);
            _coinPool.ReturnToPool(entityLink);
        }
        else
        {
            Object.Destroy(rigidbody.gameObject);
        }
    }

    private void ResetProjectile(int entity, Rigidbody rigidbody)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
       
        if (_world.GetPool<Direction>().Has(entity))
        {
            ref var direction = ref _world.GetPool<Direction>().Get(entity);
            direction.Value = Vector3.forward;
        }
    }

    private void ResetEnemy(int entity, Rigidbody rigidbody)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

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

    private void ResetCoin(int entity, Rigidbody rigidbody)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
