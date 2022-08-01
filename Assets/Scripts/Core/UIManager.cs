using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------------------------------------
 *  사용할때 주의할점
 *  Addressable로 변환 후 풀링대상이 Resource폴더 밖으로 되어있으므로
 *  Scene초기화 작업을 진행했을때 풀 등록을 먼저 해준다.
------------------------------------------------------------------- */

public class UIManager {
    int m_order = 10;

    Stack<UI_Popup> m_stackPopup = new Stack<UI_Popup>();
    UI_Scene m_sceneUI = null;


    public GameObject Root 
    {
        get {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort) {
            canvas.sortingOrder = m_order;
            m_order += 1;
        }
        else {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) {
            name = typeof(T).Name;
        }


        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null) {
            go.transform.SetParent(parent);
        }

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name)) {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name)) {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        m_sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) {
            name = typeof(T).Name;
        }

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        m_stackPopup.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    // 몬스터 사망시 체력바 ui가 사라져야하므로 Close함수 필요하다고 생각함
    public void CloseSceneUI(UI_Scene scene)
    {
        Managers.Resource.Destroy(scene.gameObject);

        if (m_sceneUI == scene)
        {
            m_sceneUI = null;
        }
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (m_stackPopup.Count == 0) {
            return;
        }

        if (m_stackPopup.Peek() != popup) {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (m_stackPopup.Count == 0) {
            return;
        }

        UI_Popup popup = m_stackPopup.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        m_order--;
    }

    public void CloseAllPopupUI()
    {
        while (m_stackPopup.Count > 0) {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        m_sceneUI = null;
    }
}

