using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;

    public const string WalkParameter = "Walk";
    public const string RunParameter = "Run";
    public const string AttackParameter = "Attack";
    public const string DeadParameter = "Dead";

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(bool walk)
    {
        anim.SetBool(WalkParameter, walk);
    }

    public void Run(bool run)
    {
        anim.SetBool(RunParameter, run);
    }

    public void Attack()
    {
        anim.SetTrigger(AttackParameter);
    }

    public void Dead()
    {
        anim.SetTrigger(DeadParameter);
    }
}
