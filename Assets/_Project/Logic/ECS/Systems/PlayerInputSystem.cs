using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    public EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<InputVector> _inputsPool;

    public PlayerInputSystem(GameConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<PlayerTag>().Inc<InputVector>().End();
        _inputsPool = _world.GetPool<InputVector>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _filter)
        {
            ref var input = ref _inputsPool.Get(player);
            input.Value = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }
    }
}