using Leopotam.EcsLite;

public class SpawnProjectileSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly ProjectileFactory _factory;
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<SpawnProjectileRequest> _requests;

    public SpawnProjectileSystem(ProjectileFactory factory)
    {
        _factory = factory;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<SpawnProjectileRequest>().End();
        _requests = _world.GetPool<SpawnProjectileRequest>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var request = ref _requests.Get(entity);
            _factory.Create(_world, request.Position, request.Direction);
            _world.DelEntity(entity);
        }
    }
}