using UnityEngine;

class BTTask_AttackTarget : BTNode
{
    string _targetKey;
    public BTTask_AttackTarget(AIController aIController, string targetKey) : base(aIController)
    {
        _targetKey = targetKey;
    }

    public override void EndTask()
    {
        
    }

    public override EBTTaskResult Execute()
    {
        AIC.GetBehaviorTree().GetBlackboardValue(_targetKey, out object value);
        GameObject TargetObj = (GameObject)value;
        if(TargetObj==null)
        {
            return EBTTaskResult.Faliure;
        }

        AIC.transform.LookAt(TargetObj.transform);
        AIC.GetComponent<Zombie>().Attack();
        return EBTTaskResult.Success;
    }

    public override EBTTaskResult UpdateTask()
    {
        return EBTTaskResult.Success;
    }
}

