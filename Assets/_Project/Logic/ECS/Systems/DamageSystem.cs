using Leopotam.EcsLite;
using UnityEngine;

public class DamageSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EnemyConfig _config;
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsFilter _enemyFilter;
    private EcsPool<Health> _playerPool;
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
        _positionsPool = _world.GetPool<TransformRef>();
        _damageCooldownsPool = _world.GetPool<EnemyDamageCooldown>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            var playerPos = _positionsPool.Get(player).Value.position;
            ref var playerHealth = ref _playerPool.Get(player);

            foreach (var enemy in _enemyFilter)
            {
                var enemyPos = _positionsPool.Get(enemy).Value.position;
                ref var damageCooldown = ref _damageCooldownsPool.Get(enemy);

                if (enemyPos.IsEnoughClose(playerPos, _config.DamageDistance) == false
                    || TryConsumeCooldown(ref damageCooldown) == false)
                    continue;

                ApplyDamageToPlayer(ref playerHealth);
                ResetCooldown(ref damageCooldown);
                CreateHealthUpdateEvent(playerHealth.Current);
            }
        }
    }

    private bool TryConsumeCooldown(ref EnemyDamageCooldown damageCooldown)
    {
        damageCooldown.Current -= Time.deltaTime;
        return damageCooldown.Current <= 0;
    }

    private void ApplyDamageToPlayer(ref Health playerHealth)
    {
        playerHealth.Current = Mathf.Max(0, playerHealth.Current - _config.Damage);
    }

    private void ResetCooldown(ref EnemyDamageCooldown cooldown)
    {
        cooldown.Current = cooldown.Max;
    }

    private void CreateHealthUpdateEvent(int currentHealth)
    {
        var eventEntity = _world.NewEntity();
        _world.GetPool<UpdatePlayerHealthEvent>().Add(eventEntity).CurrentHealth = currentHealth;
    }
}
