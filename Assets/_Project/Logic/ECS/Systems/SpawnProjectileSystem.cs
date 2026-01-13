using Leopotam.EcsLite;
using UnityEngine;

public class SpawnProjectileSystem : IEcsRunSystem
{
    private readonly ProjectileFactory _factory;

    public SpawnProjectileSystem(ProjectileFactory factory)
    {
        _factory = factory;
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<SpawnProjectileRequest>().End();
        var requests = world.GetPool<SpawnProjectileRequest>();

        foreach (var entity in filter)
        {
            ref var request = ref requests.Get(entity);
            _factory.Create(world, request.Position, request.Direction);
            world.DelEntity(entity);
        }
    }
}