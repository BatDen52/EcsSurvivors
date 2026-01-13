using Leopotam.EcsLite;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    private EcsWorld _world;

    private void OnCollisionEnter(Collision collision)
    {
        if (_world == null || _world.IsAlive() == false)
            return;

        var entity = GetComponent<EntityLink>()?.Entity;

        if (entity == null) 
            return;

        if(_world.GetPool<CollisionEvent>().Has(entity.Value))
            return;

        ref var collisionEvent = ref _world.GetPool<CollisionEvent>().Add(entity.Value);
        collisionEvent.Other = collision.gameObject;
    }

    public void Initialize(EcsWorld world)
    {
        _world = world;
    }
}