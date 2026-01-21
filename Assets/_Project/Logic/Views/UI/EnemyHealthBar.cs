using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void UpdateHealth(int current, int max)
    {
        _healthSlider.value = (float)current / max;
        _healthText.text = current.ToString();
    }

    public void SetPosition(Vector3 worldPosition, Camera camera, GameplayConfig config)
    {
        transform.position = worldPosition + Vector3.up * config.HealthBarHeightOffset;
        transform.LookAt(2 * worldPosition - camera.transform.position);
    }
}
