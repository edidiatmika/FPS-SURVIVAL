using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(bool walk)
    {
        anim.SetBool(AnimationTags.walkParameter, walk);
    }

    public void Run(bool run)
    {
        anim.SetBool(AnimationTags.runParameter, run);
    }

    public void Attack()
    {
        anim.SetTrigger(AnimationTags.attackTrigger);
    }

    public void Dead()
    {
        anim.SetTrigger(AnimationTags.deadTrigger);
    }
}
