using Leopotam.EcsLite;

public class ObjectLifecycleSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _destroyFilter;
    private EcsFilter _returnToPoolFilter;
    private EcsPool<DestroyEntityRequest> _destroyEntityRequestsPool;
    private PoolReturnService _poolReturnService;

    public ObjectLifecycleSystem(ObjectPool<EntityLink> enemyPool, ObjectPool<EntityLink> projectilePool, 
        ObjectPool<EntityLink> coinPool, EcsWorld world)
    {
        _poolReturnService = new PoolReturnService(enemyPool, projectilePool, coinPool, world);
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _destroyFilter = _world.Filter<DestroyRequest>().End();
        _returnToPoolFilter = _world.Filter<ReturnToPoolRequest>().End();

        var destroyEntityFilter = _world.Filter<DestroyEntityRequest>().End();

        _destroyEntityRequestsPool = _world.GetPool<DestroyEntityRequest>();
    }

    public void Run(IEcsSystems systems)
    {
        var destroyEntityFilter = _world.Filter<DestroyEntityRequest>().End();
        foreach (var entity in destroyEntityFilter)
        {
            ref var request = ref _destroyEntityRequestsPool.Get(entity);

            if (_world.GetPool<TransformRef>().Has(request.Entity))
            {
                _poolReturnService.ReturnEntityToPool(request.Entity);
                _world.DelEntity(request.Entity);
            }

            _world.DelEntity(entity);
        }

        foreach (var entity in _destroyFilter)
        {
            _poolReturnService.ReturnEntityToPool(entity);
            _world.DelEntity(entity);
        }

        foreach (var entity in _returnToPoolFilter)
        {
            _poolReturnService.ReturnEntityToPool(entity);
            _world.DelEntity(entity);
        }
    }
}
