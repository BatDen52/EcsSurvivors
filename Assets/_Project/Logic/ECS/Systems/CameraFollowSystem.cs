using Leopotam.EcsLite;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly CameraConfig _config;
    private readonly Camera _camera;
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsPool<TransformRef> _transformsPool;

    public CameraFollowSystem(CameraConfig config, Camera camera)
    {
        _config = config;
        _camera = camera;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformRef>().End();
        _transformsPool = _world.GetPool<TransformRef>();
    }

    public void Run(IEcsSystems systems)
    {
        if (_playerFilter.GetEntitiesCount() == 0) 
            return;

        Vector3 targetPosition = Vector3.zero;
        Transform target = null;

        foreach (var player in _playerFilter)
        {
            target = _transformsPool.Get(player).Value;
            targetPosition = target.position + _config.Offset;
            break;
        }

        _camera.transform.position = Vector3.Lerp(
            _camera.transform.position,
            targetPosition,
            _config.FollowSpeed * Time.deltaTime
        );

        _camera.transform.LookAt(target);
    }
}