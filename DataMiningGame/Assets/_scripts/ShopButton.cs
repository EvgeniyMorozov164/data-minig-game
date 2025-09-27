using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private int _itemIndex;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _costText;    

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        if (ShopManager.Instance == null)
        {
            Debug.LogError("ShopButton: ShopManager is null!");
            return;
        }

        _button.onClick.AddListener(OnButtonClick);
        UpdateButtonUI();
    }

    void OnButtonClick()
    {
        ShopManager.Instance.PurchaseItem(_itemIndex);
        UpdateButtonUI();
    }

    public void UpdateButtonUI()
    {
        if (ShopManager.Instance == null) return;

        ShopItem item = ShopManager.Instance.GetShopItem(_itemIndex);

        if (item == null) return;

        _nameText.text = item.ItemName;
        _costText.text = $"{item.CurrentCost} Bits";
    }
}
