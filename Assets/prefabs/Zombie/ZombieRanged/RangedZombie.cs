using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{

    public override void AttackPoint()
    {
        base.AttackPoint();
        Debug.Log("throwing my balls");
    }
}
