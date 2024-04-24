using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    public void Init(int itemId, int amount)
    {
        itemImage.sprite = ItemResourcesHelper.Instance.GetItemResource((ItemEnum)itemId).itemSprite;
        itemImage.SetNativeSize();
        SetImageScale(itemId);

        if (amount == 1) amountText.gameObject.SetActive(false);
        else amountText.text = $"x{amount}";
    }

    private void SetImageScale(int itemId)
    {
        switch ((ItemEnum)itemId)
        {
            case ItemEnum.Cash:
            case ItemEnum.Gold:
            case ItemEnum.Glasses:
                itemImage.transform.localScale = Vector3.one;
                break;

            case ItemEnum.Cap:
                itemImage.transform.localScale = Vector3.one * .75f;
                break;

            case ItemEnum.Chest:
            case ItemEnum.Grenade:
                itemImage.transform.localScale = Vector3.one * .5f;
                break;

            case ItemEnum.Bayonet:
            case ItemEnum.Sniper:
                itemImage.transform.localScale = Vector3.one * .4f;
                break;
        }
    }
}
