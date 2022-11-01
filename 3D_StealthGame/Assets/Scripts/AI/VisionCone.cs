using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    
    [HideInInspector]
    public GameObject _target;

    public bool _playerDetected;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;

            if(Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _playerLayer))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Guard has seen player");
                    _target = other.gameObject;
                    _playerDetected = true;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _target = null;
        }
    }
}
