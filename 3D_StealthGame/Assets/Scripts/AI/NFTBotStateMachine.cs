using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class NFTBotStateMachine : MonoBehaviour
{
    private StateMachine _brain;
    private MoveAgent _agentPatrol;
    private PlayerStateMachine _player;

    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField]
    private TMP_Text _stateNote;


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
        
        _player = FindObjectOfType<PlayerStateMachine>();
        _brain = GetComponent<StateMachine>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agentPatrol = GetComponent<MoveAgent>();
  
        _playerIsNear = false;
        _withinCatchRange = false;
        _playerDetected = false;

        _brain.PushState(Idle, OnIdleEnter, OnIdleExit);
    }

    
    void Update()
    {
        _playerIsNear = Vector3.Distance(transform.position, _player.transform.position) < 5;
        _withinCatchRange = Vector3.Distance(transform.position, _player.transform.position) < 1;

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


    // IDLE STATE
    void OnIdleEnter()
    {
        _agent.ResetPath();
    }
    void Idle()
    {
        _stateNote.text = "Idle";
        _changeMind -= Time.deltaTime;
        if (_changeMind <= 0)
        {
            _brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            _changeMind = Random.Range(4, 10);
        }

    }
    void OnIdleExit()
    {
    }


    // WANDER STATE
    void OnWanderEnter()
    {
        _stateNote.text = "Wander";
        _animator.SetBool("isWandering", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 4f) + transform.position;
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
