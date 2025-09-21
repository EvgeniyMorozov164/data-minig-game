using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ulong _bits = 0;
    [SerializeField] private ulong _BPS = 0;
    [SerializeField] private ulong _BPC = 1;

    public ulong Bits => _bits;
    public ulong BitsPerSecond => _BPS;
    public ulong BitsPerClick => _BPC;

    public static GameManager Instance;

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
}
