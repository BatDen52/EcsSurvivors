using UnityEngine;

public class PoolService : MonoBehaviour
{
    [SerializeField] public Transform _projectilePrefab;
    [SerializeField] public Transform _enemyPrefab;
    [SerializeField] public Transform _coinPrefab;

    public ObjectPool<EntityLink> ProjectilePool { get; private set; }
    public ObjectPool<EntityLink> EnemyPool { get; private set; }
    public ObjectPool<EntityLink> CoinPool { get; private set; }

    public static PoolService Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Initialize(Transform projectilePrefab, Transform enemyPrefab, Transform coinPrefab, GameConfig config)
    {
        _projectilePrefab = projectilePrefab;
        _enemyPrefab = enemyPrefab;
        _coinPrefab = coinPrefab;

        ProjectilePool = new ObjectPool<EntityLink>(_projectilePrefab.GetComponent<EntityLink>(), config.ProjectilePoolSize);
        EnemyPool = new ObjectPool<EntityLink>(_enemyPrefab.GetComponent<EntityLink>(), config.EnemyPoolSize);
        CoinPool = new ObjectPool<EntityLink>(_coinPrefab.GetComponent<EntityLink>(), config.CoinPoolSize);
    }
}
