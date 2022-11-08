using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateBase : StateMachineBehaviour
{
    private PlayerControls playerControls;
    public PlayerControls GetPlayerControls(Animator animator)
    {
        if (playerControls == null)
        {
        playerControls = animator.GetComponent<PlayerControls>();
        }
        
        return playerControls;


    }
}
