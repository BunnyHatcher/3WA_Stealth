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
    protected MoveAgent _agentPatrol;
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Animator _FSM;

    protected TMP_Text _stateNote;
    protected GameObject _player;

    //Vision Cone
    protected VisionCone _visionCone;
    #endregion



    #region Awake & Start

    private void Awake()
    {
        _player = GameObject.Find("Werehog");

        _visionCone = _player.GetComponentInChildren<VisionCone>();
        _stateNote = _player.GetComponentInChildren<TMP_Text>();

        _agent = _player.GetComponent<NavMeshAgent>();
        _agentPatrol = _player.GetComponent<MoveAgent>();
        
        _animator = _player.GetComponent<Animator>();
        _FSM = _player.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
               

    }

    #endregion



    #region Methods

    


    
    
    
    protected void Move()
    {
        // Get input for movement direction
        _movementDirection = _cameraTransform.forward * Input.GetAxisRaw("Vertical")
                            + _cameraTransform.right * Input.GetAxisRaw("Horizontal");


        _movementDirection.y = 0f;

        // Move into calculated direction
        _controller.Move(_movementDirection.normalized * _playerControlSettings._currentSpeed * Time.deltaTime);

        _animatorHandler._moveSpeed = (_movementDirection.normalized * _playerControlSettings._currentSpeed
                                        * Time.deltaTime).magnitude;

        Debug.Log("Move Speed: " + _animatorHandler._moveSpeed);

    }

    protected void RotateTowardsCamera()
    {
        //Rotation
        Vector3 lookDirection = _cameraTransform.forward;
        lookDirection.y = 0;


        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        _playerTransform.rotation = Quaternion.Lerp(_playerTransform.rotation, lookRotation, _playerControlSettings._turnSpeed * Time.deltaTime);
    }

    #endregion

}
