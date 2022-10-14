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

public class PlayerMovement : MonoBehaviour
{
    //public and serialized
    public float _moveSpeed = 10f;
    public float _turnSpeed = 70f;
    public float _jumpForce = 5f;


    //privates and protected
    private PlayerState _currentState;
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private Animator _animator;
    private bool _isJumping = false;


    private void Awake() // usually used for getting components of the object the script is on
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() // usually used to get components in other objects
    {
        _cameraTransform = Camera.main.transform;
        TransitionToState(PlayerState.IDLE);
        
    }



    void Update()
    {
        OnStateUpdate();


    }


    private void FixedUpdate()
    {
       if (_isJumping )
        {
            _direction.y = _jumpForce;
            _isJumping = false; // to prevent changing to isJumping every frame after the first one
        }

       else
        { 
         // add direction on y-axis to simulate normal gravity when falling, other wise characte will drop only very slowly
       _direction.y = _rigidbody.velocity.y;
        }

        RotateTowardsCamera();
        
        // move character by setting its velocity to the direction of movement calculated earlier
       _rigidbody.velocity = _direction;

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
                }

                else if(Input.GetButtonDown("Jump"))
                {
                    TransitionToState(PlayerState.JUMPING);
                }

                break;

        //------W A L K I N G---------------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.WALKING:
                break;

        //-------J O G G I N G --------------------------------------------------------------------------------------------------------------------------------------------

            case PlayerState.JOGGING:
                Move();
                if (_direction.magnitude == 0)
                {
                    TransitionToState(PlayerState.IDLE);
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

                if (_rigidbody.velocity.y > 0)
                {
                    TransitionToState(PlayerState.FALLING);
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

    private void RotateTowardsCamera()
    {
        
        Vector3 lookDirection = _cameraTransform.forward;   // make a copy of cameraTransform.forward...
        lookDirection.y = 0;    // ... to be able to set the y-axis of the camera to 0
        // --> otherwise player would lean forward when looking down and lean backward when looking up

        Quaternion rotation = Quaternion.LookRotation(lookDirection); // declare the look rotation according to cameraForward...
        rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
        _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate




    }



}
