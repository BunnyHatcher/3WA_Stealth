using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionDetection : MonoBehaviour
{

    public bool _fullSlider = false;
    public bool _coneDetection = false;
    public bool _suspicionDetection = false;

    public float _suspicionTimer = 1f;
    public float _detectionTimer = 1f;

    public GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectionCheck();
    }



    void DetectionCheck()
    {

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

    }


    public void VisionConeDetection(GameObject detectionTarget)
    {
        if (!_fullSlider) return;
        
            if (_suspicionTimer <= 0)
            {
                Debug.Log("Suspicion detection");
                _suspicionDetection = true;
                _target = detectionTarget.gameObject;
                //  moveAgent.StopMovement();
                //  _baseState._timeSinceLastSawPlayer = 0;
            }

            if (_detectionTimer <= 0)
            {
                Debug.Log("Full detection");
                _coneDetection = true;
                _target = detectionTarget.gameObject;
                //    moveAgent.ResumeMovement();
            }
    }

    public void VisionConeExit()
    {
        Debug.Log("Player exited Detection");
        _coneDetection = false;
        //    _fullSlider = false;
        DetectionSlider detectionSlider = GetComponentInParent<DetectionSlider>();
        //detectionSlider.SliderDisable();
        detectionSlider.IsSeeing = false;
        _target = null;
    }
}
