using Leopotam.EcsLite;
using UnityEngine;

public class SpawnPlayerSystem : IEcsRunSystem
{
    private readonly PlayerFactory _factory;

    public SpawnPlayerSystem(PlayerFactory factory)
    {
        _factory = factory;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<SpawnPlayerRequest>().End();

        foreach (var entity in filter)
        {
            _factory.Create(world);
            world.DelEntity(entity);
        }
    }
}