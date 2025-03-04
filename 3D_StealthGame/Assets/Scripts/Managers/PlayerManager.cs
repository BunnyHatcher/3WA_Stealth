using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerStateMachine player;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
    }

   public void OnStateChange(PlayerState ToState)
    {
        AiManager.instance.OnPlayerStateChange(ToState);
    }
}
