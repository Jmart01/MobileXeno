using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] Zombie zombie;
    [SerializeField] PerceptionComp perceptionComp;
    [SerializeField] BehaviorTree behaviorTree;
    [SerializeField] HealthComponent HealthComp;
    [SerializeField] float hurtRememberingTime = 3;
    GameObject Target;
    Coroutine HurtForgettingCoroutine;
    
    public BehaviorTree GetBehaviorTree()
    {
        return behaviorTree;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(perceptionComp)
        {
            perceptionComp.onPerceptionUpdated += PerceptionUpdated;
        }
        if(behaviorTree)
        {
            behaviorTree.Init(this);
        }
        if(HealthComp)
        {
            HealthComp.onHealthChanged += HealthChanged;
        }
    }

    private void HealthChanged(int newValue, int oldValue, int maxValue, GameObject Causer)
    {
        PerceptionUpdated(true, Causer);
        StartForgetingHurt(Causer);
    }

    private void StartForgetingHurt(GameObject Causer)
    {
        if (HurtForgettingCoroutine != null)
        {
            StopCoroutine(HurtForgettingCoroutine);
            HurtForgettingCoroutine = null;
        }
        HurtForgettingCoroutine = StartCoroutine(ForgetHurt(Causer));
    }

    IEnumerator ForgetHurt(GameObject Causer)
    {
        yield return new WaitForSeconds(hurtRememberingTime);
        if(!perceptionComp.IsCurrentlySensing(Causer))
        {
            behaviorTree.SetBlackboardKey("Target", null);
        }
    }


    private void PerceptionUpdated(bool SuccessfullySensed, GameObject objectSensed)
    {
        if (SuccessfullySensed)
        {
            Target = objectSensed;
            behaviorTree.SetBlackboardKey("CheckLocation", null);
        }
        else
        {
            if (Target == objectSensed)
            {
                behaviorTree.SetBlackboardKey("CheckLocation", Target.transform.position);
                Target = null;
            }
        }
        behaviorTree.SetBlackboardKey("Target", Target);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(behaviorTree != null && behaviorTree.Run() != EBTTaskResult.Running)
        {
            behaviorTree.Reset();
        }
    }
}
