using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : Character
{
    NavMeshAgent navAgent;
    Animator animator;
    Rigidbody ZombieRigidbody;
    float speed;
    Vector3 previousLocation;

    [SerializeField] float StaminaReward = 0.5f;
    [SerializeField] float CreditReward = 10f;
    

    // Start is called before the first frame update
    public override void Start() 
    {
        navAgent = GetComponent<NavMeshAgent>();
        base.Start();
        animator = GetComponent<Animator>();
        ZombieRigidbody = GetComponent<Rigidbody>();
        previousLocation = transform.position;
    }

    internal void Attack()
    {
        animator.SetLayerWeight(1, 1);
    }
    public virtual void AttackPoint()
    {
        
    }
    public void AttackFinished()
    {
        animator.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float MoveDelta = Vector3.Distance(transform.position, previousLocation);
        speed = MoveDelta / Time.deltaTime;
        previousLocation = transform.position; 
        animator.SetFloat("Speed", speed);
    }

    public void ChangeReward(float multiplier)
    {
        CreditReward = CreditReward * multiplier;
    }

    public override void NoHealthLeft(GameObject killer)
    {
        base.NoHealthLeft();
        AIController AIC = GetComponent<AIController>();
        if(AIC != null)
        {
            AIC.StopAIBehavior();
        }
        if(killer != null)
        {
            AbilityComponent abilityComp = killer.GetComponent<AbilityComponent>();
            CreditSystem creditSystem = killer.GetComponent<CreditSystem>();
            if(abilityComp && creditSystem)
            {
                abilityComp.ChangeStamina(StaminaReward);
                creditSystem.ChangeCredit(CreditReward);
            }
        }
    }

    public void UpdateFromEnemySaveData(EnemySaveData data)
    {
        transform.position = data.Location;

        HealthComponent healthComp = GetComponent<HealthComponent>();
        healthComp.ChangeHealth(data.Health - healthComp.GetHealth());
    }

    public EnemySaveData GenerateEnemySaveData()
    {
        return new EnemySaveData(transform.position,
            GetComponent<HealthComponent>().GetHealth(),
            gameObject.name);
    }
}

[Serializable]
public struct EnemySaveData
{
    public EnemySaveData(Vector3 enemyLoc, float enemyHealth, string enemyName)
    {
        Location = enemyLoc;
        Health = enemyHealth;
        Name = enemyName;
    }

    public Vector3 Location;
    public float Health;
    public string Name;
}
