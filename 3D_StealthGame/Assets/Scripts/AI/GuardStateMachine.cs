using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateMachine : MonoBehaviour
{
    private StateMachine _brain;
    private Animator _animator;

    void Start()
    {
        _brain = GetComponent<StateMachine>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
