using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DetectionSlider detectionSlider = GetComponentInParent<DetectionSlider>();
            // detectionSlider.SliderEnable();
            detectionSlider.IsHearing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DetectionSlider detectionSlider = GetComponentInParent<DetectionSlider>();
            detectionSlider.SliderDisable();
            detectionSlider.IsHearing = false;
        }
    }
}
