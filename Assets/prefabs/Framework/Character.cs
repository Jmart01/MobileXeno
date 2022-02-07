using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    HealthComponent healthComp;

    // Start is called before the first frame update
    public virtual void Start()
    {
        healthComp = GetComponent<HealthComponent>();

        healthComp.onHealthChanged += HealthChanged;
        healthComp.noHealthLeft += NoHealthLeft;
    }

    public virtual void NoHealthLeft(GameObject killer = null)
    {
        //play DeathAnimation
        
        //always check the animation layer
        GetComponent<Animator>().SetLayerWeight(2, 1);
        int DeathStateNameHash = Animator.StringToHash("DeathState");
        GetComponent<Animator>().Play(DeathStateNameHash, 2);
    }

    public void OnDeathAnimationFinished()
    {
        Destroy(gameObject);
    }

    public virtual void HealthChanged(float newValue, float oldValue, float maxValue, GameObject Cause)
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
