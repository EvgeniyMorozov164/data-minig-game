using System.Collections.Generic;
using UnityEngine;
using YG;
public class GameManager : MonoBehaviour
{
     public ulong Bits => YG2.saves.bits;
    public ulong BitsPerSecond => YG2.saves.BPS;
    public ulong BitsPerClick => YG2.saves.BPC;

    public static GameManager Instance;

    private float _timer = 0f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            YG2.onGetSDKData += OnDataLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        YG2.onGetSDKData -= OnDataLoaded;
    }

    private void OnDataLoaded()
    {
        Debug.Log($"=== ДАННЫЕ ЗАГРУЖЕНЫ ===");
        Debug.Log($"Bits: {YG2.saves.bits}, BPS: {YG2.saves.BPS}, BPC: {YG2.saves.BPC}");
        Debug.Log($"Улучшений: {YG2.saves.shopItemsData?.Count ?? 0}");

        
        if (YG2.saves.shopItemsData == null)
        {
            Debug.LogWarning("shopItemsData is null! Инициализируем...");
            YG2.saves.shopItemsData = new List<ShopItemData>();
        }

        if (YG2.isFirstGameSession)
        {
            Debug.Log("Первый запуск! Устанавливаем значения по умолчанию");
            YG2.saves.SetDefaults();
        }

        if (ShopManager.Instance != null)
            ShopManager.Instance.ApplyLoadedUpgrades();

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateUI();

        UpdateAllShopButtons();
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
        YG2.saves.bits += value;
        UIManager.Instance?.UpdateUI();
    }

    public void AddBPS(ulong value)
    {
        YG2.saves.BPS += value;
    }

    public void AddBPC(ulong value)
    {
        YG2.saves.BPC += value;
    }

    public void Click()
    {
        YG2.saves.bits += YG2.saves.BPC;
        UIManager.Instance?.UpdateUI();

        YG2.SaveProgress();
    }

    private void AddBitsPerSecond()
    {
        if (YG2.saves.BPS > 0)
        {
            YG2.saves.bits += YG2.saves.BPS;
            UIManager.Instance?.UpdateUI();

            if (Time.time % 10f < Time.deltaTime)
            {
                YG2.SaveProgress();
                Debug.Log("СОХРАНЕНИЕ!");
            }
        }
    }

    public bool SubtractBits(ulong value)
    {
        if (value > YG2.saves.bits)
        {
            Debug.Log($"Недостаточно битов: нужно {value}, есть {YG2.saves.bits}");

            return false;
        }

        YG2.saves.bits -= value;
        Debug.Log($"Списано {value} битов, осталось {YG2.saves.bits}");
        UIManager.Instance?.UpdateUI();

        return true;
    }

    private void UpdateAllShopButtons()
    {
        ShopButton[] allShopButtons = FindObjectsOfType<ShopButton>();
        foreach (ShopButton button in allShopButtons)
        {
            button.UpdateButtonUI();
        }
    }
}
