using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private NPC npc;
    private Interactable interactable;
    private InteractableUI interactableUI;

    public void Interact(Item itemInHand, Vector3 playerPos)
    {
        if(!npc.idleNPC && GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Night) { return; }
        InteractNPC();

        //npc.TalkInvoke();
    }

    public void InRange()
    {
        if (npc.inconversation || !npc.idleNPC && GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Night) 
        {
            OutRange();
            return; 
        }

        interactableUI.InRange();
    }

    public void OutRange()
    {
        interactableUI.OutRange();
    }

    public void InteractNPC()
    {
        npc.inconversation = true;
        OutRange();
        npc.ChangeState(StateEnum.Talking);
    }

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactableUI = GetComponent<InteractableUI>();
        npc = GetComponent<NPC>();

        interactable.doAction.AddListener(Interact);
        interactable.inRange.AddListener(InRange);
        interactable.outRange.AddListener(OutRange);
    }
}
