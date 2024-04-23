using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    public void Init(int itemId, int amount)
    {
        amountText.text = $"x{amount}";
    }
}
