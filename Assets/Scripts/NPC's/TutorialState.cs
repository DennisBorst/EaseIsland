using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialState : State
{
    public TutorialState(StateEnum id)
    {
        this.id = id;
    }

    public override void OnEnter(IUser _iUser)
    {
        base.OnEnter(_iUser);

        _iUser.anim.SetBool("isWalking", true);
        _iUser.navMeshAgent.destination = _iUser.npc.tutorialTransform.position;
    }

    public override void OnExit()
    {
        _iUser.npc.idleNPC = false;
        _iUser.navMeshAgent.ResetPath();
    }

    public override void OnUpdate()
    {
        if (_iUser.navMeshAgent.remainingDistance <= _iUser.navMeshAgent.stoppingDistance)
        {
            _iUser.npc.GetComponent<NPCInteractable>().InteractNPC();
        }
    }
}
