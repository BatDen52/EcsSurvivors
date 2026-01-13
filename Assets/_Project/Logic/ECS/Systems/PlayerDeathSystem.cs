using Leopotam.EcsLite;
using UnityEngine;

public class PlayerDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _deadPlayersFilter;
    private EcsFilter _gameStateFilter;
    private EcsPool<DeadTag> _deadPool;
    private EcsPool<PlayerTag> _playersPool;
    private EcsPool<GameState> _gameStatesPool;
    private EcsPool<InputVector> _inputPool;
    private EcsPool<FireCooldown> _fireCooldownPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _deadPlayersFilter = _world.Filter<DeadTag>().Inc<PlayerTag>().End();
        _gameStateFilter = _world.Filter<GameState>().End();
        _deadPool = _world.GetPool<DeadTag>();
        _playersPool = _world.GetPool<PlayerTag>();
        _gameStatesPool = _world.GetPool<GameState>();
        _inputPool = _world.GetPool<InputVector>();
        _fireCooldownPool = _world.GetPool<FireCooldown>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _deadPlayersFilter)
        {
            _inputPool.Del(player);
            _fireCooldownPool.Del(player);

            foreach (var state in _gameStateFilter)
            {
                ref var gameState = ref _gameStatesPool.Get(state);
                gameState.IsGameOver = true;
            }

            _world.GetPool<DestroyRequest>().Add(player);
        }
    }
}
