using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    private Spawner m_spawner = null;
    private bool m_isSpawn = false;         // 스폰여부

    public void SetSpawner(Spawner _spawner)
	{
        m_spawner = _spawner;
	}

    void Start()
    {
        
    }

    void Update()
    {
        
    }


	private void OnTriggerEnter(Collider other)
	{
        if(m_isSpawn == true) {
            return;
		} 
		if(other.gameObject.layer != (int)Define.Layer.Player) {
            return;
		}

        if(m_spawner != null) {
            m_spawner.StartSpawn();
            m_isSpawn = true;
        }
	}
}
