using Leopotam.EcsLite;

public class SpawnCoinSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly CoinFactory _factory;
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<SpawnCoinRequest> _requests;

    public SpawnCoinSystem(CoinFactory factory)
    {
        _factory = factory;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<SpawnCoinRequest>().End();
        _requests = _world.GetPool<SpawnCoinRequest>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var request = ref _requests.Get(entity);
            _factory.Create(_world, request.Position);
            _world.DelEntity(entity);
        }
    }
}