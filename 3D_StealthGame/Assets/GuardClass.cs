using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardClass : MonoBehaviour
{
    /*[HideInInspector]*/ public GuardStateMachine guardStateMachine;
    /*[HideInInspector]*/ public MoveAgent moveAgent;
    public SuspicionDetection suspicionDetection;
    public DetectionSlider detectionSlider;

    private void Awake()
    {
        guardStateMachine = GetComponent<GuardStateMachine>();
        moveAgent = GetComponent<MoveAgent>();
        suspicionDetection = GetComponent<SuspicionDetection>();
        detectionSlider = GetComponent<DetectionSlider>();
    }
}
