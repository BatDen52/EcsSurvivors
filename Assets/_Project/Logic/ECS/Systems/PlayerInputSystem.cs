using Leopotam.EcsLite;

public class PlayerInputSystem : IEcsInitSystem, IEcsRunSystem
{
    private InputService _inputService; 
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<InputVector> _inputsPool;

    public PlayerInputSystem(InputService inputService)
    {
        _inputService = inputService;
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
            input.Value = _inputService.GetDirection();
        }
    }
}
