using Leopotam.EcsLite;
using UnityEngine;

public class PlayerFactory : BaseFactory<PlayerTag>
{
    private const string DefaultName = "Player";
    private readonly EntityLink _prefab;
    protected readonly PlayerConfig _config;

    public PlayerFactory(PlayerConfig config)
    {
        _config = config;
        _prefab = _config.PlayerPrefab;
    }

    public override int Create(EcsWorld world)
    {
        var playerGo = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        playerGo.name = DefaultName;

        var entity = world.NewEntity();
        SetupTransform<PlayerTag>(world, entity, playerGo.transform);

        ref var health = ref world.GetPool<Health>().Add(entity);
        health.Current = _config.MaxHealth;
        health.Max = _config.MaxHealth;

        world.GetPool<MoveSpeed>().Add(entity).Value = _config.MoveSpeed;
        world.GetPool<FireCooldown>().Add(entity).Max = _config.ShootCooldown;
        world.GetPool<InputVector>().Add(entity);

        playerGo.Entity = entity;

        return entity;
    }
}
