using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveAgent : MonoBehaviour
{
    private StateMachine _brain;
    private Animator _animator;
    private Text _stateNote;

    private NavMeshAgent _agent;    
    public Transform m_target;
    //private PlayerStateMachine _playerStateMachine;

    //Patrol Behaviour
    public Transform[] _points;
    private int _destPoint = 0;

    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        _brain = GetComponent<StateMachine>();
        //_playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;

        GotoNextPoint();

    }


    private void Update()
    {
        // _agent.SetDestination(m_target.position);
        
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            GotoNextPoint();



    }
    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (_points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _agent.destination = _points[_destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        _destPoint = (_destPoint + 1) % _points.Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        for (int i = 0; i < _points.Length - 1; i++)
        {
            if (i == _points.Length) // at the last waypoint
            {
              Gizmos.DrawLine(_points[i].position, _points[0].position);
             
            }

            else
            {
              Gizmos.DrawLine(_points[i].position, _points[i + 1].position);
            }

        }
    }











}
