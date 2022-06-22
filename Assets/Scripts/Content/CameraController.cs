using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Vector3 m_delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    GameObject m_player = null;

    public void SetPlayer(GameObject player) { m_player = player; }

    void Start()
    {
        
    }

    void LateUpdate()
    { 
         if (m_player == null) {
             return;
         }

         RaycastHit hit;
         if (Physics.Raycast(m_player.transform.position, m_delta, out hit, m_delta.magnitude, 1 << (int)Define.Layer.Block)) {
             float dist = (hit.point - m_player.transform.position).magnitude * 0.8f;
             transform.position = m_player.transform.position + m_delta.normalized * dist;
         }
         else {
            transform.position = Vector3.Lerp(transform.position, m_player.transform.position + m_delta, 0.7f);
            //transform.LookAt(m_player.transform);
		 }
    }

    public void SetQuarterView(Vector3 p_delta)
    {
        m_delta = p_delta;
    }
}
