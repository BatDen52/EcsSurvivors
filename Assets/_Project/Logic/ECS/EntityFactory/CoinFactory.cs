using Leopotam.EcsLite;
using UnityEngine;

public class CoinFactory : BaseFactory<CoinTag>
{
    private const string DefaultName = "Coin";

    private readonly EntityLink _prefab;
    private readonly CoinConfig _config;
    private readonly ObjectPool<EntityLink> _coinPool;

    public CoinFactory(CoinConfig config, ObjectPool<EntityLink> coinPool)
    {
        _config = config;
        _coinPool = coinPool;
        _prefab = _config.CoinPrefab;
    }

    public int Create(EcsWorld world, Vector3 position)
    {
        var entity = world.NewEntity();

        var coinGo = _coinPool.Get(position, Quaternion.identity);
        coinGo.name = DefaultName;
        coinGo.Entity = entity;

        SetupTransform(world, entity, coinGo.transform);

        return entity;
    }

    public override int Create(EcsWorld world)
    {
        return Create(world, Vector3.zero);
    }
}
