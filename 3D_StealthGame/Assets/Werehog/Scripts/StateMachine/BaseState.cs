using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public abstract class BaseState : StateMachineBehaviour
{
    #region References
    //private StateMachine _brain;
    protected GameObject _player;
    
    protected GameObject _enemy;
    protected MoveAgent _agentPatrol;
    protected NavMeshAgent _agent;
    protected AnimateAgent _enemyAnimations;
    protected Animator _animator;
    protected Animator _FSM;

    protected TMP_Text _stateNote;

    //Vision Cone
    protected VisionCone _visionCone;
    #endregion

    #region Bools & Parameters
    
    protected bool _isPerformingAction = false;
    protected float _distanceFromTarget;
    
    // Chasing
    protected float _endChaseDistance = 5.5f;
    
    // Suspicion
    public float _timeSinceLastSawPlayer = Mathf.Infinity;
    protected float _suspicionTime = 3f;

    // Attack
    protected float _attackRange;
    #endregion



    #region Awake & Start

    private void Awake()
    {
        //_player = GameObject.Find("Player");
        _player = GameObject.FindWithTag("Player");
        _enemy = GameObject.Find("Werehog");

        // A.I.
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _agentPatrol = _enemy.GetComponent<MoveAgent>();
        // Animation
        _animator = _enemy.GetComponent<Animator>();
        _enemyAnimations = _enemy.GetComponent<AnimateAgent>();
        // State Machine
        _FSM = GameObject.Find("WerehogStateMachine").GetComponent<Animator>();
        _stateNote = _enemy.GetComponentInChildren<TMP_Text>();

        _visionCone = _enemy.GetComponentInChildren<VisionCone>();
    }

    private void Start()
    {
               

    }

    #endregion



    #region Methods

    


    
    
    
   

    #endregion

}
