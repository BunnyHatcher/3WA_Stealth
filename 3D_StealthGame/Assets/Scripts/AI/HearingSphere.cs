using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttentionSlider attentionSlider = GetComponentInParent<AttentionSlider>();
            attentionSlider.SliderEnable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttentionSlider attentionSlider = GetComponentInParent<AttentionSlider>();
            attentionSlider.SliderDisable();
        }
    }
}
