using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

public class SpatialCacheSystem : IEcsInitSystem, IEcsRunSystem
{
    private const float UpdateInterval = 0.2f; 

    private EcsWorld _world;
    private Dictionary<int, Vector3> _enemyPositions = new Dictionary<int, Vector3>();
    private Dictionary<int, float> _lastUpdateTimes = new Dictionary<int, float>();

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
    }

    public void Run(IEcsSystems systems)
    {
        var filter = _world.Filter<EnemyTag>().Inc<TransformRef>().End();
        var transforms = _world.GetPool<TransformRef>();

        foreach (var enemy in filter)
        {
            if (!_lastUpdateTimes.ContainsKey(enemy) || Time.time - _lastUpdateTimes[enemy] > UpdateInterval)
            {
                _enemyPositions[enemy] = transforms.Get(enemy).Value.position;
                _lastUpdateTimes[enemy] = Time.time;
            }
        }
    }

    public Vector3? GetNearestEnemyPosition(Vector3 playerPosition)
    {
        if (_enemyPositions.Count == 0) return null;

        Vector3? nearestPosition = null;
        float minSqrDistance = float.MaxValue;

        foreach (var position in _enemyPositions.Values)
        {
            float sqrDistance = playerPosition.SqrDistance(position);
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestPosition = position;
            }
        }

        return nearestPosition;
    }
}
