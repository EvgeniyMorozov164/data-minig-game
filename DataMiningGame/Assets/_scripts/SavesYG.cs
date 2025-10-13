using UnityEngine;
using System;
using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        public ulong bits = 0;
        public ulong BPS = 0;
        public ulong BPC = 1;

        // «јћ≈Ќя≈ћ List<int> на полную информацию об улучшени€х
        public List<ShopItemData> shopItemsData = new List<ShopItemData>();

        public SavesYG()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            bits = 0;
            BPS = 0;
            BPC = 1;
            shopItemsData.Clear();
        }
    }

    // ƒќЅј¬Ћя≈ћ класс дл€ сохранени€ данных ShopItem
    [System.Serializable]
    public class ShopItemData
    {
        public int itemIndex;
        public uint amount;
        public ulong currentCost;

        public ShopItemData(int index, uint amt, ulong cost)
        {
            itemIndex = index;
            amount = amt;
            currentCost = cost;
        }
    }
}