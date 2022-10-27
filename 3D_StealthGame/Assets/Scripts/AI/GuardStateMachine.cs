using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GuardStateMachine : MonoBehaviour
{
    private StateMachine _brain;
    private MoveAgent _agentPatrol;
    private PlayerStateMachine _player;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Text _stateNote;


    private float _changeMind;
    
    //Vision Cone
    private VisionCone _visionCone;

    // bools
    private bool _playerIsNear;
    private bool _withinCatchRange;
    private bool _playerDetected;


    private void Awake()
    {
        _visionCone = GetComponentInChildren<VisionCone>();
    }

    void Start()
    {
        _player = GetComponent<PlayerStateMachine>();
        _brain = GetComponent<StateMachine>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerStateMachine>();
        _agentPatrol = GetComponent<MoveAgent>();
        _agent = GetComponent<NavMeshAgent>();
  
        _playerIsNear = false;
        _withinCatchRange = false;
        _playerDetected = false;

        _brain.PushState(Patrol, OnPatrolEnter, OnPatrolExit);
    }

    
    void Update()
    {
        
    }

    



    // PATROL STATE
    void OnPatrolEnter()
    {
    }
    void Patrol()
    {
        _stateNote.text = "Patroling";

    }
    void OnPatrolExit()
    {
    }


    // WANDER STATE
    void OnWanderEnter()
    {
        _animator.SetBool("isWandering", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 1.2f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 3f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        _agent.SetDestination(destination);
    }
    void Wander()
    {
        _stateNote.text = "Wandering";
    }

    void OnWanderExit()
    {
        _animator.SetBool("isWandering", false);
    }
}
