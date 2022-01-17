using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BTTask_AlwaysFail : BTNode
{
    public BTTask_AlwaysFail(AIController aIController) : base(aIController)
    {
    }

    public override void EndTask()
    {
        
    }

    public override EBTTaskResult Execute()
    {
        return EBTTaskResult.Faliure;
    }

    public override EBTTaskResult UpdateTask()
    {
        return EBTTaskResult.Faliure;
    }
}

