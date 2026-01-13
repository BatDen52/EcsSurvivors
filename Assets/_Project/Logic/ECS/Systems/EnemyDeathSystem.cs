using Leopotam.EcsLite;
using UnityEngine;

public class EnemyDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    private EcsWorld _world;
    private EcsFilter _deadEnemiesFilter;
    private EcsPool<DeadTag> _deadPool;
    private EcsPool<EnemyTag> _enemiesPool;
    private EcsPool<TransformRef> _transformsPool;
    private EcsPool<UIHealthBar> _uiBarsPool;

    public EnemyDeathSystem(GameConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _deadEnemiesFilter = _world.Filter<DeadTag>().Inc<EnemyTag>().End();
        _deadPool = _world.GetPool<DeadTag>();
        _enemiesPool = _world.GetPool<EnemyTag>();
        _transformsPool = _world.GetPool<TransformRef>();
        _uiBarsPool = _world.GetPool<UIHealthBar>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var enemy in _deadEnemiesFilter)
        {
            if (Random.value < _config.CoinDropChance)
            {
                var spawnRequest = _world.NewEntity();
                ref var request = ref _world.GetPool<SpawnCoinRequest>().Add(spawnRequest);
                request.Position = _transformsPool.Get(enemy).Value.position;
            }

            if (_uiBarsPool.Has(enemy))
            {
                ref var uiBar = ref _uiBarsPool.Get(enemy);
                if (uiBar.HealthBarCanvas != null)
                {
                    GameObject.Destroy(uiBar.HealthBarCanvas.gameObject);
                }
                _uiBarsPool.Del(enemy);
            }

            _world.GetPool<DestroyRequest>().Add(enemy);
        }
    }
}
