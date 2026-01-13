using Leopotam.EcsLite;
using UnityEngine;

public class SpawnCoinSystem : IEcsRunSystem
{
    private readonly CoinFactory _factory;

    public SpawnCoinSystem(CoinFactory factory)
    {
        _factory = factory;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<SpawnCoinRequest>().End();
        var requests = world.GetPool<SpawnCoinRequest>();

        foreach (var entity in filter)
        {
            ref var request = ref requests.Get(entity);
            _factory.Create(world, request.Position);
            world.DelEntity(entity);
        }
    }
}