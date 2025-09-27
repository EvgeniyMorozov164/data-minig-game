using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> _shopItems = new List<ShopItem>();
    private ReadOnlyCollection<ShopItem> _readOnlyShopItem;

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
        _shopItems.Add(new ShopItem("Double Click", 15, 1, false));
        _shopItems.Add(new ShopItem("Script Bot", 128, 5, true));
        _shopItems.Add(new ShopItem("Click-Click", 256, 7, false));
    }

    public ShopItem GetShopItem(int index)
    {
        if (index < 0 || index >= _shopItems.Count)
        {
            Debug.LogError($"ShopManager: Индекс {index} вне диапазона!");
            return null;
        }
        return _shopItems[index];
    }

    public int GetShopItemsCount() => _shopItems.Count;

    public void PurchaseItem(int itemIndex)
    {
        Debug.Log($"Попытка покупки предмета {itemIndex}, битов: {GameManager.Instance.Bits}");

        if (itemIndex < 0 || itemIndex >= _shopItems.Count)
        {
            Debug.LogError("ShopManager: Неверный индекс улучшения!");
            return;
        }

        ShopItem item = _shopItems[itemIndex];
        ulong cost = item.CurrentCost;

        if (item.CanAfford(GameManager.Instance.Bits))
        {
            Debug.Log($"Битов хватает! Стоимость: {cost}");
            if (GameManager.Instance.SubtractBits(cost))
            {
                Debug.Log("Покупка успешна!");

                item.ApplyPurchase();

                if (item.IsBPSItem)            
                    GameManager.Instance.AddBPS(item.BoostValue);
                else
                    GameManager.Instance.AddBPC(item.BoostValue);


                Debug.Log($"Куплено улучшение: {item.ItemName}");
                UpdateAllShopButtons();
            }
        }
        else
            Debug.Log("Недостаточно Bits для покупки!");
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
}
