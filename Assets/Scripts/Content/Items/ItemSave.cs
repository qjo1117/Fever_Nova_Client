using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSave
{
    public List<ItemInfo> itemInfoList;

    public ItemSave()
    {
        itemInfoList = new List<ItemInfo>();
    }
}
