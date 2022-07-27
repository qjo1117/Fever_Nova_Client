using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
	public class DestroyObject
	{
		public GameObject obj = null;
		public float delayTime = 0.0f;
		public float currentDelayTime = 0.0f;
	}


	public int DataAsyncCount = 0;
	public int DataMaxAsyncCount = 0;

	private List<DestroyObject> m_destroys = new List<DestroyObject>();
	private Stack<DestroyObject> m_destroyPool = new Stack<DestroyObject>();

	public void Init()
	{
		DataAsyncCount = 0;
	}

	public void Update()
	{
		float l_deltatime = Time.deltaTime;
		foreach(DestroyObject destory in m_destroys) {
			destory.currentDelayTime += l_deltatime;
			if (destory.currentDelayTime > destory.delayTime) {
				Destroy(destory.obj);
				m_destroyPool.Push(destory);
				m_destroys.Remove(destory);
				break;
			}
		}
	}

	#region Resource Folder

	public T Load<T>(string _path) where T : Object
    {
        if (typeof(T) == typeof(GameObject)) {
            // ObjectPoolManager에 있는 녀석인지 확인한다.
            string name = _path;
            int index = name.LastIndexOf('/');
            if (index >= 0) {
                name = name.Substring(index + 1);
            }
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null) {
                return go as T;
            }
        }
        return Resources.Load<T>(_path);
    }

    // Resource에서 저장하는 것이 아니라 불러오는 것이다.
    // 거의 Resource폴더를 접근해서 Prefab,Sound,Particle등을 참조하기 쉽게 만들어둔 PathManager느낌
    public GameObject NewPrefab(string _path, Transform _parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{_path}");
        if (original == null) {
            Debug.Log($"Failed to Load prefab : {_path}");
            return null;
        }

        // Poolable을 가지고 있으면 PoolManager에 있는 녀석이다.
        if (original.GetComponent<Poolable>() != null) {
            return Managers.Pool.Pop(original, _parent).gameObject;
        }

        GameObject go = Object.Instantiate(original, _parent);
        go.name = original.name;
        return go;
    }

    // 생성                   [0]
    // 풀링 등록               [Destroy]
    // 생성 지운다.

    public void DelPrefab(GameObject _go)
    {
        if (_go == null) {
            return;
        }

        // 만약에 풀링이 필요한 아이라면 -> 풀링 매니저한테 맡겨진다.
        Poolable poolable = _go.GetComponent<Poolable>();
        if (poolable != null){
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(_go);
    }

	#endregion


	#region Addressable


	List<AsyncOperationHandle> m_listAddressable = new List<AsyncOperationHandle>();

	// Addressable은 비동기 방식이기때문에 오브젝트가 바로 실행되지않는다.
	// 그러므로 해당하는 번들을 로딩하는 시간이 필요하므로 씬 전환할때 Fade Out 써야할듯

	public void Clear()
	{
		DataAsyncCount = 0;
		DataMaxAsyncCount = 0;
		foreach (AsyncOperationHandle handle in m_listAddressable) {
			Addressables.Release(handle);
		}

		// Release했을시 InvalidOperation 남아있으므로 m_listAddressable Clear - 찬혁 -
		m_listAddressable.Clear();
	}

	public GameObject Instantiate(string _key, Transform _parent = null)
	{
		// ObjectPoolManager에 있는 녀석인지 확인한다.
		string name = _key;
		int index = name.LastIndexOf('/');
		if (index >= 0) {
			name = name.Substring(index + 1);
		}

		// 오브젝트에 등록이 되어있는지 확인한다.
		GameObject obj = Managers.Pool.GetOriginal(name);

		DataMaxAsyncCount += 1;

		// 풀에 등록이 안되어있다면 호출을 한다.
		if (obj == null) {
			Addressables.InstantiateAsync(_key, _parent).Completed +=
				(AsyncOperationHandle<GameObject> p_obj) => {
					obj = p_obj.Result;
					DataAsyncCount += 1;
				};
		}

		// 등록된녀석이면 그냥 실행해준다.
		return Managers.Pool.Pop(obj, _parent).gameObject;
	}

	public void Destroy(GameObject _obj)
	{
		if (_obj == null) {
			return;
		}

		// 만약에 풀링이 필요한 아이라면 -> 풀링 매니저한테 맡겨진다.
		Poolable poolable = _obj.GetComponent<Poolable>();
		if (poolable != null) {
			Managers.Pool.Push(poolable);
			return;
		}

		Addressables.ReleaseInstance(_obj);
	}

	public void Destroy(GameObject _obj, float _delayTime)
	{
		if(m_destroyPool.Count == 0) {
			m_destroyPool.Push(new DestroyObject());
		}
		DestroyObject l_destroy = m_destroyPool.Pop();
		l_destroy.obj = _obj;
		l_destroy.delayTime = _delayTime;
		l_destroy.currentDelayTime = 0.0f;
		m_destroys.Add(l_destroy);
	}

	// 등록만 하기 때문에 생성은 되지않는다.
	public void RegisterPoolGameObject(string _key, int _count = 10)
	{
		DataMaxAsyncCount += 1;
		Addressables.LoadAssetAsync<GameObject>(_key).Completed +=
			(AsyncOperationHandle<GameObject> p_obj) => {
				string name = _key;
				int index = name.LastIndexOf('/');
				if (index >= 0) {
					name = name.Substring(index + 1);
				}
				GameObject result = p_obj.Result;
				result.name = name;
				Managers.Pool.CreatePool(result, _count);
				m_listAddressable.Add(p_obj);               // Ref카운딩

				DataAsyncCount += 1;
			};
	}

	#endregion

}
