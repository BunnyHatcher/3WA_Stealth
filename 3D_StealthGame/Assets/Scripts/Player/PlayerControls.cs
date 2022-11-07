using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    #region References
    private CharacterController _controller;
    private Transform _cameraTransform;
    private Vector3 _playerMovement;
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
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        _controller.Move(direction * _currentSpeed * Time.deltaTime);

        
    }


    

}
