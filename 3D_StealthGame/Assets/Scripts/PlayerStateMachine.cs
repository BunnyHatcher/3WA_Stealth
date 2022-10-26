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
    [Header("Movement Speeds")]
    public float _moveSpeed = 10f;
    public float _turnSpeed = 500f;
    public float _jumpForce = 5f;
    [SerializeField] private float _walkingSpeed = 1f;
    [SerializeField] private float _joggingSpeed = 5f;
    [SerializeField] private float _runningSpeed = 10f;
    [SerializeField] private float _sneakingSpeed = 2f;

    [Header("Smoothing Values")]
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private float _speedSmoothDampVelocity = 0.0f;
    [SerializeField] private float _speedSmoothTime = 0.3f;


    [Header("FloorDetection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float yFloorOffset = 1f;
    private FloorDetector _floorDetector;

    [Header("Dodging")]
    /*
    private float _dodgeDuration;
    private float _dodgeLength;
    private float remainingDodgeTime;
    private Vector3 dodgingDirectionInput;
    private bool _isDodging = false;
    */

    [SerializeField] AnimationCurve dodgeCurve;
    bool _isDodging;
    float _dodgeTimer;


    //privates and protected
    private PlayerState _currentState;
    private float _currentSpeed;
    private Vector3 _direction = new Vector3();
    private bool _isJumping = false;
    private bool _isGrounded = true;
    private bool _isSneaking = false;

    // References
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private Animator _animator;
    private float deltaTime;

    private void Awake() // usually used for getting components of the object the script is on
    {
        _rigidbody = GetComponent<Rigidbody>();
        _floorDetector = GetComponentInChildren<FloorDetector>();
        _animator = GetComponentInChildren<Animator>();
        

        _currentSpeed = _joggingSpeed;
    }

    private void Start() // usually used to get components in other objects
    {
        _cameraTransform = Camera.main.transform;
        TransitionToState(PlayerState.IDLE);
       
        //Dodging
        /*
        Keyframe _lastDodgeFrame = dodgeCurve[dodgeCurve.length - 1];// Get points of Dodge Curve
        _dodgeTimer = _lastDodgeFrame.time;// set dodge timer to time passed since last dodge frame
        */
        
    }



    void Update()
    {
        OnStateUpdate();

        // Draw boxes that represents the ground checker
        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);

        _isGrounded = groundColliders.Length > 0;// if more than one ground collider touches the ground, isGrounded becomes true

        if (_isGrounded)
        {
            //Debug.Log("Touchdown!");

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
            //Debug.Log("Is Sticking to ground");
        }

       if (_isJumping )
        {
            _direction.y = _jumpForce;
            _isJumping = false; // to prevent changing to isJumping every frame after the first one: we want to jump only once
        }

       /*
        if (_isDodging)
        {
            _isDodging = false;
        }
       */
        
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
            case PlayerState.IDLE:
                break;
            case PlayerState.WALKING:
                break;
            case PlayerState.JOGGING:
                _currentSpeed = _joggingSpeed;
                break;
            case PlayerState.RUNNING:
                _currentSpeed = _runningSpeed;
                break;
            case PlayerState.SNEAKING:
                _isSneaking = true;
                _currentSpeed = _sneakingSpeed;
                _animator.SetBool("isSneaking", true);
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
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (_direction.magnitude > 0)
                {
                    if (Input.GetButton("Run"))
                    {
                        TransitionToState(PlayerState.RUNNING);
                    }
                    else
                    {
                        TransitionToState(PlayerState.JOGGING);
                    }
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == false)
                {
                    TransitionToState(PlayerState.SNEAKING);
                }

                

                else if(Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);                    
                }

                else if (Input.GetButtonDown("Dodge"))
                {
                    TransitionToState(PlayerState.DODGING);
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
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (_direction.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
                    
                }

                else if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);                    
                }

                else if (!_isGrounded)
                {
                    TransitionToState(PlayerState.FALLING);
                }

                else if (Input.GetButton("Run"))
                {
                    TransitionToState(PlayerState.RUNNING);                    
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == false)
                {
                    TransitionToState(PlayerState.SNEAKING);
                }

                else if (Input.GetButtonDown("Dodge"))
                {
                    Debug.Log("Transition to Dodge");
                    TransitionToState(PlayerState.DODGING);
                }

                break;

        //-------R U N N I N G ----------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.RUNNING:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                Debug.Log("Running");

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                else if ( !_isGrounded )
                {
                    TransitionToState(PlayerState.FALLING);
                }

               else if (!Input.GetButton("Run"))
                {
                    TransitionToState(PlayerState.JOGGING);
                }

               else if (Input.GetButtonDown("Dodge"))
                {
                    
                    TransitionToState(PlayerState.DODGING);
                }

                break;
            
        //------S N E A K I N G --------------------------------------------------------------------------------------------------------------------------------------------
                
            case PlayerState.SNEAKING:
                Move();
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                _animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                if (Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                else if (Input.GetButtonDown("Sneak") && _isSneaking == true)
                {
                    TransitionToState(PlayerState.JOGGING);
                }

                else if (!_isGrounded)
                {
                    TransitionToState(PlayerState.FALLING);
                }

                break;

        //------J U M P I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.JUMPING:
                Move();
                _animator.SetBool("isJumping", true);
                _animator.SetBool("isGrounded", false);

                if ( _rigidbody.velocity.y < -0.2f && !_isGrounded )
                {
                    TransitionToState(PlayerState.FALLING);
                    
                }

                break;

        //------F A L L I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.FALLING:
                Move();
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", false);

                if (_isGrounded)
                {
                    TransitionToState(PlayerState.IDLE);
                    _animator.SetBool("isGrounded", true);

                }

                break;

       //------D O D G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.DODGING:

                StartCoroutine(Dodge());
                        
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
                _isSneaking = false;
                _animator.SetBool("isSneaking", false);
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.FALLING:
                break;
            case PlayerState.DODGING:
                _isDodging = false;
                _animator.SetBool("isDodging", false);
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
        _direction = _cameraTransform.forward.normalized * Input.GetAxis("Vertical")   //forward/backward movement: relative to CAMERA
                      + _cameraTransform.right.normalized * Input.GetAxisRaw("Horizontal");//left-right movement: relative to PLAYER

        // Smoothing movement
       // _currentSpeed = Mathf.SmoothDamp( _smoothSpeed, _currentSpeed, ref _speedSmoothDampVelocity, _speedSmoothTime);

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
        rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
        _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2f); // by default OverlapBox only takes half the size of the box in each dimension, so we need to double the size of _boxDimension
    }



    /*
    public void Dodge(float deltaTime)
    {
        Vector3 _dodgeMovement = new Vector3();

        _dodgeMovement += _cameraTransform.forward * Input.GetAxis("Vertical") * _dodgeLength / _dodgeDuration;
        _dodgeMovement += _cameraTransform.right * Input.GetAxisRaw("Horizontal") * _dodgeLength / _dodgeDuration;

        //make sure character rotates into direction of movement  
        Quaternion rollRotation = Quaternion.LookRotation(_direction);
        _rigidbody.transform.rotation = rollRotation;

        remainingDodgeTime -= deltaTime;
    }
    */

    IEnumerator Dodge()
    {
        _isDodging = true;
        float timer = 0;
        //_animator.SetTrigger("DodgeTrigger");
        _animator.SetBool("isDodging", true);

        while (timer < _dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            //Vector3 dodgeDir = (_cameraTransform.forward * Input.GetAxis("Vertical"))
            //                 + (_cameraTransform.right * Input.GetAxisRaw("Horizontal")) * speed;
            //_rigidbody.Move(dodgeDir * Time.deltaTime);
            timer += Time.deltaTime;
            
            yield return null;
        }
    }
























}
