using Leopotam.EcsLite;
using UnityEngine;

public class UIEventListeners : MonoBehaviour
{
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    [SerializeField] private PlayerCoinsUI _playerCoinsUI;
    [SerializeField] private Camera _mainCamera;

    private EcsWorld _world;
    private EcsFilter _playerHealthEventsFilter;
    private EcsFilter _playerCoinsEventsFilter;
    private EcsPool<UpdatePlayerHealthEvent> _playerHealthEventsPool;
    private EcsPool<UpdatePlayerCoinsEvent> _playerCoinsEventsPool;

    private void Update()
    {
        if (_world == null)
            return;

        ProcessPlayerHealthEvents();
        ProcessPlayerCoinsEvents();
    }

    public void InitializeEcsFilters(EcsWorld world)
    {
        _world = world;
        _playerHealthEventsFilter = _world.Filter<UpdatePlayerHealthEvent>().End();
        _playerCoinsEventsFilter = _world.Filter<UpdatePlayerCoinsEvent>().End();

        _playerHealthEventsPool = _world.GetPool<UpdatePlayerHealthEvent>();
        _playerCoinsEventsPool = _world.GetPool<UpdatePlayerCoinsEvent>();
    }

    private void ProcessPlayerHealthEvents()
    {
        foreach (var entity in _playerHealthEventsFilter)
        {
            ref var evt = ref _playerHealthEventsPool.Get(entity);
            _playerHealthUI.SetHealth(evt.CurrentHealth);
            _world.DelEntity(entity);
        }
    }

    private void ProcessPlayerCoinsEvents()
    {
        foreach (var entity in _playerCoinsEventsFilter)
        {
            ref var evt = ref _playerCoinsEventsPool.Get(entity);
            _playerCoinsUI.SetCoins(evt.CoinCount);
            _world.DelEntity(entity);
        }
    }
}