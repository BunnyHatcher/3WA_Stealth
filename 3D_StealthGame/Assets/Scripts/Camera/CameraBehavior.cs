using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;
    [SerializeField] private float _rotateSpeed;
    public GameObject lookAtVisor;
    private float _resetTimer = 0f;

    private Transform _target;
    [SerializeField] private Transform _playerTransform = null;
    private bool _rightToLeft = true;
    [SerializeField] LayerMask _rayLayer;





    private void Start()
    {
        _target = _rightLimit;
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
           lookAtVisor.transform.LookAt(_playerTransform.position);
           //_target = _playerTransform;

            if (_resetTimer < Time.timeSinceLevelLoad)
            {
                _playerTransform = null;
            }

        }

        else
        {
            PingPong();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _rayLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                _playerTransform = other.transform;
                Debug.Log("Player detected");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _resetTimer = Time.timeSinceLevelLoad + 2f;
        }

    }



    private void PingPong()
    {
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, _target.localPosition, _rotateSpeed * Time.deltaTime, 0f);
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);

        transform.rotation = lookRotation;

        Quaternion targetRotation = Quaternion.LookRotation(_target.localPosition); // rotation that we want to reach

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f) // if we get close to reaching the same rotation as the target rotation...
        {

            _rightToLeft = !_rightToLeft; // ... the rotation direction is inversed

            if (_rightToLeft)
            {
                _target = _rightLimit;
            }

            else
            {
                _target = _leftLimit;
            }

        }
    }

}
