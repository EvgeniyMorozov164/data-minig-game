using UnityEngine;
[System.Serializable]
public class ShopItem
{
    [SerializeField] private string _itemName;
    //[SerializeField] private string _description;
    [SerializeField] private ulong _baseCost;
    [SerializeField] private ulong _boostValue;
    [SerializeField] private bool _isBPSItem;
    [SerializeField] private float _costMultiplier = 1.15f;

    [SerializeField] private ulong _currentCost;
    [SerializeField] private uint _amount;

    public string ItemName => _itemName;
   // public string Description => _description;
    public ulong BaseCost => _baseCost;
    public ulong BoostValue => _boostValue;
    public bool IsBPSItem => _isBPSItem;
    public float CostMultiplier => _costMultiplier;

    public ulong CurrentCost => _currentCost;
    public uint Amount => _amount;

    public ShopItem (string name, ulong baseCost, ulong boostValue, bool isBPSItem)
    {
        _itemName = name;
        
        _baseCost = baseCost;
        _boostValue = boostValue;
        _isBPSItem = isBPSItem;
        _amount = 0;
    }

    //public bool Purchase(ulong playerBits)
    //{
    //    if (playerBits >= _currentCost)
    //    {
    //        playerBits -= _currentCost;
    //        _amount++;
    //        _currentCost = (ulong)(_baseCost * Mathf.Pow(_costMultiplier, _amount));
    //        return true;
    //    }
    //    return false;
    //}

    public bool CanAfford(ulong playerbits)
    {
        return playerbits >= _currentCost;
    }

    public void ApplyPurchase()
    {
        _amount++;
       _currentCost = (ulong)(_baseCost * Mathf.Pow(_costMultiplier, _amount));
    }

    public void Initialize()
    {
        _currentCost = _baseCost;
    }
}
