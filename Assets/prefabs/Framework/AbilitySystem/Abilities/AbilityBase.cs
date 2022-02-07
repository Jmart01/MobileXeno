using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    [SerializeField] float CooldownTime = 5f;
    [SerializeField] Sprite Icon;
    [SerializeField] int AbilityLevel = 1;

    public float CooldownPercent
    {
        get;
        private set;
    }

    public AbilityComponent ownerComp
    {
        get;
        private set;
    }


    public Sprite GetIcon()
    {
        return Icon;
    }
    public bool IsOnCooldown
    {
        private set;
        get;
    }

    public virtual void Init(AbilityComponent ownerAbilityComp)
    {
        ownerComp = ownerAbilityComp;
        IsOnCooldown = false;
        CooldownPercent = 1f;
    }

    bool CanCast()
    {
        return !IsOnCooldown && ownerComp.GetStaminaLevel() >= AbilityLevel;
    }

    public abstract void ActivateAbility();

    protected bool CommitAbility()
    {
        if(CanCast())
        {
            StartCooldown();

            return true;
        }
        return false;
    }

    private void StartCooldown()
    {
        ownerComp.StartCoroutine(CooldownCoroutine());
        ownerComp.StartCoroutine(SetMaterialCooldownTime());
    }

    private IEnumerator CooldownCoroutine()
    {   
        IsOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        IsOnCooldown = false;
        yield return null;
    }

    private IEnumerator SetMaterialCooldownTime()
    {
        float timer = 0f;
        while(timer < CooldownTime)
        {
            timer = timer + Time.deltaTime;
            CooldownPercent = Mathf.Clamp(1-(timer / CooldownTime),0,1);

            //Debug.Log(CooldownPercent);
            yield return new WaitForEndOfFrame();
        }
        CooldownPercent = 1;
        yield return null;
    }

    public int GetLevel()
    {
        return AbilityLevel;
    }
}
