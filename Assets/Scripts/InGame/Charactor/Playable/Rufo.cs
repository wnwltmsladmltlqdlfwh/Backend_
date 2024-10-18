using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Rufo : InGameBaseController
{
    enum AnimNames
    {
        Attack1_1,
        Attack2_1,
        Bind,
        Die,
        Groggy,
        Hit,
        Idle,
        Move,
        Skill1_1,
        Skill1_2,
        Skill1_3,
        Skill1_4,
        Skill1_5,
        Skill1_6,
        Skill1_7,
        Spawn,
        Ultimate1_1,
        Victory,
    }

    [SerializeField]
    bool usingSkill = false;

    protected override void Init()
    {
        _anim = transform.GetComponentInChildren<SkeletonAnimation>();
    }

    public override void UpdateIdle()
    {
        _anim.AnimationName = AnimNames.Idle.ToString();
    }

    public override void UpdateMove()
    {
        _anim.AnimationName = AnimNames.Move.ToString();
    }

    public override void UpdateAttack()
    {
        _anim.AnimationName = AnimNames.Attack1_1.ToString();
    }

    public override void UpdateSkill()
    {
        if (usingSkill == false)
            StartCoroutine(SkillCoroutine());
    }

    IEnumerator SkillCoroutine()
    {
        usingSkill = true;

        _anim.AnimationState.SetAnimation(0, "Skill1_1", false);

        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_1"));
        _anim.AnimationState.AddAnimation(0, "Skill1_2", false, 0);
        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_2"));
        _anim.AnimationState.AddAnimation(0, "Skill1_3", false, 0);
        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_3"));
        _anim.AnimationState.AddAnimation(0, "Skill1_4", false, 0);
        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_4"));
        _anim.AnimationState.AddAnimation(0, "Skill1_5", false, 0);
        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_5"));
        _anim.AnimationState.AddAnimation(0, "Skill1_6", false, 0);
        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_6"));
        _anim.AnimationState.AddAnimation(0, "Skill1_7", false, 0);

        yield return new WaitUntil(() => AnimatorIsDone(0, "Skill1_7"));
        usingSkill = false;
        _state = Define.State.Idle;
    }

    private bool AnimatorIsDone(int trackIndex, string animationName)
    {
        var stateInfo = _anim.state.GetCurrent(trackIndex);
        return stateInfo != null
            && stateInfo.Animation.Name == animationName
            && stateInfo.AnimationTime >= stateInfo.AnimationEnd;
    }

    public override void UpdateUltimate()
    {
        _anim.AnimationName = AnimNames.Ultimate1_1.ToString();
    }

    public override void UpdateGroggy()
    {
        _anim.AnimationName = AnimNames.Groggy.ToString();
    }

    public override void UpdateDie()
    {
        _anim.AnimationName = AnimNames.Die.ToString();
    }
}
