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

    //Wandering
    private float _changeMind;
    public float _changeMindMinRange = 4;
    public float _changeMindMaxRange = 10;

    //Vision Cone
    private VisionCone _visionCone;

    // bools
    private bool _playerIsNear;
    private bool _withinCatchRange;
    private bool _playerDetected;

    //Attacking
    private float _attackTimer;


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
        _stateNote.text = "Idle";
        _agent.ResetPath();
    }
    void Idle()
    {
        _changeMind -= Time.deltaTime;
        if (_playerIsNear)
        {
            _brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
        else if (_changeMind <= 0)
        {
            _brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            _changeMind = Random.Range(_changeMindMinRange, _changeMindMaxRange);
        }

    }
    void OnIdleExit()
    {
    }

    // CHASE STATE
    void OnChaseEnter()
    {
        _stateNote.text = "Chase";
        _animator.SetBool("Chase", true);
    }

    void Chase()
    {
        _agent.SetDestination(_player.transform.position);
        if (Vector3.Distance(transform.position, _player.transform.position) > 5.5f)
        {
            _brain.PopState();
            _brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }

        if (_withinCatchRange)
        {
            _brain.PushState(Attack, OnEnterAttack, null);
        }
    }

    void OnChaseExit()
    {
        _animator.SetBool("Chase", false);
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
        
        if (_agent.remainingDistance <= .25f) // distance left before reaching the point they were looking for
        {
            _agent.ResetPath();
            _brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }

        if (_playerIsNear)
        {
            _brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }

    void OnWanderExit()
    {
        _animator.SetBool("isWandering", false);
    }


    // ATTACK
    void OnEnterAttack()
    {
        _agent.ResetPath();
        _stateNote.text = "Attack";
    }
    void Attack()
    {
            _attackTimer -= Time.deltaTime;
            if (!_withinCatchRange)
            {
                _brain.PopState();
            }
            else if (_attackTimer <= 0)
            {
                _animator.SetTrigger("Attack");
                
                _attackTimer = 2f;
            }
     
    }












}
