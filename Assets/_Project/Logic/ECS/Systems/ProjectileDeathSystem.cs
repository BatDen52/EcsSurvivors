using Leopotam.EcsLite;

public class ProjectileDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _deadProjectilesFilter;
    private EcsPool<DeadTag> _deadPool;
    private EcsPool<ProjectileTag> _projectilesPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _deadProjectilesFilter = _world.Filter<DeadTag>().Inc<ProjectileTag>().End();
        _deadPool = _world.GetPool<DeadTag>();
        _projectilesPool = _world.GetPool<ProjectileTag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var projectile in _deadProjectilesFilter)
        {
            _world.GetPool<DestroyRequest>().Add(projectile);
        }
    }
}
