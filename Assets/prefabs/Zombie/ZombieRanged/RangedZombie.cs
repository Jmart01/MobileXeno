using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{

    [SerializeField]Rigidbody Fireball;
    [SerializeField] Transform BallLaunchTrans;

    public override void Start()
    {
        base.Start();
        Fireball = GetComponentInChildren<ProjectileLauncher>().GetProjectileToLaunch();
    }
    public override void AttackPoint()
    {
        base.AttackPoint();

        Rigidbody fireballInstance = Instantiate(Fireball, BallLaunchTrans.position, BallLaunchTrans.rotation);
        fireballInstance.velocity = GetComponentInChildren<ProjectileLauncher>().ProjectileVelocity();
    }
}
