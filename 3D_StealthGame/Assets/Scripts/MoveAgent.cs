using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public Transform m_target;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        _agent.SetDestination(m_target.position);
    }
}
