using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GuardStateMachine : MonoBehaviour
{
    private StateMachine _brain;
    private NavMeshAgent _agent;
    private PlayerStateMachine _player;
    private Animator _animator;
    private Text _stateNote;
    
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
}
