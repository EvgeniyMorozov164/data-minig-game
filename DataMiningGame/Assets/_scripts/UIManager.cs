using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bitsTotalText;
    [SerializeField] private TextMeshProUGUI _BPSText;
    [SerializeField] private TextMeshProUGUI _BPCText;

    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        _bitsTotalText.text = $"Bits: {GameManager.Instance.Bits}";
        _BPSText.text = $"BPS: {GameManager.Instance.BitsPerSecond}";
        _BPCText.text = $"BPC: {GameManager.Instance.BitsPerClick}";
    }
}
