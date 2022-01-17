
using System;

public enum EUpdateAbort
{
    None,
    LowerPriority,
    Self,
    Both,
}

public enum EKeyQuery
{
    Set,
    NotSet
}

public class BlackboardDecorator : Decorator
{
    string _keyName;
    EKeyQuery _keyQuery;
    EUpdateAbort _updateAbort;
    public BlackboardDecorator(AIController aIController, BTNode child, string keyName, EKeyQuery keyQuery, EUpdateAbort observeAbort) : base(aIController, child)
    {
        _keyName = keyName;
        _keyQuery = keyQuery;
        _updateAbort = observeAbort;
        aIController.GetBehaviorTree().onBlackboardKeyUpdated -= BlackBoardValueUpdated;
        aIController.GetBehaviorTree().onBlackboardKeyUpdated += BlackBoardValueUpdated;
    }

    private void BlackBoardValueUpdated(string name, object value)
    {
        if (name != _keyName)
        {
            return;
        }

        //we are ruing ourselves:
        if (AIC.GetBehaviorTree().IsRunningNode(this))
        {
            if(!ShouldDoTask(value) && (_updateAbort == EUpdateAbort.Both || _updateAbort == EUpdateAbort.Self))
            {
                AbortTask();
            }
        }
        //we are runinng lower priority
        else if (AIC.GetBehaviorTree().IsLowerPriority(AIC.GetBehaviorTree().GetCurrentRunningNode(), this)) 
        { 
            if(ShouldDoTask(value) && (_updateAbort == EUpdateAbort.Both ||_updateAbort == EUpdateAbort.LowerPriority))
            {
                AbortLowerPriority();
            }
        }
    }

    private void AbortLowerPriority()
    {
        if (AIC.GetBehaviorTree().IsLowerPriority(AIC.GetBehaviorTree().GetCurrentRunningNode(), this))
        {
            AIC.GetBehaviorTree().RestartTree();
        }
    }

    public override void EndTask()
    {
        
    }

    public override EBTTaskResult Execute()
    {
        AIC.GetBehaviorTree().GetBlackboardValue(_keyName, out object keyVal);
        if (ShouldDoTask(keyVal))
        {
            return EBTTaskResult.Running;
        }
        return EBTTaskResult.Faliure;
    }

    public override EBTTaskResult UpdateTask()
    {
        EBTTaskResult result = EBTTaskResult.Faliure;
        if(!Child.HasStarted())
        {
            result = Child.Start();
            if(result != EBTTaskResult.Running)
            {
                return result;
            }
        }

        result = Child.Update();
        return result;
    }

    bool ShouldDoTask(object keyVal)
    {
        switch (_keyQuery)
        {
            case EKeyQuery.Set:
                if (keyVal != null)
                    return true;
                break;
            case EKeyQuery.NotSet:
                if (keyVal == null)
                    return true;
                break;
        }

        return false;
    }
}

