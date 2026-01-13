using Leopotam.EcsLite;

public class HealthCheckSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<Health> _healthPool;
    private EcsPool<DeadTag> _deadPool;
    private EcsPool<PlayerTag> _playersPool;
    private EcsPool<EnemyTag> _enemiesPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<Health>().End();
        _healthPool = _world.GetPool<Health>();
        _deadPool = _world.GetPool<DeadTag>();
        _playersPool = _world.GetPool<PlayerTag>();
        _enemiesPool = _world.GetPool<EnemyTag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var health = ref _healthPool.Get(entity);

            if (health.Current <= 0 && !_deadPool.Has(entity))
            {
                _deadPool.Add(entity);

                if (_playersPool.Has(entity))
                {
                    var eventEntity = _world.NewEntity();
                    _world.GetPool<UpdatePlayerHealthEvent>().Add(eventEntity).CurrentHealth = 0;
                }
            }
        }
    }
}
