using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeZombie : Zombie
{
    [SerializeField] BoxCollider AttackRange;

    public override void AttackPoint()
    {
        base.AttackPoint();
        Collider[] targets = Physics.OverlapBox(AttackRange.bounds.center, AttackRange.bounds.extents, AttackRange.transform.rotation);
        foreach (var target in targets)
        {
            Player targetAsPlayer = target.GetComponent<Player>();
            if (targetAsPlayer)
            {
                target.GetComponent<HealthComponent>().ChangeHealth(-1, gameObject);
            }
        }
    }


}
