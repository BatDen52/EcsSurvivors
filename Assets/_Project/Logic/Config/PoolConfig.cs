using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Configs/PoolConfig")]
public class PoolConfig : ScriptableObject
{
    [SerializeField] private int _projectilePoolSize = 20;
    [SerializeField] private int _enemyPoolSize = 15;
    [SerializeField] private int _coinPoolSize = 10;

    public int ProjectilePoolSize => _projectilePoolSize;
    public int EnemyPoolSize => _enemyPoolSize;
    public int CoinPoolSize => _coinPoolSize;
}
