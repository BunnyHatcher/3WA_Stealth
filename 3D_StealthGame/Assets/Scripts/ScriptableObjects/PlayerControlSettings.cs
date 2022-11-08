using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerControlSettings", order = 1)]
public class PlayerControlSettings : ScriptableObject
{
    // FIX MEMBERS
    
    public float _currentSpeed = 6f;
    public float _runningSpeed = 10f;
    public float _walkingSpeed = 1f;
    public float _joggingSpeed = 5f;
    public float _sneakingSpeed = 2f;
    public float _turnSpeed = 500f;
    public float _jumpForce = 5f;

}



