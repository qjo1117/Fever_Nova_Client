using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NetWorkManager
{
    private Session session;
    private Dictionary<int, Action> m_dicProtocol = new Dictionary<int, Action>();
    private int m_clientId = -1;

    public Dictionary<int, Action> ProtocolAction { get => m_dicProtocol; }
    public Session Session { get => session; }
    public int ClientId { get => m_clientId; }


    public void Init()
    {
        session = new Session();
        session.Initialize();
        Register();
    }

    public void Register()
	{
        m_dicProtocol.Add((int)E_PROTOCOL.CRYPTOKEY, KeyProcess);
        m_dicProtocol.Add((int)E_PROTOCOL.STC_IDCREATE, IdProcess);

    }

    public void Register(E_PROTOCOL _protocol, Action _action)
	{
        if (m_dicProtocol.ContainsKey((int)_protocol) == false) {
            m_dicProtocol.Add((int)_protocol, _action);
		}
    }

    private void KeyProcess()
	{
		session.CryptoKeyDataSetting();
		session.Write((int)E_PROTOCOL.CTS_IDCREATE); // 立加
	}

    private void IdProcess()
	{
        int l_id = -1;
        session.GetData(out l_id);
        m_clientId = l_id;

        session.Write((int)E_PROTOCOL.CTS_SPAWN); // 胶迄夸没
    }


    public GameObject playerUnit;
    public List<GameObject> players = new List<GameObject>();

    public float x = 0;
    public float y = 0;

    public void End()
	{
        //MoveTest.GetInstance().CloseSocket();
        if (session.CheckConnecting())
        {
            session.Write((int)E_PROTOCOL.CTS_EXIT);// 辆丰
            session.TreadEnd();
            session.CloseSocket();
        }

    }

    public void Update()
    {
        //bool l_isMove = false;
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    x -= Time.deltaTime * 1;
        //    l_isMove = true;
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    x += Time.deltaTime * 1;
        //    l_isMove = true;
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    y -= Time.deltaTime * 1;
        //    l_isMove = true;
        //}
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    y += Time.deltaTime * 1;
        //    l_isMove = true;
        //}

        //if (l_isMove)
        //{
        //    session.Write((int)E_PROTOCOL.MOVE, x, y);
        //}


        if (session.CheckRead())
        {
            if(m_dicProtocol.ContainsKey(session.GetProtocol()) == true) {
                m_dicProtocol[session.GetProtocol()].Invoke();
            }


			//switch (session.GetProtocol())
			//{
			//	case (int)E_PROTOCOL.CRYPTOKEY:
			//		{
			//			session.CryptoKeyDataSetting();
			//			session.Write((int)E_PROTOCOL.SPAWN); // 立加
			//		}
			//		break;
			//	case (int)E_PROTOCOL.INUSER:
			//		{
			//			int lid;
			//			session.GetInData(out lid);
			//			GameObject temp = GameObject.Instantiate(playerUnit);
			//			temp.GetComponent<Player>().id = lid;
			//			players.Add(temp);
			//		}
			//		break;
			//	case (int)E_PROTOCOL.MOVEUSER:
			//		{
			//			int lid;
			//			float lx;
			//			float ly;
			//			session.GetMoveData(out lid, out lx, out ly);
			//			foreach (GameObject obj in players)
			//			{
			//				if (obj.GetComponent<Player>().id == lid)
			//				{
			//					obj.GetComponent<Player>().x = lx;
			//					obj.GetComponent<Player>().y = ly;
			//				}
			//			}
			//		}
			//		break;
			//	case (int)E_PROTOCOL.OUTUSER:
			//		{
			//			int lid;
			//			session.GetOutData(out lid);
			//			foreach (GameObject obj in players)
			//			{
			//				if (obj.GetComponent<Player>().id == lid)
			//				{
			//					Destroy(obj);
			//					players.Remove(obj);
			//				}
			//			}
			//		}
			//		break;
			//}
		}
    }



    void OnApplicationQuit()
    {

    }
}
