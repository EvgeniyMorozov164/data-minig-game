using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ulong _bits;
    [SerializeField] private ulong _BPS;
    [SerializeField] private ulong _BPC = 1;

    public ulong Bits => _bits;
    public ulong BitsPerSecond => _BPS;
    public ulong BitsPerClick => _BPC;

    public static GameManager Instance;

    private float _timer = 0f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1f)
        {
            _timer = 0;
            AddBitsPerSecond();
        }
    }
    public void AddBits(ulong value)
    {
        _bits += value;
    }

    public void AddBPS(ulong value)
    {
        _BPS += value;
    }

    public void AddBPC(ulong value)
    {
        _BPC += value;
    }

    public void Click()
    {
        _bits += _BPC;
    }

    private void AddBitsPerSecond()
    {
        if (_BPS > 0)
        {
            _bits += _BPS;
            if (UIManager.Instance != null)
                UIManager.Instance.UpdateUI();
        }
    }
}
