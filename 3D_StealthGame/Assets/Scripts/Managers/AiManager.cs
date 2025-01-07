using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public static AiManager instance;

    [SerializeField] private List<GuardClass> guardList;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GuardsTargetCamera(CameraBehavior cam)
    {
        foreach (GuardClass guard in guardList) 
        {
            guard.moveAgent._target = cam.transform;
            guard.guardStateMachine.FollowStart();
            Debug.Log("Follow State");
            
        }
    }
}
