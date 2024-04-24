using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private ItemView itemView;
    public void Init(int itemId, int amount)
    {
        itemView.Init(itemId, amount);
    }
}
