using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    Vector3 m_delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    GameObject m_player = null;

    [SerializeField]
    private float m_anlge = 0.0f;

    [SerializeField]
    private Vector3 m_direction = Vector3.right;

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
        Vector3 l_delta = Quaternion.AngleAxis(m_anlge, m_direction) * m_delta;
        if (Physics.Raycast(m_player.transform.position, l_delta, out hit, m_delta.magnitude, 1 << (int)Define.Layer.Block)) {
            float dist = (hit.point - m_player.transform.position).magnitude * 0.8f;
            transform.position = m_player.transform.position + m_delta.normalized * dist;
        }
        else {
            transform.position = m_player.transform.position + m_delta;
            transform.rotation = Quaternion.LookRotation(m_direction);
        }
    }

    public void SetQuarterView(Vector3 p_delta)
    {
        m_delta = p_delta;
    }
}
