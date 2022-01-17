using UnityEngine;

public class BTTask_Wait : BTNode
{
    float _waitTime;
    float counter = 0;
    public BTTask_Wait(AIController aIController, float waitTime) : base(aIController)
    {
        _waitTime = waitTime;
    }

    public override void EndTask()
    {
    }

    public override EBTTaskResult Execute()
    {
        counter = 0;
        return EBTTaskResult.Running;
    }

    public override EBTTaskResult UpdateTask()
    {
        counter += Time.deltaTime;
        if (counter >= _waitTime)
        {
            return EBTTaskResult.Success;
        }
        return EBTTaskResult.Running;
    }
}

