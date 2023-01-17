using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkState : State
{
    private Dialogue playCurrentDialogue;

    public TalkState(StateEnum id)
    {
        this.id = id;
    }

    public override void OnEnter(IUser _iUser)
    {
        base.OnEnter(_iUser);
        if (!_iUser.npc.idleNPC) { _iUser.navMeshAgent.enabled = false; }
        _iUser.anim.SetBool("isWalking", false);
        _iUser.npc.gameObject.transform.LookAt(CharacterMovement.Instance.playerObj, Vector3.up);
        _iUser.npc.gameObject.transform.eulerAngles = new Vector3(0, _iUser.npc.gameObject.transform.eulerAngles.y, 0);

        if(_iUser.npcDialogue.Count != 0)
        {
            playCurrentDialogue = _iUser.npcDialogue[0];
            _iUser.npcDialogue.RemoveAt(0);
        }
        else { playCurrentDialogue = _iUser.npcLoopDialogue; }

        DialogueManager.Instance.StartConversation(playCurrentDialogue, _iUser.npc);

        if(playCurrentDialogue.optionNames.Length == 0) { DialogueManager.Instance.StartDialogue(); }
        else { DialogueManager.Instance.StartOptions(); }

    }

    public override void OnExit()
    {
        if (!_iUser.npc.idleNPC) { _iUser.navMeshAgent.enabled = true; }
    }

    public override void OnUpdate()
    {

    }
}
