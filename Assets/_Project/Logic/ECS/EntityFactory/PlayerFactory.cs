using Leopotam.EcsLite;
using UnityEngine;

public class PlayerFactory : BaseFactory<PlayerTag>
{
    private readonly Transform _prefab;
    private readonly GameObject _healthBarPrefab;

    public PlayerFactory(GameConfig config, Transform prefab, GameObject healthBarPrefab)
        : base(config)
    {
        _prefab = prefab;
        _healthBarPrefab = healthBarPrefab;
    }

    public override int Create(EcsWorld world)
    {
        var playerGo = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        playerGo.name = "Player";
        Transform playerTransform = playerGo.transform;

        var entity = world.NewEntity();
        SetupTransform<PlayerTag>(world, entity, playerTransform);

        ref var health = ref world.GetPool<Health>().Add(entity);
        health.Current = _config.PlayerMaxHealth;
        health.Max = _config.PlayerMaxHealth;

        world.GetPool<MoveSpeed>().Add(entity).Value = _config.PlayerMoveSpeed;
        world.GetPool<FireCooldown>().Add(entity).Max = _config.ShootCooldown;
        world.GetPool<InputVector>().Add(entity);

        var entityLink = playerTransform.GetComponent<EntityLink>() ?? playerTransform.gameObject.AddComponent<EntityLink>();
        entityLink.Entity = entity;

        return entity;
    }
}
