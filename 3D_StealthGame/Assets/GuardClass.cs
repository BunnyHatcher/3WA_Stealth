using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardClass : MonoBehaviour
{
    /*[HideInInspector]*/ public GuardStateMachine guardStateMachine;
    /*[HideInInspector]*/ public MoveAgent moveAgent;

    private void Start()
    {
        guardStateMachine = GetComponent<GuardStateMachine>();
        moveAgent = GetComponent<MoveAgent>();
    }
}
