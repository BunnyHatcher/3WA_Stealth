using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    
   // public GameObject _target;

    private BaseState _baseState;

    public GuardClass guard;

 /*   
    public bool _fullSlider = false;
    public bool _coneDetection = false;
    public bool _suspicionDetection = false;

    public float _suspicionTimer = 1f;
    public float _detectionTimer = 1f;
    */

    Vector3 rayDirectionGizmo;

    MoveAgent moveAgent;

    private void Awake()
    {
        _baseState = FindObjectOfType<BaseState>();
        moveAgent = GetComponentInParent<MoveAgent>();
        guard = GetComponentInParent<GuardClass>();
    }

    private void Update()
    {

        // Collision timer
        /*
        if (_fullSlider == true)
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
        */
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

                    DetectionSlider detectionSlider = GetComponentInParent<DetectionSlider>();

                    detectionSlider.IsSeeing = true;
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
                if (hit.collider.CompareTag("Player"))
                {
                    guard.suspicionDetection.VisionConeDetection(other.gameObject);
                }

            }

        }
    }  
    




    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guard.suspicionDetection.VisionConeExit();
            DetectionSlider detectionSlider = GetComponentInParent<DetectionSlider>();
            detectionSlider.IsSeeing = false;
        }
    }
   





}
