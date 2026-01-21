using Leopotam.EcsLite;

public class SpawnPlayerSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly PlayerFactory _factory;
    private EcsWorld _world;
    private EcsFilter _filter;

    public SpawnPlayerSystem(PlayerFactory factory)
    {
        _factory = factory;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<SpawnPlayerRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            _factory.Create(_world);
            _world.DelEntity(entity);
        }
    }
}