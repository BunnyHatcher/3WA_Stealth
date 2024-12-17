using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public static AiManager instance;

    [SerializeField] private List<MoveAgent> guardList;

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
        foreach (MoveAgent guard in guardList) 
        {
            guard._target = cam.transform;
        }
    }
}
