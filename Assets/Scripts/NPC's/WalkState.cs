using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : State
{
    private float goalRadius = 200;
    private Vector3 finalPostion;

    public WalkState(StateEnum id)
    {
        this.id = id;
    }

    public override void OnEnter(IUser _iUser)
    {
        base.OnEnter(_iUser);

        _iUser.anim.SetBool("isWalking", true);
        SetDestination();
    }

    public override void OnExit()
    {
        _iUser.navMeshAgent.ResetPath();
    }

    public override void OnUpdate()
    {
        if (_iUser.navMeshAgent.remainingDistance <= _iUser.navMeshAgent.stoppingDistance)
        {
            fsm.SwitchState(StateEnum.Idle);
            _iUser.npc.reachedFirstDes = true;
        }
    }

    private void SetDestination()
    {
        if (!_iUser.npc.reachedFirstDes)
        {
            finalPostion = _iUser.npc.direction.position;
        }
        else if(GameManger.Instance.dayNightCycle.dayTime == DayNightCycle.DayTime.Day)
        {
            GetRandomDestination();
        }

        _iUser.navMeshAgent.destination = finalPostion;
    }

    private void GetRandomDestination()
    {
        Vector3 _randomDirection = UnityEngine.Random.insideUnitSphere * goalRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(_randomDirection, out hit, goalRadius, NavMesh.AllAreas);
        finalPostion = hit.position;
    }
}