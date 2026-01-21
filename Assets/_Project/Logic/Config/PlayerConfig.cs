using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _shootCooldown = 1f;

    public int MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public float ShootCooldown => _shootCooldown;
}
