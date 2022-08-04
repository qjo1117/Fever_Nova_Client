using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
	private bool m_isLoading = false;

	private void Awake()
	{
        gameObject.SetActive(true);
		Begin();
	}

	public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

	protected virtual void Begin()
	{
		GameObject obj = GameObject.FindObjectOfType<EventSystem>()?.gameObject;
		if(obj == null) {
			obj = Managers.Resource.NewPrefab("EventSystem");
			obj.name = "@EventSystem";
		}

		LoadGameObject();

		StartCoroutine("Loading");
	}

	protected virtual void LoadGameObject() { }
	protected virtual void Init() {  }
	protected virtual void OnUpdate() {  }

	private void Update()
	{
		if(m_isLoading == false) {
			return;
		}

		OnUpdate();
	}



	IEnumerator Loading()
	{
		GameObject loading = Managers.Resource.NewPrefab("Loading");
		while (true) {
			// 리소스가 다 로딩되었으면 끝
			if (Managers.Resource.DataMaxAsyncCount <= Managers.Resource.DataAsyncCount) {
				Managers.Resource.DelPrefab(loading);
				Init();
				m_isLoading = true;
				yield break;
			}
			// 리소스가 로딩중이면
			else {
				yield return null;
			}
		}

	}

	public abstract void Clear();
}
