using Leopotam.EcsLite;
using UnityEngine;

public abstract class BaseFactory<T> : IEntityFactory where T : struct
{
    public abstract int Create(EcsWorld world);

    protected void SetupTransform<TComponent>(EcsWorld world, int entity, Transform transform)
        where TComponent : struct
    {
        world.GetPool<TransformRef>().Add(entity).Value = transform;
        world.GetPool<RigidbodyRef>().Add(entity).Value = transform.GetComponent<Rigidbody>();
        world.GetPool<TComponent>().Add(entity);
    }
}