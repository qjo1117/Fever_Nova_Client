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
    // 데이터시트와 함께 사용하기 위해서는 데이터 시트 변경 될떄 마다 m_ItemPath 같이 변경 해주어야함.
    // ex)  

    public string[]                    m_ItemPath =             
    {
        Path.Health
    };

    public Transform                m_parentItemPoint;      // 아이템이 생성될 오브젝트들의 부모 Transfrom

    [SerializeField]
    private List<ItemInfo>       m_listItemInfo;         // Json에서 가져온 ItemInfo리스트

    private ItemSave                      m_saveLoadBuf;          // Json 세이브, 로드한 데이터 임시저장하는 버퍼


    public void Init()
    {
        ItemInfoLoad();
        ItemLoad();
    }

    // ItemPoints 오브젝트 아래에 존재하는 SpawnPoint 오브젝트들을 찾아서 Transform.position (Vector3)값과 id값을
    // 가져온다음 이 값들을 Json파일로 출력해주는 함수
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


    // Json파일에서 Load한 아이템 정보를 통해 실제 아이템오브젝트를 생성하는 함수
    public void ItemLoad()
    {
        int l_count = m_listItemInfo.Count;

        for (int i = 0; i < l_count; i++)
        {
            Spawn(m_listItemInfo[i].id, m_listItemInfo[i].position);
        }
    }

    // Json파일에서 아이템 정보를 Load하는 함수
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

    // ItemPoints 오브젝트 아래에 존재하는 SpawnPoint 오브젝트들을 찾아서 
    // Transform.position => m_listItemInfo[index].position 으로 변환 하는 함수
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

    // 버퍼에 Json파일로 출력할 Data 복사하는 함수
    private void InitializeSaveData()
    {
        foreach (ItemInfo info in m_listItemInfo)
        {
            m_saveLoadBuf.itemInfoList.Add(info);
        }
    }

    // 버퍼에서 Json파일에서 불러온 Data 가져오는 함수
    private void InitializeLoadData()
    {
        foreach (ItemInfo info in m_saveLoadBuf.itemInfoList)
        {
            m_listItemInfo.Add(info);
        }
    }

    // 아이템 정보 리스트와, ItemSpawnPoint 오브젝트가 존재하는지 체크하고
    // 오브젝트가 존재하면 Clear하는 함수 (ItemLoad시 ItemSpawnPoint 오브젝트가 남아있으면 안되기떄문)
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

    // 버퍼의 Data를 Json파일로 출력하는 함수
    private void JsonFileSave()
    {
        string toJson = JsonUtility.ToJson(m_saveLoadBuf, true);
        File.WriteAllText(Application.dataPath + "/Data/ItemInfos.json", toJson);

        Debug.Log("Saved!");
    }

    // Json파일의 Data를 버퍼로 가져오는 함수
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
