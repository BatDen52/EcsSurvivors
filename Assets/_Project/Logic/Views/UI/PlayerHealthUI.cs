using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetHealth(int currentHealth)
    {
        _healthText.text = currentHealth.ToString();
    }
}