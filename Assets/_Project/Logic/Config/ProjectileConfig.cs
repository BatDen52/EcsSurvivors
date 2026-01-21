using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private float _speed = 12f;
    [SerializeField] private int _damage = 25;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private EntityLink _projectilePrefab;

    public float Speed => _speed;
    public int Damage => _damage;
    public float Lifetime => _lifetime;
    public EntityLink ProjectilePrefab => _projectilePrefab;
}
