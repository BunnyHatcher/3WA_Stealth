using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;
    [SerializeField] private float _rotateSpeed;

    private Transform _target;
    private Transform _playerTransform = null;
    private bool _rightToLeft = true;

    
    
    
    
    private void Start()
    {
        _target = _rightLimit;
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            transform.LookAt(_playerTransform.position);
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
            Debug.Log("Player detected");
            _playerTransform = other.transform;
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
