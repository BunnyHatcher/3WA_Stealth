using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterStateBase : StateMachineBehaviour
{
    //References necessary for character movements
    
    protected PlayerControlSettings _playerControlSettings;
    protected CharacterController _controller;
    protected Vector3 _movementDirection;
    protected Transform _cameraTransform;
    protected Transform _playerTransform;
    protected GameObject _playerGameObject;

    private void Awake()
    {
        string GUID = AssetDatabase.FindAssets("PlayerControlValues")[0];
        string path = AssetDatabase.GUIDToAssetPath(GUID);
        _playerControlSettings = (PlayerControlSettings) AssetDatabase.LoadAssetAtPath(path, typeof(PlayerControlSettings));
        Debug.Log(_playerControlSettings._currentSpeed);

        _playerGameObject = GameObject.Find("Erika Archer");

        _playerTransform = _playerGameObject.transform;

        _controller = _playerGameObject.GetComponent<CharacterController>();

        _cameraTransform = Camera.main.transform;
    }

}
