using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Configs/GameplayConfig")]
public class GameplayConfig : ScriptableObject
{
    [SerializeField] private string _obstacleTag = "Obstacle";
    [SerializeField] private float _healthBarHeightOffset = 1.5f;

    public string ObstacleTag => _obstacleTag;
    public float HealthBarHeightOffset => _healthBarHeightOffset;
}
