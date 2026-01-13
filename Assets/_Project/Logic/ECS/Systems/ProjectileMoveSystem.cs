using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileMoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<RigidbodyRef> _rigidbodiesPool;
    private EcsPool<TransformRef> _transformsPool;
    private EcsPool<Direction> _directionPool;
    private EcsPool<ProjectileLifetime> _lifetimesPool;
    private EcsPool<DestroyRequest> _destroyRequestsPool;

    public ProjectileMoveSystem(GameConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<ProjectileTag>().Inc<RigidbodyRef>().Inc<TransformRef>().End();
        _rigidbodiesPool = _world.GetPool<RigidbodyRef>();
        _transformsPool = _world.GetPool<TransformRef>();
        _directionPool = _world.GetPool<Direction>();
        _lifetimesPool = _world.GetPool<ProjectileLifetime>();
        _destroyRequestsPool = _world.GetPool<DestroyRequest>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var projectile in _filter)
        {
            ref var rb = ref _rigidbodiesPool.Get(projectile);
            ref var transform = ref _transformsPool.Get(projectile);

            rb.Value.MovePosition(transform.Value.position + _directionPool.Get(projectile).Value * _config.ProjectileSpeed * Time.deltaTime);

            if (_lifetimesPool.Has(projectile) &&  _destroyRequestsPool.Has(projectile) == false)
            {
                ref var lifetime = ref _lifetimesPool.Get(projectile);
                lifetime.Value -= Time.deltaTime;
               
                if (lifetime.Value <= 0)
                {
                    _world.GetPool<DeadTag>().Add(projectile);
                }
            }
        }
    }
}
