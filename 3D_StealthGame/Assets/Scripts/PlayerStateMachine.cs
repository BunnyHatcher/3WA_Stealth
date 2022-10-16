using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerState
{
    IDLE,
    WALKING,
    JOGGING,
    RUNNING,
    SNEAKING,
    JUMPING,
    FALLING,
    DODGING,

}

[RequireComponent(typeof(Rigidbody))]

public class PlayerStateMachine : MonoBehaviour
{
    //public and serialized
    public float _moveSpeed = 10f;
    public float _turnSpeed = 500f;
    public float _jumpForce = 5f;

    [Header("FloorDetection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float yFloorOffset = 1f;
    private FloorDetector _floorDetector;


    //privates and protected
    private PlayerState _currentState;
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private Animator _animator;
    private bool _isJumping = false;
    private bool _isGrounded = true;


    private void Awake() // usually used for getting components of the object the script is on
    {
        _rigidbody = GetComponent<Rigidbody>();
        _floorDetector = GetComponentInChildren<FloorDetector>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start() // usually used to get components in other objects
    {
        _cameraTransform = Camera.main.transform;
        TransitionToState(PlayerState.IDLE);
        
    }



    void Update()
    {
        OnStateUpdate();

        // Draw boxes that represents the ground checker
        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);

        _isGrounded = groundColliders.Length > 0;// if more than one ground collider touches the ground, isGrounded becomes true

        if (_isGrounded)
        {
            Debug.Log("Touchdown!");
        }


    }


    private void FixedUpdate()
    {

       if(_currentState == PlayerState.JUMPING || _currentState == PlayerState.FALLING)
        {
            _direction.y = _rigidbody.velocity.y;
        }

       else
        {
            StickToGround();
            Debug.Log("Is Sticking to ground");
        }

       if (_isJumping )
        {
            _direction.y = _jumpForce;
            _isJumping = false; // to prevent changing to isJumping every frame after the first one: we want to jump only once
        }
        
        RotateTowardsCamera();
        
        // move character by setting its velocity to the direction of movement calculated earlier
       _rigidbody.velocity = _direction;
        //Debug.Log(_direction.magnitude);

    }


    //---------------------------| S T A T E  M A C H I N E S |----------------------------------------------------------------------------------------------------------------------

    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALKING:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JUMPING:
                _isJumping = true;                
                break;
            case PlayerState.FALLING:
                break;
            case PlayerState.DODGING:
                break;
            default:
                break;

        }
    }

    private void OnStateUpdate()
    {
        switch (_currentState)
        {
        //-----I D L E ------------------------------------------------------------------------------------------------------------------------------------------------


            case PlayerState.IDLE:
                Move();

                if(_direction.magnitude > 0)
                {
                    TransitionToState(PlayerState.JOGGING);
                    _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                    _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                    _animator.SetFloat("moveSpeed", _direction.magnitude);
                }

                else if(Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                break;

        //------W A L K I N G---------------------------------------------------------------------------------------------------------------------------------------------------
        /*
            case PlayerState.WALKING:
                Move();

                if (_direction.magnitude > 0)
                {
                    TransitionToState(PlayerState.JOGGING);
                    _animator.SetFloat("MoveSpeedX", Input.GetAxis("Horizontal"));
                }

                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                break;
        */

        //-------J O G G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.JOGGING:
                Move();
                if (_direction.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                    _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                    _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                    _animator.SetFloat("moveSpeed", _direction.magnitude);
                }

                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                    
                }

                else if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }

                break;

        //-------R U N N IN G ----------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.RUNNING:
                Move();
                
               if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                else if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }
                
                break;
            
        //------S N E A K I N G --------------------------------------------------------------------------------------------------------------------------------------------
                
            case PlayerState.SNEAKING:
                Move();

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                else if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
                }

                break;

        //------J U M P I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.JUMPING:
                Move();          

                if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);                    
                }

                break;

        //------F A L L I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.FALLING:
                Move();

                if (_isGrounded)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;

       //------D O D G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.DODGING:
                break;

            default:
                break;

        }

    }

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALKING:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.FALLING:
                break;
            case PlayerState.DODGING:
                break;
            default:
                break;

        }

    }


    //---------------------------| S T A T E  M A C H I N E  M E T H O D S |----------------------------------------------------------------------------------------------------------------------

    private void TransitionToState(PlayerState ToState)
    {
        OnStateExit();
        _currentState = ToState;
        OnStateEnter();

    }

    //---------------------------| O T H E R  M E T H O D S | ----------------------------------------------------------------------------------------------------------------------

    private void Move()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical")   //forward/backward movement: relative to CAMERA
                      + _cameraTransform.right * Input.GetAxis("Horizontal");      //left-right movement: relative to PLAYER

        _direction *= _moveSpeed; // multiply by movement speed to get direction of movement

        _direction.y = 0; // Vertical transform is not taken into account, we have the Jumpmethod for that
    }

    private void StickToGround()
    {
        Vector3 averagePosition = _floorDetector.AverageHeight();

        Vector3 newPosition = new Vector3(_rigidbody.position.x, averagePosition.y + yFloorOffset, _rigidbody.position.z); // glues the character to the average position on the y-axis
        _rigidbody.MovePosition(newPosition);
        _direction.y = 0;
    }

    private void RotateTowardsCamera()
    {        
        Vector3 lookDirection = _cameraTransform.forward;   // make a copy of cameraTransform.forward...
        lookDirection.y = 0;    // ... to be able to set the y-axis of the camera to 0
        // --> otherwise player would lean forward when looking down and lean backward when looking up

        Quaternion rotation = Quaternion.LookRotation(lookDirection); // declare the look rotation according to cameraForward...
        rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
        _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2f); // by default OverlapBox only takes half the size of the box in each dimension, so we need to double the size of _boxDimension
    }



}
