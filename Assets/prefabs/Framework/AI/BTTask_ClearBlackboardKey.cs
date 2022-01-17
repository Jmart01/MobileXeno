using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BTTask_ClearBlackboardKey : BTNode
{
    string _keyName;
    public BTTask_ClearBlackboardKey(AIController aIController, string keyName) : base(aIController)
    {
        _keyName = keyName;
    }

    public override void EndTask()
    {
    }

    public override EBTTaskResult Execute()
    {
        AIC.GetBehaviorTree().SetBlackboardKey(_keyName, null);
        return EBTTaskResult.Success;
    }

    public override EBTTaskResult UpdateTask()
    {
        return EBTTaskResult.Success;
    }
}

