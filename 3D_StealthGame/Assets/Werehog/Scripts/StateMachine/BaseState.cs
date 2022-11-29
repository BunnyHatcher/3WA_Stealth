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
    protected Animator _animator;
    protected Animator _FSM;

    protected TMP_Text _stateNote;

    //Vision Cone
    protected VisionCone _visionCone;
    #endregion

    #region Bools & Parameters
    protected bool _withinAttackRange;

    protected float _endChaseDistance = 5.5f;
    public float _timeSinceLastSawPlayer = Mathf.Infinity;

    #endregion



    #region Awake & Start

    private void Awake()
    {
        //_player = GameObject.Find("Player");
        _player = GameObject.FindWithTag("Player");

        _enemy = GameObject.Find("Werehog");

        _visionCone = _enemy.GetComponentInChildren<VisionCone>();
        _stateNote = _enemy.GetComponentInChildren<TMP_Text>();

        _agent = _enemy.GetComponent<NavMeshAgent>();
        _agentPatrol = _enemy.GetComponent<MoveAgent>();
        
        _animator = _enemy.GetComponent<Animator>();
        _FSM = _enemy.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
               

    }

    #endregion



    #region Methods

    


    
    
    
   

    #endregion

}
