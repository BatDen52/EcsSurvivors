using Leopotam.EcsLite;
using UnityEngine;

public class PlayerFactory : BaseFactory<PlayerTag>
{
    private readonly Transform _prefab;
    protected readonly PlayerConfig _config;

    public PlayerFactory(PlayerConfig config, Transform prefab)
    {
        _prefab = prefab;
        _config = config;
    }

    public override int Create(EcsWorld world)
    {
        var playerGo = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        playerGo.name = "Player";
        Transform playerTransform = playerGo.transform;

        var entity = world.NewEntity();
        SetupTransform<PlayerTag>(world, entity, playerTransform);

        ref var health = ref world.GetPool<Health>().Add(entity);
        health.Current = _config.MaxHealth;
        health.Max = _config.MaxHealth;

        world.GetPool<MoveSpeed>().Add(entity).Value = _config.MoveSpeed;
        world.GetPool<FireCooldown>().Add(entity).Max = _config.ShootCooldown;
        world.GetPool<InputVector>().Add(entity);

        var entityLink = playerTransform.GetComponent<EntityLink>() ?? playerTransform.gameObject.AddComponent<EntityLink>();
        entityLink.Entity = entity;

        return entity;
    }
}
