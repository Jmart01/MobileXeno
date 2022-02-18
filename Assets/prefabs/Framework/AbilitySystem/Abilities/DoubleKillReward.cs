using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Abilities/DoubleKillReward")]
public class DoubleKillReward : AbilityBase
{
    [SerializeField] float rewardMulti = 2f;
    [SerializeField] float rewardBackToNormal = .5f;
    [SerializeField] float activeTime = 10f;
    Zombie[] zombies;
    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
       
    }
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(ChangeReward());
            zombies = FindObjectsOfType<Zombie>();
            foreach (Zombie zombie in zombies)
            {
                zombie.ChangeReward(rewardMulti);
            }
        }
    }

    private IEnumerator ChangeReward()
    {
        float abilityTimer = 0.0f;
        while(abilityTimer < activeTime)
        {
            abilityTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        foreach (Zombie zombie in zombies)
        {
            zombie.ChangeReward(rewardBackToNormal);
        }
        yield return new WaitForEndOfFrame();
    }

   
}
