using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<InputVector> _inputsPool;
    private EcsPool<MoveSpeed> _speedsPool;
    private EcsPool<RigidbodyRef> _rigidbodiesPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<PlayerTag>().Inc<InputVector>().Inc<MoveSpeed>().Inc<RigidbodyRef>().End();
        _inputsPool = _world.GetPool<InputVector>();
        _speedsPool = _world.GetPool<MoveSpeed>();
        _rigidbodiesPool = _world.GetPool<RigidbodyRef>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _filter)
        {
            ref var input = ref _inputsPool.Get(player);
            ref var speed = ref _speedsPool.Get(player);
            ref var rigidbody = ref _rigidbodiesPool.Get(player);

            Vector3 moveDirection = new Vector3(input.Value.x, 0, input.Value.y).normalized;
            rigidbody.Value.linearVelocity = moveDirection * speed.Value;
        }
    }
}