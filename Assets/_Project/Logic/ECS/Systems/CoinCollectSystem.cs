using Leopotam.EcsLite;
using UnityEngine;

public class CoinCollectSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    private EcsWorld _world;
    private EcsFilter _coinFilter;
    private EcsFilter _playerFilter;
    private EcsFilter _stateFilter;
    private EcsPool<TransformRef> _coinTransformsPool;
    private EcsPool<TransformRef> _playerTransformsPool;
    private EcsPool<ResourceCoins> _resourcesPool;

    public CoinCollectSystem(GameConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _coinFilter = _world.Filter<CoinTag>().Inc<TransformRef>().End();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformRef>().End();
        _stateFilter = _world.Filter<GameState>().Inc<ResourceCoins>().End();
        _coinTransformsPool = _world.GetPool<TransformRef>();
        _playerTransformsPool = _world.GetPool<TransformRef>();
        _resourcesPool = _world.GetPool<ResourceCoins>();
    }

    public void Run(IEcsSystems systems)
    {
        if (_playerFilter.GetEntitiesCount() == 0)
            return;

        Vector3 playerPosition = Vector3.zero;

        foreach (var player in _playerFilter)
        {
            playerPosition = _playerTransformsPool.Get(player).Value.position;
            break;
        }

        foreach (var coin in _coinFilter)
        {
            ref var coinTransform = ref _coinTransformsPool.Get(coin);

            if (coinTransform.Value.position.IsEnoughClose(playerPosition, _config.CoinCollectionDistance))
            {
                foreach (var state in _stateFilter)
                {
                    ref var coins = ref _resourcesPool.Get(state);
                    coins.Count++;
                }

                GameObject.Destroy(coinTransform.Value.gameObject);
                _world.DelEntity(coin);
            }
        }
    }
}
