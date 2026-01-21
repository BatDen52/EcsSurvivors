using Leopotam.EcsLite;
using UnityEngine;

public abstract class BaseFactory<T> : IEntityFactory where T : struct
{
    public abstract int Create(EcsWorld world);

    protected void SetupTransform(EcsWorld world, int entity, Transform transform)
    {
        world.GetPool<TransformRef>().Add(entity).Value = transform;
        world.GetPool<T>().Add(entity);

        if (transform.TryGetComponent(out Rigidbody rigidbody))
            world.GetPool<RigidbodyRef>().Add(entity).Value = rigidbody;
    }
}