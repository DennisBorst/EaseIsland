using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IUser
{
    NPC npc { get; }
    NavMeshAgent navMeshAgent { get; }
    Animator anim { get; }
    List<Dialogue> npcDialogue { get; }
    Dialogue npcLoopDialogue { get; }

}
