using Leopotam.EcsLite;
using UnityEngine;

public class EnemyMoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EnemyConfig _config;
    private EcsWorld _world;
    private EcsFilter _enemyFilter;
    private EcsFilter _playerFilter;
    private EcsPool<TransformRef> _transformsPool;
    private EcsPool<MoveSpeed> _speedsPool;
    private EcsPool<RigidbodyRef> _rigidbodiesPool;

    public EnemyMoveSystem(EnemyConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _enemyFilter = _world.Filter<EnemyTag>().Inc<TransformRef>().Inc<MoveSpeed>().Inc<RigidbodyRef>().End();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformRef>().End();
        _transformsPool = _world.GetPool<TransformRef>();
        _speedsPool = _world.GetPool<MoveSpeed>();
        _rigidbodiesPool = _world.GetPool<RigidbodyRef>();
    }

    public void Run(IEcsSystems systems)
    {
        if (_playerFilter.GetEntitiesCount() == 0) 
            return;

        Vector3 playerPosition = Vector3.zero;

        foreach (var player in _playerFilter)
        {
            playerPosition = _transformsPool.Get(player).Value.position;
            break;
        }

        foreach (var enemy in _enemyFilter)
        {
            ref var enemyTransform = ref _transformsPool.Get(enemy);
            ref var speed = ref _speedsPool.Get(enemy);
            ref var rigidbody = ref _rigidbodiesPool.Get(enemy);
            Vector3 position = enemyTransform.Value.position;

            Vector3 direction = (playerPosition - position).normalized;

            rigidbody.Value.linearVelocity = 
                                    playerPosition.IsEnoughClose(position, _config.StopDistance) == false ?
                                    direction * speed.Value :
                                    Vector3.zero;
        }
    }
}
