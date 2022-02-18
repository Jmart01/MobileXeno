using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Abilities/InstaKillAbility")]
public class InstaKillAbility : AbilityBase
{
    [SerializeField] float ActiveForX = 5f;

    Zombie[] zombies;
    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp); 
    }

    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(ActivateInstaKillCoroutine());
            zombies = FindObjectsOfType<Zombie>();
            foreach(Zombie zombie in zombies)
            {
                zombie.GetComponent<HealthComponent>().ChangeHealth(-9);
            }
        }
    }

    private IEnumerator ActivateInstaKillCoroutine()
    {
        if(zombies != null)
        {
            float abilityTimer = 0.0f;
            while (abilityTimer < ActiveForX)
            {
                abilityTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
        }
        foreach(Zombie zombie in zombies)
        {
            zombie.GetComponent<HealthComponent>().ChangeHealth(9);
        }
        Debug.Log("no zombies found");
    }
    

   
   
}
