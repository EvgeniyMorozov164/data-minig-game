using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bitsTotalText;
    [SerializeField] private TextMeshProUGUI _BPSText;
    [SerializeField] private TextMeshProUGUI _BPCText;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        if (GameManager.Instance != null)
        {
            _bitsTotalText.text = $"Bits: {GameManager.Instance.Bits}";
            _BPSText.text = $"BPS: {GameManager.Instance.BitsPerSecond}";
            _BPCText.text = $"BPC: {GameManager.Instance.BitsPerClick}";
        }
        else        
            Debug.LogWarning("GameManager.Instance is null!");
    }
}
