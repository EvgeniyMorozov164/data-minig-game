using UnityEngine;
using System.Collections.Generic;
using YG;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> _shopItems = new List<ShopItem>();
    private System.Collections.ObjectModel.ReadOnlyCollection<ShopItem> _readOnlyShopItem;

    public static ShopManager Instance;
    public IReadOnlyCollection<ShopItem> ShopItems => _readOnlyShopItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeShopItems();
            InitItems();
            _readOnlyShopItem = _shopItems.AsReadOnly();
        }
        else
            Destroy(gameObject);
    }

    private void InitializeShopItems()
    {
        _shopItems.Clear();
        _shopItems.Add(new ShopItem("640 Kb", 8, 1, true));
        _shopItems.Add(new ShopItem("Double Click", 16, 1, false));
        _shopItems.Add(new ShopItem("Script Bot", 128, 5, true));
        _shopItems.Add(new ShopItem("Click-Click", 256, 7, false));
    }

    public ShopItem GetShopItem(int index)
    {
        if (index < 0 || index >= _shopItems.Count)
        {
            Debug.LogError($"ShopManager: Индекс {index} не существует!");
            return null;
        }
        return _shopItems[index];
    }

    public int GetShopItemsCount() => _shopItems.Count;

    public void PurchaseItem(int itemIndex)
    {
        Debug.Log($"Попытка покупки улучшения {itemIndex}, битов: {YG2.saves.bits}");

        if (itemIndex < 0 || itemIndex >= _shopItems.Count)
        {
            Debug.LogError("ShopManager: Неверный индекс улучшения!");
            return;
        }

        ShopItem item = _shopItems[itemIndex];
        ulong cost = item.CurrentCost;

        if (item.CanAfford(YG2.saves.bits))
        {
            Debug.Log($"Можно купить! Стоимость: {cost}");
            if (GameManager.Instance.SubtractBits(cost))
            {
                item.ApplyPurchase();

                // НЕМЕДЛЕННО применяем улучшение
                if (item.IsBPSItem)
                    GameManager.Instance.AddBPS(item.BoostValue);
                else
                    GameManager.Instance.AddBPC(item.BoostValue);

                // Сохраняем полные данные об улучшении
                SaveShopItemData(itemIndex, item.Amount, item.CurrentCost);

                // ВЫЗЫВАЕМ СОХРАНЕНИЕ
                YG2.SaveProgress();

                Debug.Log($"Улучшение куплено: {item.ItemName}");
                UpdateAllShopButtons();

                // НЕМЕДЛЕННО обновляем UI
                UIManager.Instance?.UpdateUI();
            }
        }
        else
            Debug.Log("Недостаточно Bits для покупки!");
    }

    // Сохраняем данные улучшения
    private void SaveShopItemData(int index, uint amount, ulong currentCost)
    {
        // Ищем уже существующие данные
        ShopItemData existingData = YG2.saves.shopItemsData.Find(data => data.itemIndex == index);

        if (existingData != null)
        {
            // Обновляем существующие
            existingData.amount = amount;
            existingData.currentCost = currentCost;
        }
        else
        {
            // Создаем новые
            YG2.saves.shopItemsData.Add(new ShopItemData(index, amount, currentCost));
        }
    }

    // Загружаем данные улучшений
    private void LoadShopItemData()
    {
        foreach (ShopItemData data in YG2.saves.shopItemsData)
        {
            if (data.itemIndex >= 0 && data.itemIndex < _shopItems.Count)
            {
                ShopItem item = _shopItems[data.itemIndex];
                // Восстанавливаем состояние ShopItem
                item.SetLoadedData(data.amount, data.currentCost);
            }
        }
    }

    private void UpdateAllShopButtons()
    {
        ShopButton[] allShopButtons = FindObjectsOfType<ShopButton>();
        foreach (ShopButton button in allShopButtons)
        {
            button.UpdateButtonUI();
        }
    }

    private void InitItems()
    {
        foreach (ShopItem item in _shopItems)
        {
            item.Initialize();
        }
    }

    public void ApplyLoadedUpgrades()
    {
        // Сначала загружаем данные улучшений
        LoadShopItemData();

        // Сбрасываем статистику
        YG2.saves.BPS = 0;
        YG2.saves.BPC = 1;

        // Применяем все купленные улучшения
        foreach (ShopItemData data in YG2.saves.shopItemsData)
        {
            ShopItem item = GetShopItem(data.itemIndex);
            if (item != null && data.amount > 0)
            {
                if (item.IsBPSItem)
                    YG2.saves.BPS += item.BoostValue * data.amount;
                else
                    YG2.saves.BPC += item.BoostValue * data.amount;
            }
        }

        // Немедленно обновляем UI
        UIManager.Instance?.UpdateUI();
    }
}