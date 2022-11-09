using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum RedHoodState
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

public class RedHoodStateMachine : MonoBehaviour
{

    //-----REFERENCES------------------------------- 

    #region Classes & Components
    private StateMachine _brain;
    private RedHoodStateMachine _player;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    protected PlayerControlSettings _param;
    #endregion


    #region Floor Detection Refs
    [Header("FloorDetection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float yFloorOffset = 1f;
    private FloorDetector _floorDetector;
    #endregion

    #region Dodging Refs
    [Header("Dodging")]    
    private float _dodgeDuration;
    private float _dodgeLength;
    private float remainingDodgeTime;
    private Vector3 dodgingDirectionInput;
    public bool _isDodging = false;
    #endregion


    //privates and protected    
    private RedHoodState _currentState;
    private float _currentSpeed;
    private Vector3 _direction = new Vector3();
    private bool _isJumping = false;
    private bool _isGrounded = true;
    private bool _isSneaking = false;

    
    private float deltaTime;

    #region Awake & Start
    private void Awake()
    {
        // Bring Scriptable Objects into CharacterStateBase Script
        string GUID = AssetDatabase.FindAssets("PlayerControlValues")[0]; // Find Scriptable Object asset
        string path = AssetDatabase.GUIDToAssetPath(GUID);
        _param = (PlayerControlSettings)AssetDatabase.LoadAssetAtPath(path, typeof(PlayerControlSettings));

        _rigidbody = GetComponent<Rigidbody>();
        _floorDetector = GetComponentInChildren<FloorDetector>();
        _animator = GetComponentInChildren<Animator>();        

        //Set Speed at Start to Jogging Speed
        _currentSpeed = _param._joggingSpeed;
    }

    private void Start()
    {
        //Set Camera position to Main Camera
        _cameraTransform = Camera.main.transform;

        // Start game in Idle State
        TransitionToState(RedHoodState.IDLE);

    }
    #endregion



    void Update()
    {
        #region GroundCheck
        // Draw boxes that represents the ground checker
        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);

        _isGrounded = groundColliders.Length > 0;// if more than one ground collider touches the ground, isGrounded becomes true

        if (_isGrounded)
        {
            //Debug.Log("Touchdown!");
        }
        #endregion
    }


    private void FixedUpdate()
    {

       if(_currentState == RedHoodState.JUMPING || _currentState == RedHoodState.FALLING)
        {
            _direction.y = _rigidbody.velocity.y;
        }

       else
        {
            StickToGround();
            //Debug.Log("Is Sticking to ground");
        }

       if (_isJumping )
        {
            _direction.y = _param._jumpForce;
            _isJumping = false; // to prevent changing to isJumping every frame after the first one: we want to jump only once
        }       
        
        RotateTowardsCamera();
        
        // move character by setting its velocity to the direction of movement calculated earlier
       _rigidbody.velocity = _direction;
       
        // Debug.Log("Speed is: " + _direction.magnitude);

    }


    //---------------------------| S T A T E  M A C H I N E S |----------------------------------------------------------------------------------------------------------------------


    private void OnStateEnter()
    {
        switch (_currentState)
        {
            case RedHoodState.IDLE:
                break;
            case RedHoodState.WALKING:
                break;
            case RedHoodState.JOGGING:
                _currentSpeed = _param._joggingSpeed;
                break;
            case RedHoodState.RUNNING:
                _currentSpeed = _param._runningSpeed;
                break;
            case RedHoodState.SNEAKING:
                _isSneaking = true;
                _currentSpeed = _param._sneakingSpeed;
                _animator.SetBool("isSneaking", true);
                break;
            case RedHoodState.JUMPING:
                _isJumping = true;
                break;
            case RedHoodState.FALLING:
                break;
            case RedHoodState.DODGING:
                _animator.SetBool("isDodging", true);
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


            case RedHoodState.IDLE:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (_direction.magnitude > 0)
                {
                    if (Input.GetButton("Run"))
                    {
                        TransitionToState(RedHoodState.RUNNING);
                    }
                    else
                    {
                        TransitionToState(RedHoodState.JOGGING);
                    }
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == false)
                {
                    TransitionToState(RedHoodState.SNEAKING);
                }



                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(RedHoodState.JUMPING);
                }

                else if (Input.GetButtonDown("Dodge"))
                {
                    TransitionToState(RedHoodState.DODGING);
                }

                break;

            //------W A L K I N G---------------------------------------------------------------------------------------------------------------------------------------------------
            /*
                case RedHoodState.WALKING:
                    Move();

                    if (_direction.magnitude > 0)
                    {
                        TransitionToState(RedHoodState.JOGGING);
                        _animator.SetFloat("MoveSpeedX", Input.GetAxis("Horizontal"));
                    }

                    else if (Input.GetButtonDown("Jump"))
                    {
                        TransitionToState(RedHoodState.JUMPING);
                    }
                    break;
            */

            //-------J O G G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.JOGGING:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (_direction.magnitude == 0)
                {
                    TransitionToState(RedHoodState.IDLE);

                }

                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(RedHoodState.JUMPING);
                }

                else if (!_isGrounded)
                {
                    TransitionToState(RedHoodState.FALLING);
                }

                else if (Input.GetButton("Run"))
                {
                    TransitionToState(RedHoodState.RUNNING);
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == false)
                {
                    TransitionToState(RedHoodState.SNEAKING);
                }

                else if (Input.GetButtonDown("Dodge"))
                {
                    Debug.Log("Transition to Dodge");
                    TransitionToState(RedHoodState.DODGING);
                }

                break;

            //-------R U N N I N G ----------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.RUNNING:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                //Debug.Log("Running");

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(RedHoodState.JUMPING);
                }

                else if (!_isGrounded)
                {
                    TransitionToState(RedHoodState.FALLING);
                }

                else if (!Input.GetButton("Run"))
                {
                    TransitionToState(RedHoodState.JOGGING);
                }

                else if (Input.GetButtonDown("Dodge"))
                {

                    TransitionToState(RedHoodState.DODGING);
                }

                break;

            //------S N E A K I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.SNEAKING:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(RedHoodState.JUMPING);
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == true)
                {
                    TransitionToState(RedHoodState.JOGGING);
                }

                else if (!_isGrounded)
                {
                    TransitionToState(RedHoodState.FALLING);
                }

                break;

            //------J U M P I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.JUMPING:
                Move();
                _animator.SetBool("isJumping", true);
                _animator.SetBool("isGrounded", false);

                if (_rigidbody.velocity.y < -0.2f && !_isGrounded)
                {
                    TransitionToState(RedHoodState.FALLING);

                }

                break;

            //------F A L L I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.FALLING:
                Move();
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", false);

                if (_isGrounded)
                {
                    TransitionToState(RedHoodState.IDLE);
                    _animator.SetBool("isGrounded", true);

                }

                break;

            //------D O D G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case RedHoodState.DODGING:

                Dodge();

                break;

            default:
                break;

        }

    }

    

    private void OnStateExit()
    {
        switch (_currentState)
        {
            case RedHoodState.IDLE:
                break;
            case RedHoodState.WALKING:
                break;
            case RedHoodState.JOGGING:
                break;
            case RedHoodState.RUNNING:
                break;
            case RedHoodState.SNEAKING:
                _isSneaking = false;
                _animator.SetBool("isSneaking", false);
                break;
            case RedHoodState.JUMPING:
                break;
            case RedHoodState.FALLING:
                break;
            case RedHoodState.DODGING:
                _isDodging = false;
                _animator.SetBool("isDodging", false);
                break;
            default:
                break;

        }

    }



    //---------------------------| O T H E R  M E T H O D S | ----------------------------------------------------------------------------------------------------------------------

private void Move()
{
    _direction = _cameraTransform.forward.normalized * Input.GetAxis("Vertical")   //forward/backward movement: relative to CAMERA
                  + _cameraTransform.right.normalized * Input.GetAxisRaw("Horizontal");//left-right movement: relative to PLAYER
    _direction *= _currentSpeed; // multiply by movement speed to get direction of movement
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
    rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _param._turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
    _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate
}

private void Dodge()
{
   
}

private void OnDrawGizmos()
{
    Gizmos.color = Color.magenta;
    Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2f); // by default OverlapBox only takes half the size of the box in each dimension, so we need to double the size of _boxDimension
}




public void TransitionToState(RedHoodState ToState)
{
    OnStateExit();
    _currentState = ToState;
    OnStateEnter();

}






























}
