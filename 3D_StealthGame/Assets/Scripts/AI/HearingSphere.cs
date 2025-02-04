using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SuspicionSlider suspicionSlider = GetComponentInParent<SuspicionSlider>();
            // suspicionSlider.SliderEnable();
            suspicionSlider.IsHearing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SuspicionSlider suspicionSlider = GetComponentInParent<SuspicionSlider>();
            suspicionSlider.SliderDisable();
            suspicionSlider.IsHearing = false;
        }
    }
}
