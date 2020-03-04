using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<ItemData> items;

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    public ItemData GetItem(int index)
    {
        return items[index];
    }
    
    public int GetItemCount()
    {
        return items.Count;
    }

    public List<ItemData> GetItems()
    {
        return items;
    }

}
