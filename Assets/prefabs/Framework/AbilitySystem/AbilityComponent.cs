using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnNewAbilityInitialized(AbilityBase newAbility);

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] float StaminaLevel;
    [SerializeField] float MaxStaminaLevel;

    [SerializeField] AbilityBase[] abilities;

    public event OnNewAbilityInitialized onNewAbilityInitialized;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInitialized?.Invoke(abilities[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal int GetStaminaLevel()
    {
        return (int)StaminaLevel;
    }
}
