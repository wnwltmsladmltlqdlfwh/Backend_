using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using System;

public abstract class InGameBaseController : MonoBehaviour
{
    protected Define.State _state = Define.State.Idle;

    protected SkeletonAnimation _anim = null;

    void Start()
    {
        Init();
    }

    protected abstract void Init();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _state = Define.State.Attack;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            _state = Define.State.Moving;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _state = Define.State.Skill_1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _state = Define.State.Ultimate;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _state = Define.State.Die;
        }

        switch (_state)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMove();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Skill_1:
                UpdateSkill();
                break;
            case Define.State.Ultimate:
                UpdateUltimate();
                break;
            case Define.State.Groggy:
                UpdateGroggy();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
        }
    }

    public virtual void UpdateIdle() { }
    public virtual void UpdateMove() { }
    public virtual void UpdateAttack() { }
    public virtual void UpdateSkill() { }
    public virtual void UpdateUltimate() { }
    public virtual void UpdateGroggy() { }
    public virtual void UpdateDie() { }
}
