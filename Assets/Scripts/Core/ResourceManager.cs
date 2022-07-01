using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
	#region Resource Folder

	public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject)) {
            // ObjectPoolManager에 있는 녀석인지 확인한다.
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0) {
                name = name.Substring(index + 1);
            }
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null) {
                return go as T;
            }
        }
        return Resources.Load<T>(path);
    }

    // Resource에서 저장하는 것이 아니라 불러오는 것이다.
    // 거의 Resource폴더를 접근해서 Prefab,Sound,Particle등을 참조하기 쉽게 만들어둔 PathManager느낌
    public GameObject NewPrefab(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null) {
            Debug.Log($"Failed to Load prefab : {path}");
            return null;
        }

        // Poolable을 가지고 있으면 PoolManager에 있는 녀석이다.
        if (original.GetComponent<Poolable>() != null) {
            return Managers.Pool.Pop(original, parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    // 생성                   [0]
    // 풀링 등록               [Destroy]
    // 생성 지운다.

    public void DelPrefab(GameObject go)
    {
        if (go == null) {
            return;
        }

        // 만약에 풀링이 필요한 아이라면 -> 풀링 매니저한테 맡겨진다.
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null){
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public void Destroy(GameObject go, float time)
    {
        if (go == null) {
            return;
        }

        Object.Destroy(go);
    }

	#endregion


	#region Addressable


	List<AsyncOperationHandle> m_listAddressable = new List<AsyncOperationHandle>();

	// Addressable은 비동기 방식이기때문에 오브젝트가 바로 실행되지않는다.
	// 그러므로 해당하는 번들을 로딩하는 시간이 필요하므로 씬 전환할때 Fade Out 써야할듯

	public void Clear()
	{
		foreach (AsyncOperationHandle handle in m_listAddressable) {
			Addressables.Release(handle);
		}
	}

	public GameObject Instantiate(string p_key, Transform p_parent = null)
	{
		// ObjectPoolManager에 있는 녀석인지 확인한다.
		string name = p_key;
		int index = name.LastIndexOf('/');
		if (index >= 0)
		{
			name = name.Substring(index + 1);
		}

		// 오브젝트에 등록이 되어있는지 확인한다.
		GameObject obj = Managers.Pool.GetOriginal(name);

		// 풀에 등록이 안되어있다면 호출을 한다.
		if (obj == null)
		{
			Addressables.InstantiateAsync(p_key, p_parent).Completed +=
				(AsyncOperationHandle<GameObject> p_obj) =>
				{
					obj = p_obj.Result;
				};
		}

		// 등록된녀석이면 그냥 실행해준다.
		return Managers.Pool.Pop(obj, p_parent).gameObject;
	}

	public void Destroy(GameObject p_obj)
	{
		if (p_obj == null)
		{
			return;
		}

		// 만약에 풀링이 필요한 아이라면 -> 풀링 매니저한테 맡겨진다.
		Poolable poolable = p_obj.GetComponent<Poolable>();
		if (poolable != null)
		{
			Managers.Pool.Push(poolable);
			return;
		}

		Addressables.ReleaseInstance(p_obj);
	}

	// 등록만 하기 때문에 생성은 되지않는다.
	public void RegisterPoolGameObject(string p_key)
	{
		Addressables.LoadAssetAsync<GameObject>(p_key).Completed +=
			(AsyncOperationHandle<GameObject> p_obj) =>
			{
				GameObject result = p_obj.Result;
				Managers.Pool.CreatePool(result);
				m_listAddressable.Add(p_obj);               // Ref카운딩
			};
	}

	#endregion

}
