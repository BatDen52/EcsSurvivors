using TMPro;
using UnityEngine;

public class PlayerCoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void SetCoins(int coinCount)
    {
        _coinsText.text = coinCount.ToString();
    }
}