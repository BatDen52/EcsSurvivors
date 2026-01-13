using Leopotam.EcsLite;
using UnityEngine;

public class ObjectLifecycleSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _destroyFilter;
    private EcsFilter _returnToPoolFilter;
    private EcsPool<DestroyRequest> _destroyRequestsPool;
    private EcsPool<DestroyEntityRequest> _destroyEntityRequestsPool;
    private EcsPool<ReturnToPoolRequest> _returnToPoolRequestsPool;
    private PoolReturnService _poolReturnService;

    public ObjectLifecycleSystem(PoolService poolService)
    {
        _poolReturnService = new PoolReturnService(poolService, null);
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _poolReturnService = new PoolReturnService(PoolService.Instance, _world);

        _destroyFilter = _world.Filter<DestroyRequest>().End();
        _returnToPoolFilter = _world.Filter<ReturnToPoolRequest>().End();

        var destroyEntityFilter = _world.Filter<DestroyEntityRequest>().End();

        _destroyRequestsPool = _world.GetPool<DestroyRequest>();
        _destroyEntityRequestsPool = _world.GetPool<DestroyEntityRequest>();
        _returnToPoolRequestsPool = _world.GetPool<ReturnToPoolRequest>();
    }

    public void Run(IEcsSystems systems)
    {
        // Обработка запросов на уничтожение конкретных сущностей
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

        // Обработка запросов на уничтожение текущей сущности
        foreach (var entity in _destroyFilter)
        {
            _poolReturnService.ReturnEntityToPool(entity);
            _world.DelEntity(entity);
        }

        // Обработка запросов на возврат в пул
        foreach (var entity in _returnToPoolFilter)
        {
            _poolReturnService.ReturnEntityToPool(entity);
            _world.DelEntity(entity);
        }
    }
}
