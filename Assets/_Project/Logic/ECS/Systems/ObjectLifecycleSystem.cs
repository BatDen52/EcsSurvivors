using Leopotam.EcsLite;

public class ObjectLifecycleSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _destroyFilter;
    private EcsFilter _returnToPoolFilter;
    private EcsPool<DestroyRequest> _destroyRequestsPool;
    private EcsPool<DestroyEntityRequest> _destroyEntityRequestsPool;
    private EcsPool<ReturnToPoolRequest> _returnToPoolRequestsPool;
    private PoolReturnService _poolReturnService;

    public ObjectLifecycleSystem(ObjectPool<EntityLink> enemyPool, ObjectPool<EntityLink> projectilePool, 
        ObjectPool<EntityLink> coinPool)
    {
        _poolReturnService = new PoolReturnService(enemyPool, projectilePool, coinPool);
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _poolReturnService.SetWorld(_world);

        _destroyFilter = _world.Filter<DestroyRequest>().End();
        _returnToPoolFilter = _world.Filter<ReturnToPoolRequest>().End();

        var destroyEntityFilter = _world.Filter<DestroyEntityRequest>().End();

        _destroyRequestsPool = _world.GetPool<DestroyRequest>();
        _destroyEntityRequestsPool = _world.GetPool<DestroyEntityRequest>();
        _returnToPoolRequestsPool = _world.GetPool<ReturnToPoolRequest>();
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
