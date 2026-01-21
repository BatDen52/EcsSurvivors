using Leopotam.EcsLite;
using UnityEngine;

public class DamageSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EnemyConfig _config;
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsFilter _enemyFilter;
    private EcsPool<Health> _playerPool;
    private EcsPool<EnemyTag> _enemiesPool;
    private EcsPool<TransformRef> _positionsPool;
    private EcsPool<EnemyDamageCooldown> _damageCooldownsPool;

    public DamageSystem(EnemyConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<Health>().End();
        _enemyFilter = _world.Filter<EnemyTag>().Inc<Health>().Inc<EnemyDamageCooldown>().End();
        _playerPool = _world.GetPool<Health>();
        _enemiesPool = _world.GetPool<EnemyTag>();
        _positionsPool = _world.GetPool<TransformRef>();
        _damageCooldownsPool = _world.GetPool<EnemyDamageCooldown>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var enemy in _enemyFilter)
        {
            ref var enemyPos = ref _positionsPool.Get(enemy);

            foreach (var player in _playerFilter)
            {
                ref var playerPos = ref _positionsPool.Get(player);

                if (enemyPos.Value.position.IsEnoughClose(playerPos.Value.position, _config.DamageDistance))
                {
                    ref var damageCooldown = ref _damageCooldownsPool.Get(enemy);
                    damageCooldown.Current -= Time.deltaTime;

                    if (damageCooldown.Current <= 0)
                    {
                        damageCooldown.Current = damageCooldown.Max;

                        ref var playerHealth = ref _playerPool.Get(player);
                        playerHealth.Current -= _config.Damage;
                        if (playerHealth.Current < 0)
                            playerHealth.Current = 0;

                        var eventEntity = _world.NewEntity();
                        _world.GetPool<UpdatePlayerHealthEvent>().Add(eventEntity).CurrentHealth = playerHealth.Current;
                    }
                }
            }
        }
    }
}
