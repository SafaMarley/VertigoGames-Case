using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CellView : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI amountText;
    public void Init(int itemId, int amount)
    {
        amountText.text = $"x{amount}";
    }
}
