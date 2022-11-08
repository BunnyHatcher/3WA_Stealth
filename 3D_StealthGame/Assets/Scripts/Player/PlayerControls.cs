using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    #region References
    
    // Camera
    private Transform _cameraTransform;

    // Rotation
    public float RotationDamping;

    //Movement
    private CharacterController _controller;
    private Vector3 _movementDirection;
    private float _currentSpeed = 6f;


    [Header("Movement Speeds")]
    public float _moveSpeed = 10f;
    public float _turnSpeed = 500f;
    public float _jumpForce = 5f;
    [SerializeField] private float _walkingSpeed = 1f;
    [SerializeField] private float _joggingSpeed = 5f;
    [SerializeField] private float _runningSpeed = 10f;
    [SerializeField] private float _sneakingSpeed = 2f;

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
        
        
        
        
        
       /* 
        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), deltaTime * _turnSpeed);
        }
       */

















}
