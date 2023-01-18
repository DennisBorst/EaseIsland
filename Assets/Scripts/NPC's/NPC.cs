using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum StateEnum
{
    Idle,
    Walk,
    Talking,
    Tutorial
}

public class NPC : MonoBehaviour, IUser
{
    [HideInInspector] public bool inconversation;
    [HideInInspector] public Transform direction;
    [HideInInspector] public bool reachedFirstDes = false;
    public bool idleNPC = false;
    [Space]
    public Transform tutorialTransform;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform dayTransform;
    [SerializeField] private Transform nightTransform;
    [SerializeField] private float walkSpeed;
    [Space]
    [SerializeField] private List<Dialogue> npcDialogue = new List<Dialogue>();
    [SerializeField] private Dialogue npcLoopDialogue;
    [Space]
    [SerializeField] private UnityEvent[] events;
    [SerializeField] private StateEnum startStateEnum;

    private NavMeshAgent agent;
    private State state;

    public FSM fsm;
    public State startState;

    NPC IUser.npc => this;
    NavMeshAgent IUser.navMeshAgent => agent;
    Animator IUser.anim => anim;
    List<Dialogue> IUser.npcDialogue => npcDialogue;
    Dialogue IUser.npcLoopDialogue => npcLoopDialogue;

    public void ChangeState(StateEnum state)
    {
        fsm.SwitchState(state);
    }

    public void AddDayNightTimeEvent()
    {
        GameManger.Instance.dayNightCycle.dayEvent.AddListener(DayEvent);
        GameManger.Instance.dayNightCycle.nightEvent.AddListener(NightEvent);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        direction = dayTransform;

        fsm = new FSM(this, startStateEnum, new IdleState(StateEnum.Idle),
                    new WalkState(StateEnum.Walk), new TalkState(StateEnum.Talking),
                    new TutorialState(StateEnum.Tutorial));
    }

    private void Start()
    {
        if (idleNPC) { return; }
        GameManger.Instance.dayNightCycle.dayEvent.AddListener(DayEvent);
        GameManger.Instance.dayNightCycle.nightEvent.AddListener(NightEvent);
    }

    private void Update()
    {
        if (fsm != null)
        {
            fsm.OnUpdate();
        }
    }

    private void DayEvent()
    {
        reachedFirstDes = false;
        direction = dayTransform;

        if(fsm.currentState != fsm.states[StateEnum.Talking])
        {
            fsm.SwitchState(StateEnum.Walk);
        }
    }

    private void NightEvent()
    {
        direction = nightTransform;

        if (fsm.currentState != fsm.states[StateEnum.Talking])
        {
            fsm.SwitchState(StateEnum.Idle);
            reachedFirstDes = false;
            fsm.SwitchState(StateEnum.Walk);
        }
        else
        {
            reachedFirstDes = false;
        }
    }

    public void InvokeOptionEvent(int eventInt)
    {
        events[eventInt].Invoke();
    }
}
