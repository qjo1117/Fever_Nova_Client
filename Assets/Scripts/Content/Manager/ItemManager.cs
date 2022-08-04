using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    public int id;
    public Vector3 position;
}

public class ItemManager : MonoBehaviour
{
    // �����ͽ�Ʈ�� �Բ� ����ϱ� ���ؼ��� ������ ��Ʈ ���� �ɋ� ���� m_ItemPath ���� ���� ���־����.
    // ex)  

    public string[] m_ItemPath =             
    {
        Path.Health
    };

    public Transform                        m_parentItemPoint;      // �������� ������ ������Ʈ���� �θ� Transfrom
    [SerializeField]
    private List<ItemInfo>                  m_listItemInfo;         // Json���� ������ ItemInfo����Ʈ
    private ItemSave                        m_saveLoadBuf;          // Json ���̺�, �ε��� ������ �ӽ������ϴ� ����


    public void Init()
    {
        //ItemInfoLoad();
        //ItemLoad();
    }

    public void OnUpdate()
    {

    }

    // ItemPoints ������Ʈ �Ʒ��� �����ϴ� SpawnPoint ������Ʈ���� ã�Ƽ� Transform.position (Vector3)���� id����
    // �����´��� �� ������ Json���Ϸ� ������ִ� �Լ�
    [ContextMenu("ItemInfoSave")]
    public void ItemInfoSave()
    {
        m_saveLoadBuf = new ItemSave();
        if(m_parentItemPoint == null)
        {
            m_parentItemPoint = Util.FindChild<Transform>(gameObject, "ItemPoints");
        }

        m_listItemInfo.Clear();

        TransformToVector();
        InitializeSaveData();
        JsonFileSave();
    }

    public void Spawn(int _id, Vector3 _spawnPosition)
    {
        BaseItem l_newItem;
        l_newItem = Managers.Resource.Instantiate(m_ItemPath[_id]).GetComponent<BaseItem>();

        l_newItem.transform.parent = m_parentItemPoint;
        l_newItem.transform.position = _spawnPosition;
    }


    // Json���Ͽ��� Load�� ������ ������ ���� ���� �����ۿ�����Ʈ�� �����ϴ� �Լ�
    public void ItemLoad()
    {
        int l_count = m_listItemInfo.Count;

        for (int i = 0; i < l_count; i++)
        {
            Spawn(m_listItemInfo[i].id, m_listItemInfo[i].position);
        }
    }

    // Json���Ͽ��� ������ ������ Load�ϴ� �Լ�
    public void ItemInfoLoad()
    {
        m_saveLoadBuf = new ItemSave();
        if (m_parentItemPoint == null)
        {
            m_parentItemPoint = Util.FindChild<Transform>(gameObject, "ItemPoints");
        }

        ObjectExistCheck();
        JsonFileLoad();
        InitializeLoadData();
    }

    // ItemPoints ������Ʈ �Ʒ��� �����ϴ� SpawnPoint ������Ʈ���� ã�Ƽ� 
    // Transform.position => m_listItemInfo[index].position ���� ��ȯ �ϴ� �Լ�
    private void TransformToVector()
    {
        ItemInfo l_iteminfo;
        ItemSpawnPoint l_itemSpawnPoint;

        for (int i = 0; i < m_parentItemPoint.childCount; i++)
        {
            l_iteminfo = new ItemInfo();
            l_itemSpawnPoint = m_parentItemPoint.GetChild(i).GetComponent<ItemSpawnPoint>();

            l_iteminfo.id = l_itemSpawnPoint.m_index;
            l_iteminfo.position = l_itemSpawnPoint.transform.position;

            m_listItemInfo.Add(l_iteminfo);
        }
    }

    // ���ۿ� Json���Ϸ� ����� Data �����ϴ� �Լ�
    private void InitializeSaveData()
    {
        foreach (ItemInfo info in m_listItemInfo)
        {
            m_saveLoadBuf.itemInfoList.Add(info);
        }
    }

    // ���ۿ��� Json���Ͽ��� �ҷ��� Data �������� �Լ�
    private void InitializeLoadData()
    {
        foreach (ItemInfo info in m_saveLoadBuf.itemInfoList)
        {
            m_listItemInfo.Add(info);
        }
    }

    // ������ ���� ����Ʈ��, ItemSpawnPoint ������Ʈ�� �����ϴ��� üũ�ϰ�
    // ������Ʈ�� �����ϸ� Clear�ϴ� �Լ� (ItemLoad�� ItemSpawnPoint ������Ʈ�� ���������� �ȵǱ⋚��)
    private void ObjectExistCheck()
    {
        if (m_listItemInfo.Count > 0) 
        {
            m_listItemInfo.Clear();
        }

        foreach (Transform trasnform in m_parentItemPoint)
        {
            Destroy(trasnform.gameObject);
        }
    }

    // ������ Data�� Json���Ϸ� ����ϴ� �Լ�
    private void JsonFileSave()
    {
        string toJson = JsonUtility.ToJson(m_saveLoadBuf, true);
        File.WriteAllText(Application.dataPath + "/Data/ItemInfos.json", toJson);

        Debug.Log("Saved!");
    }

    // Json������ Data�� ���۷� �������� �Լ�
    private void JsonFileLoad()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Data/ItemInfos.json");
        m_saveLoadBuf = JsonUtility.FromJson<ItemSave>(jsonString);

        Debug.Log("Loaded!!");
    }

    public void Clear()
    {
        foreach (Transform trasnform in m_parentItemPoint)
        {
            Destroy(trasnform.gameObject);
        }

        m_listItemInfo.Clear();
    }
}
