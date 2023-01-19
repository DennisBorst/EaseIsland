using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private float maxTimer = 10;
    private float timer;

    public IdleState(StateEnum id)
    {
        this.id = id;
    }

    public override void OnEnter(IUser _iUser)
    {
        base.OnEnter(_iUser);
        _iUser.anim.SetBool("isWalking", false);

        timer = maxTimer;
        //timer = Random.RandomRange(maxTimer - 3, maxTimer);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (_iUser.npc.idleNPC) { return; }

        timer = Timer(timer);

        if (timer <= 0)
        {
            fsm.SwitchState(StateEnum.Walk);
        }
    }

    private float Timer(float timer)
    {
        timer -= Time.deltaTime;
        return timer;
    }
}
