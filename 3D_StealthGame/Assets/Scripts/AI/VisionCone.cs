using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    
    public GameObject _target;

    private BaseState _baseState;

    public bool _investigatingDetection = false;
    public bool _fullDetection = false;
    public bool _suspicionDetection = false;

    public float _suspicionTimer = 1f;
    public float _detectionTimer = 1f;

    Vector3 rayDirectionGizmo;

    MoveAgent moveAgent;

    private void Awake()
    {
        _baseState = FindObjectOfType<BaseState>();
        moveAgent = GetComponentInParent<MoveAgent>();
    }

    private void Update()
    {

        // Collision timer
        if (_investigatingDetection == true)
        {
            _suspicionTimer -= Time.deltaTime;
            if (_suspicionTimer < 0)
            {
                _suspicionTimer = 0;
                Debug.Log("Suspicion Timer = 0");
            }
        }

        if (_suspicionDetection == true)
        {


            _detectionTimer -= Time.deltaTime;
            if (_detectionTimer < 0)
            {
                _detectionTimer = 0;
                Debug.Log("Detection Timer = 0");
            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Fleeting Detection");
                    //_target = other.gameObject;

                    // Old Method based on timer
                    // _investigatingDetection = true;

                    SuspicionSlider suspicionSlider = GetComponentInParent<SuspicionSlider>();
                    //suspicionSlider.SliderEnable();
                    suspicionSlider.IsSeeing = true;
                }
            }

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, rayDirectionGizmo);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;



            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _playerLayer))
            {
                if (hit.collider.CompareTag("Player") && _investigatingDetection == true)
                {
                    if (_suspicionTimer <= 0)
                    {
                        Debug.Log("Suspicion detection");
                        _suspicionDetection = true;
                        _target = other.gameObject;
                      //  moveAgent.StopMovement();
                      //  _baseState._timeSinceLastSawPlayer = 0;
                    }

                    if (_detectionTimer <= 0)
                    {
                        Debug.Log("Full detection");
                        _fullDetection = true;
                        _target = other.gameObject;
                        _baseState._timeSinceLastSawPlayer = 0;
                    //    moveAgent.ResumeMovement();
                    }
                }

            }

        }
    }  
    




    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited Detection");
            _fullDetection = false;
            //    _investigatingDetection = false;
            SuspicionSlider suspicionSlider = GetComponentInParent<SuspicionSlider>();
            //suspicionSlider.SliderDisable();
            suspicionSlider.IsSeeing = false;
            _target = null;
       //     moveAgent.ResumeMovement();
        }
    }
   





}
