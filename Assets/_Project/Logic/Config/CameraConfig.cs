using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig")]
public class CameraConfig : ScriptableObject
{
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private Vector3 _offset;

    public float FollowSpeed => _followSpeed;
    public Vector3 Offset => _offset;
}
