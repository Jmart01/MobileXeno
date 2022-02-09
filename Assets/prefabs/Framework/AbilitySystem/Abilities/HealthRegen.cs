using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName ="Abilities/HealthRegen")]
public class HealthRegen : AbilityBase
{
    [SerializeField] float RegenAmount = 2.5f;
    [SerializeField] float RegenTime = 2.0f;

    HealthComponent healthComp;

    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        healthComp = ownerComp.GetComponent<HealthComponent>();
    }
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(HealthRegenCoroutine());
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        float RegenCounter = 0.0f;
        while(RegenCounter < RegenTime)
        {
            yield return new WaitForEndOfFrame();
            RegenCounter += Time.deltaTime;
            healthComp.ChangeHealth(RegenAmount / RegenTime * Time.deltaTime);
        }
        yield return new WaitForEndOfFrame();
    }
}
