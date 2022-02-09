using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/FasterMovementAbility")]
public class FasterMovementAbility : AbilityBase
{
    [SerializeField] float FastForX = 5f;
    [SerializeField] float speedMulti = 2f;
    MovementComponent movementComp;
    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        movementComp = ownerComp.GetComponent<MovementComponent>();
    }
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(IncreaseMoveSpeedCoroutine());
        }
    }

    private IEnumerator IncreaseMoveSpeedCoroutine()
    {
        float increaseTimer = 0.0f;
        while(increaseTimer < FastForX)
        {
            increaseTimer += Time.deltaTime;
            movementComp.SetSpeedMulti(speedMulti);
            yield return new WaitForEndOfFrame();
        }
        movementComp.SetSpeedMulti(1);
        Debug.Log("SET IT TO ONE");
        yield return null;
    }
}
