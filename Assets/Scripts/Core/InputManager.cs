using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;
using System;
using UnityEngine.EventSystems;

/*
 * InputManager�� �Է��� ���� �޴� �͵� ������ �ش��ϴ� 
 * �������� Ű�� ������ ���� üũ�� �Ǿ��� ��� �Լ��� ȣ�� �� �� �ֵ��� ���� ���Դϴ�.
 * ���� �ȸ��� �ɲ������� �׳� GetKey �������� Ȯ���ϴ� �� ���� �ٽ� �����߽��ϴ�.
*/

[System.Serializable]
class KeyInfo 
{
	public UserKey key = UserKey.End;
	public List<KeyCode> listKey = new List<KeyCode>();
	public bool down = false;
	public bool press = false;
	public bool up = false;
}

// ���߿� Json���� ������ �뵵
class KeyInfoJson 
{
	[System.Serializable]
	public class KeyInfos {
		public UserKey key;
		public List<KeyCode> listKey = new List<KeyCode>();
	}

	public List<KeyInfos> keyInfos = new List<KeyInfos>();
}


public class InputManager
{
    private Dictionary<UserKey, KeyInfo> m_dicKeys = new Dictionary<UserKey, KeyInfo>();

	public Action KeyEvent = null;
	public Action<Define.Mouse> MouseEvent = null;

	private bool m_isPressed = false;
	private float m_pressTime = 0.5f;

	#region ������ ��ũ
	public void Init()
	{
		m_dicKeys.Clear();

		// �⺻���� ���� ���
		AddKey(UserKey.Forward, KeyCode.W);
		AddKey(UserKey.Forward, KeyCode.UpArrow);
		AddKey(UserKey.Backward, KeyCode.S);
		AddKey(UserKey.Backward, KeyCode.DownArrow);
		AddKey(UserKey.Right, KeyCode.D);
		AddKey(UserKey.Right, KeyCode.RightArrow);
		AddKey(UserKey.Left, KeyCode.A);
		AddKey(UserKey.Left, KeyCode.LeftArrow);
		AddKey(UserKey.Evasion, KeyCode.Space);
		AddKey(UserKey.Shoot, KeyCode.Mouse0);
	}

	public void Update()
	{
		// ��ȸ
		foreach (var pair in m_dicKeys) {
			KeyInfo info = pair.Value;

			// �ڵ尡 ���ȴ��� üũ�Ѵ�.
			bool check = GetKeyCode(info.listKey);

			// ��������
			if(check == true) {
				// ������ Ȯ�� �ȉ�����
				if(info.down == false && info.press == false) {
					info.down = true;
					info.press = true;
				}
				// ��� ������ �־�����
				else {
					info.down = false;
				}
			}
			// �ȴ�������
			else {
				// ���� Ű�� ��������
				if(info.down == true || info.press == true) {
					info.up = true;
					info.down = false;
					info.press = false;
				}
				// ���� Ű�� ������
				else if(info.up == true) {
					info.up = false;
				}
			}
		}


		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		if (Input.anyKey == true && KeyEvent != null) {
			KeyEvent.Invoke();
		}

		if (MouseEvent != null) {
			if (Input.GetMouseButton(0) == true) {
				if (m_isPressed == false) {
					MouseEvent.Invoke(Define.Mouse.PointerDown);
					m_pressTime = Time.time;
				}
				MouseEvent.Invoke(Define.Mouse.Press);
				m_isPressed = true;
			}
			else {
				if (m_isPressed == true) {
					if (Time.time < m_pressTime + 0.2f)
						MouseEvent.Invoke(Define.Mouse.Click);
					MouseEvent.Invoke(Define.Mouse.PointerUp);
				}
				m_isPressed = false;
				m_pressTime = 0;
			}
		}
	}

	public void Clear()
	{

	}

	public void Load()
	{
		// TODO : ���� �����
		// ������ ����

		KeyInfoJson json = new KeyInfoJson();
		foreach (var item in m_dicKeys)
		{
			json.keyInfos.Add(new KeyInfoJson.KeyInfos { key = item.Value.key, listKey = item.Value.listKey });
		}

		string strJson = JsonUtility.ToJson(json);
		Debug.Log(strJson);


		m_dicKeys.Clear();
	}

	public void RegisterKeyEvent(Action _keyEvent)
	{
		KeyEvent -= _keyEvent;
		KeyEvent += _keyEvent;
	}

	public void RegisterMouseEvent(Action<Define.Mouse> _mouseEvent)
	{
		MouseEvent -= _mouseEvent;
		MouseEvent += _mouseEvent;
	}

	#endregion

	#region ��� �Լ�
	//�߰�
	public KeyCode GetKeyData(UserKey p_key)
    {
		return m_dicKeys[p_key].listKey[0];
	}

	public void ChangeKey(UserKey _userKey, KeyCode _keyCode)
	{
		if (m_dicKeys.ContainsKey(_userKey) == true)
		{
			int l_size = m_dicKeys[_userKey].listKey.Count;
			List<KeyCode> l_keys = m_dicKeys[_userKey].listKey;
			l_keys[0] = _keyCode;
		}
	}

	public bool GetKey(UserKey p_key)
	{
		// ��ϵ� Ű�� ���� ���
		if(m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// Ŭ��������
		if(m_dicKeys[p_key].down == true || m_dicKeys[p_key].press == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyDown(UserKey _key)
	{
		// ��ϵ� Ű�� ���� ���
		if (m_dicKeys.ContainsKey(_key) == false) {
			return false;
		}

		// Ŭ��������
		if (m_dicKeys[_key].down == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyUp(UserKey _key)
	{
		// ��ϵ� Ű�� ���� ���
		if (m_dicKeys.ContainsKey(_key) == false) {
			return false;
		}

		// Ŭ��������
		if (m_dicKeys[_key].up == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyUpOrAll(UserKey _key1, UserKey _key2, UserKey _key3, UserKey _key4)
	{
		return GetKeyUp(_key1) || GetKeyUp(_key2) || GetKeyUp(_key3) || GetKeyUp(_key4);
	}

	private bool GetKeyCode(List<KeyCode> _keyCode)
	{
		foreach (KeyCode code in _keyCode) {
			if (Input.GetKey(code) == true) {
				return true;
			}
		}

		return false;
	}

	public void AddKey(UserKey _key, KeyCode _keycode)
	{
		// �ش��ϴ� Ű�� ��ü�� ���� ��� ����
		if(m_dicKeys.ContainsKey(_key) == false)  {
			m_dicKeys[_key] = new KeyInfo();
			m_dicKeys[_key].key = _key;
		}

		// �迭 ���� ���� ����
		KeyInfo l_info = m_dicKeys[_key];

		// ���� ����Ʈ�ȿ� �ڵ尡 ������ ���
		if(IsListKeyCode(l_info.listKey, _keycode) == true) {
			return;
		}

		// ���� ���
		l_info.listKey.Add(_keycode);
	}
	public void DelKey(UserKey _key, KeyCode _keycode = KeyCode.None)
	{
		// ����
		if (m_dicKeys.ContainsKey(_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo l_info = m_dicKeys[_key];

		// �ϴ� ����
		if (_keycode == KeyCode.None) {
			l_info.listKey.RemoveRange(0, l_info.listKey.Count);
		}
		// �����Ѱ� ������ ����
		else {
			l_info.listKey.Remove(_keycode);
		}
	}

	public void ChangeKey(UserKey _userKey, KeyCode _keyCode)
	{
		if(m_dicKeys.ContainsKey(_userKey) == true) {
			int l_size = m_dicKeys[_userKey].listKey.Count;
			List<KeyCode> l_keys = m_dicKeys[_userKey].listKey;
			l_keys[0] = _keyCode;
		}
	}

	private bool IsListKeyCode(List<KeyCode> _listKey, KeyCode _keyCode)
	{
		foreach (KeyCode key in _listKey) {
			if (key == _keyCode) {
				return true;
			}
		}

		return false;
	}

	#endregion
}
