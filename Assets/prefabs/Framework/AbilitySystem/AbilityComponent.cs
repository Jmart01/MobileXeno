using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnNewAbilityInitialized(AbilityBase newAbility);
public delegate void OnStaminaUpdated(float newValue);
public class AbilityComponent : MonoBehaviour
{
    [SerializeField] float StaminaLevel;
    [SerializeField] float MaxStaminaLevel;
    [SerializeField] float StaminaDropSpeed;
    [SerializeField] float StaminaDrainingStartDelay = 2;

    [SerializeField] AbilityBase[] abilities;

    public event OnNewAbilityInitialized onNewAbilityInitialized;
    public event OnStaminaUpdated onStaminaUpdated;
    Coroutine StaminaDraingCor;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInitialized?.Invoke(abilities[i]);
        }
        StaminaDraingCor = StartCoroutine(StaminaDrainingCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeStamina(float changeAmount)
    {
        if(changeAmount > 0)
        {
            if(StaminaDraingCor!= null)
            {
                StopCoroutine(StaminaDraingCor);
                StaminaDraingCor = StartCoroutine(StaminaDrainingCoroutine());
            }
        }
        StaminaLevel = Mathf.Clamp(StaminaLevel + changeAmount, 0, MaxStaminaLevel);
        onStaminaUpdated?.Invoke(StaminaLevel);
    }

    IEnumerator StaminaDrainingCoroutine()
    {
        yield return new WaitForSeconds(StaminaDrainingStartDelay);
        while(StaminaLevel > 0)
        {
            StaminaLevel -= StaminaDropSpeed * Time.deltaTime;
            onStaminaUpdated?.Invoke(StaminaLevel);
            yield return new WaitForEndOfFrame();
        }
        StaminaLevel = Mathf.Clamp(StaminaLevel, MaxStaminaLevel, StaminaLevel);
    }

    internal int GetStaminaLevel()
    {
        return (int)StaminaLevel;
    }
}
