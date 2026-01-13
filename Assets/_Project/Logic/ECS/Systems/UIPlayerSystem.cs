using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

public class UIPlayerSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly TextMeshProUGUI _healthText;
    private readonly TextMeshProUGUI _coinsText;
    private readonly GameObject _gameOverPanel;
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsFilter _gameStateFilter;
    private EcsPool<Health> _healthsPool;
    private EcsPool<ResourceCoins> _resourcesPool;
    private EcsPool<GameState> _gameStatesPool;

    public UIPlayerSystem(TextMeshProUGUI healthText, TextMeshProUGUI coinsText, GameObject gameOverPanel)
    {
        _healthText = healthText;
        _coinsText = coinsText;
        _gameOverPanel = gameOverPanel;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<Health>().End();
        _gameStateFilter = _world.Filter<GameState>().End();
        _healthsPool = _world.GetPool<Health>();
        _resourcesPool = _world.GetPool<ResourceCoins>();
        _gameStatesPool = _world.GetPool<GameState>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            ref var health = ref _healthsPool.Get(player);
            var eventEntity = _world.NewEntity();
            _world.GetPool<UpdatePlayerHealthEvent>().Add(eventEntity).CurrentHealth = health.Current;
        }

        foreach (var state in _gameStateFilter)
        {
            ref var coins = ref _resourcesPool.Get(state);
            var eventEntity = _world.NewEntity();
            _world.GetPool<UpdatePlayerCoinsEvent>().Add(eventEntity).CoinCount = coins.Count;

            ref var gameState = ref _gameStatesPool.Get(state);
            _gameOverPanel.SetActive(gameState.IsGameOver);
        }
    }
}