using System.Collections.Generic;
using System.Linq;

public static class Inventory
{
    private static Dictionary<int, int> _inventoryDictionary = new();

    public static void AddItemToInventory(int itemId, int itemAmount)
    {
        if (_inventoryDictionary.ContainsKey(itemId))
            _inventoryDictionary[itemId] += itemAmount;
        else
            _inventoryDictionary.Add(itemId, itemAmount);
    }

    public static int GetItemAmount(int itemId)
    {
        return _inventoryDictionary.ContainsKey(itemId) ? _inventoryDictionary[itemId] : 0;
    }

    public static Dictionary<int, int> GetCurrentInventory()
    {
        return _inventoryDictionary.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
    }

    public static bool UpdateInventory(int itemId, int changeAmount)
    {
        if (!_inventoryDictionary.ContainsKey(itemId)) return false;

        _inventoryDictionary[itemId] += changeAmount;
        return true;
    }
}
