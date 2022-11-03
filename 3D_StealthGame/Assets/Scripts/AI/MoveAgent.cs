using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveAgent : MonoBehaviour
{
    private StateMachine _brain;
    //private Animator _animator;
    private Text _stateNote;

    private NavMeshAgent _agent;    
    public Transform m_target;
    //private PlayerStateMachine _playerStateMachine;

    //Patrol Behaviour
    public Transform[] _points;
    private int _destPoint = 0;
    private bool _goingForward =  true;
    [SerializeField] private bool _backAndForth = false;

    //Vision Cone
    private VisionCone _visionCone;


    private void Awake()
    {
        //_animator = transform.GetChild(0).GetComponent<Animator>();
        _visionCone = GetComponentInChildren<VisionCone>();
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
        PatrolMovement();

    }

    public void PatrolMovement()
    {
        if (_backAndForth)
        {
            if (_goingForward)
            { GotoNextPoint(); }

            else
            { GoToPreviousPoint(); }
        }

        else
        {

            GotoNextPoint();

        }
    }

    private void GotoNextPoint()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)

        
        { // Returns if no points have been set up
            if (_points.Length == 0)
                return;

            _destPoint++;

            if(_destPoint >= _points.Length)
            {
                if(_backAndForth)
                {
                    _goingForward = false;
                    _destPoint = _points.Length - 1;
                }

                else
                {
                    _destPoint = 0;
                }
            }

            
            
            // Set the agent to go to the currently selected destination.
            _agent.destination = _points[_destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            //_destPoint = (_destPoint + 1) % _points.Length;
        }
    }

    private void GoToPreviousPoint()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _destPoint--;

            if(_destPoint < 0)
            {
               if(_backAndForth)
                {
                    _goingForward = true;
                    _destPoint = 0;
                }
                
                else
                {
                 _destPoint = _points.Length - 1;
                }
            }

            _agent.destination = _points[_destPoint].position;

        }


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
