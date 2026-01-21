using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly Camera _camera;
    private readonly GameplayConfig _config;
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<Health> _healthsPool;
    private EcsPool<UIHealthBar> _uiBarsPool;
    private EcsPool<TransformRef> _transformsPool;

    public UIEnemyHealthSystem(Camera camera, GameplayConfig config)
    {
        _camera = camera;
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<EnemyTag>().Inc<Health>().Inc<UIHealthBar>().End();
        _healthsPool = _world.GetPool<Health>();
        _uiBarsPool = _world.GetPool<UIHealthBar>();
        _transformsPool = _world.GetPool<TransformRef>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var enemy in _filter)
        {
            ref var uiBar = ref _uiBarsPool.Get(enemy);
            ref var health = ref _healthsPool.Get(enemy);

            if (uiBar.HealthBarCanvas == null)
                continue;

            var enemyHealthBar = uiBar.HealthBarCanvas.GetComponent<EnemyHealthBar>();
            if (enemyHealthBar != null)
            {
                enemyHealthBar.UpdateHealth(health.Current, health.Max);

                if (_transformsPool.Has(enemy))
                {
                    ref var transform = ref _transformsPool.Get(enemy);
                    enemyHealthBar.SetPosition(transform.Value.position, _camera, _config);
                }
            }
        }
    }
}
