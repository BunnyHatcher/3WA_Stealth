using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class CharacterStateBase : StateMachineBehaviour
{
    #region References

    //References necessary for character movements

    protected PlayerControlSettings _playerControlSettings;
    protected CharacterController _controller;
    public Vector3 _movementDirection;
    protected Transform _cameraTransform;
    protected Transform _playerTransform;
    protected GameObject _playerGameObject;
    protected AnimatorParametersUpdater _animatorHandler;

    //References for State Change
    private PlayerState _currentState;

    #endregion



    #region Awake & Start

    private void Awake()
    {
        // Bring Scriptable Objects into CharacterStateBase Script
        string GUID = AssetDatabase.FindAssets("PlayerControlValues")[0]; // Find Scriptable Object asset
        string path = AssetDatabase.GUIDToAssetPath(GUID);
        _playerControlSettings = (PlayerControlSettings)AssetDatabase.LoadAssetAtPath(path, typeof(PlayerControlSettings));
        
        //Debug Log
        Debug.Log(_playerControlSettings._currentSpeed);

        //Get all necessary references

        _playerGameObject = GameObject.Find("Erika Archer");
        _animatorHandler = _playerGameObject.GetComponent<AnimatorParametersUpdater>();
        _playerTransform = _playerGameObject.transform;
        _controller = _playerGameObject.GetComponent<CharacterController>();

        //Set up Camera
        _cameraTransform = Camera.main.transform;

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
