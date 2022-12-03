using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Animator))]

public class AnimateAgent : MonoBehaviour
{
    [SerializeField]
    GameObject _enemy;
    Animator _anim;
    NavMeshAgent _agent;
    Vector2 _smoothDeltaPosition = Vector2.zero;
    Vector2 _velocity = Vector2.zero;


    private void Awake()
    {
        //_enemy = GameObject.Find("Werehog");
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        // turn off RootMotion
        _anim.applyRootMotion = false;

        // Don’t update position automatically
        _agent.updatePosition = false;
    }

    void Update()
    {
        Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            _velocity = _smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.radius;

        // Update animation parameters
        _anim.SetBool("isMoving", shouldMove);
        _anim.SetFloat("velocityX", _velocity.x);
        _anim.SetFloat("velocityY", _velocity.y);

        LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
            lookAt.lookAtTargetPosition = _agent.steeringTarget + transform.forward;
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = _agent.nextPosition;
    }

    #region Methods

    public void PlayTargetAnimation()
    {

    }

    #endregion

}
