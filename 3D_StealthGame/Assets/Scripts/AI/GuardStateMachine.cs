using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class GuardStateMachine : MonoBehaviour
{
    //-----REFERENCES------------------------------- 

    #region References

    private StateMachine _brain;
    private MoveAgent _agentPatrol;
    private PlayerStateMachine _player;

    private NavMeshAgent _agent;
    private Animator _animator;
    
    [SerializeField]
    private TMP_Text _stateNote;

    //Vision Cone
    private VisionCone _visionCone;
    

    //Wandering
    private float _changeMind;
    public float _changeMindMinRange = 4;
    public float _changeMindMaxRange = 10;

    //Attacking
    private float _attackTimer;


    //Chasing
    private bool _playerIsNear;
    private bool _withinCatchRange;

    // Suspicion
    float timeSinceLastSawPlayer = Mathf.Infinity;


    #endregion





    //-----INITIALIZATIONS------------------------------- 

    #region Awake & Start
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
        _visionCone._playerDetected = false;

        _brain.PushState(Patrol, OnPatrolEnter, OnPatrolExit);
    }

    #endregion

    
    
    #region Update & FixedUpdate

    void Update()
    {
        
    }

    #endregion


    
    
    
    
    //-----S T A T E S-------------------------------    


    #region PATROL

    // PATROL STATE

    void OnPatrolEnter()
    {
        _stateNote.text = "Patroling";
    }
    void Patrol()
    {
        
        if (_visionCone._target != null)
        {
            _brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }

        else
        { 
        _agentPatrol.Patrol();
        }

    }
    void OnPatrolExit()
    {
    }

    #endregion



    #region IDLE
    
    // IDLE STATE

   /*
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

    */

    #endregion



    #region WANDER

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

    #endregion



    #region CHASE

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
            _brain.PushState(Patrol, OnPatrolEnter, OnPatrolExit);
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

    #endregion



    #region ATTACK

    // ATTACK STATE
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

    #endregion



    #region SUSPICION

    // SUSPICION STATE
    void OnEnterSuspicion()
    {
        timeSinceLastSawPlayer = 0;
    }

    void Suspicion()
    {

    }

    void OnExitSuspicion()
    {

    }

    #endregion



    // METHODS

    private void DetectPlayer()
    {
        
    }





}
