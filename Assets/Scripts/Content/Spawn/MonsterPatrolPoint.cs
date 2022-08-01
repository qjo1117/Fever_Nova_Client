using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrolPoint : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
