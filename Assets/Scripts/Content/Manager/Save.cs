using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<ItemInfo> itemInfoList;

    public Save()
    {
        itemInfoList = new List<ItemInfo>();
    }
}
