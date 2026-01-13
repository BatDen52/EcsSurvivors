using Leopotam.EcsLite;
using UnityEngine;

public class CoinFactory : BaseFactory<CoinTag>
{
    private readonly Transform _prefab;

    public CoinFactory(GameConfig config, Transform prefab)
        : base(config)
    {
        _prefab = prefab;
    }

    public int Create(EcsWorld world, Vector3 position)
    {
        Transform coinTransform;
        if (PoolService.Instance?.CoinPool != null)
        {
            var coinGo = PoolService.Instance.CoinPool.Get(position, Quaternion.identity);
            coinGo.name = "Coin";
            coinTransform = coinGo.transform;
        }
        else
        {
            var coinGo = Object.Instantiate(_prefab, position, Quaternion.identity);
            coinGo.name = "Coin";
            coinTransform = coinGo.transform;
        }

        var entity = world.NewEntity();
        SetupTransform<CoinTag>(world, entity, coinTransform);

        var entityLink = coinTransform.GetComponent<EntityLink>() ?? coinTransform.gameObject.AddComponent<EntityLink>();
        entityLink.Entity = entity;

        return entity;
    }

    public override int Create(EcsWorld world)
    {
        return Create(world, Vector3.zero);
    }
}
