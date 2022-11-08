using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    #region References

    // Camera
    public Transform _cameraTransform;

    // Rotation
    public float _turnSpeed { get; private set; }
    

    //Movement
    public CharacterController _controller;
    public Vector3 _movementDirection;
    public float _currentSpeed { get; private set; }


    [Header("Movement Speeds")]
    public float _moveSpeed = 10f;
    

    #endregion


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
    }

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for movement direction
        _movementDirection = _cameraTransform.forward * Input.GetAxisRaw("Vertical")
                            + _cameraTransform.right * Input.GetAxisRaw("Horizontal");


        _movementDirection.y = 0f;

        // Move into calculated direction
        _controller.Move(_movementDirection.normalized * _currentSpeed * Time.deltaTime);

        //Rotation
        Vector3 lookDirection = _cameraTransform.forward;
        lookDirection.y = 0;
        

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection); 
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _turnSpeed * Time.fixedDeltaTime); 
        
    }
   
















}
