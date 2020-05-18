using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimStateMachine : StateMachineBehaviour
{
    public delegate void AnimationEnd();
    public event AnimationEnd OnAnimationEnd;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAnimationEnd?.Invoke();
    }
}
