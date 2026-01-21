using UnityEngine;

[CreateAssetMenu(fileName = "CoinConfig", menuName = "Configs/CoinConfig")]
public class CoinConfig : ScriptableObject
{
    [SerializeField] private float _dropChance = 0.5f;
    [SerializeField] private float _collectionDistance = 1f;

    public float DropChance => _dropChance;
    public float CollectionDistance => _collectionDistance;
}
