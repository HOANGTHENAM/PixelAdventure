using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAYERSTATE = PlayerController.PLAYERSTATE;

public class AnimController : MonoBehaviour
{
    PLAYERSTATE _currentState;
    [SerializeField] Animator animator;
 

    public void ChangeAnim(PLAYERSTATE newState)
    {
        _currentState = newState;
        animator.SetTrigger(newState.ToString());
    }

    public void ChangeBlend(string name, float val)
    {
        animator.SetFloat(name, val);   
    }
}
