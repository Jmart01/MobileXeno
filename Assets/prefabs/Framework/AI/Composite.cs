using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Composite : BTNode
{
    List<BTNode> Children;
    int CurrentActiveChildIndex;
    BTNode CurrentRunningChild;
    public Composite(AIController aIController) : base(aIController)
    {
        Children = new List<BTNode>();
    }

    public void AddChild(BTNode child)
    {
        if(!Children.Contains(child))
        {
            Children.Add(child);
            child.Parent = this;
        }
    }

    public int GetChildIndex(BTNode bTNode)
    {
        return Children.FindIndex(0, target => { return target == bTNode; });
    }

    public override void EndTask()
    {
        CurrentActiveChildIndex = 0;
        if (Children.Count > 0)
        {
            CurrentRunningChild = Children[0];
        }
        foreach(var child in Children)
        {
            child.Finish();
        }
    }

    public override EBTTaskResult Execute()
    {
        CurrentActiveChildIndex = 0;
        if (Children.Count > 0)
        {
            CurrentRunningChild = Children[0];
        }

        return EBTTaskResult.Running;
    }

    public override EBTTaskResult UpdateTask()
    {
        EBTTaskResult result = EBTTaskResult.Faliure;
        if (!CurrentRunningChild.HasStarted())
        {
            result = CurrentRunningChild.Start();
            return UpdateComposite(result);
        }

        result = CurrentRunningChild.Update();
        return UpdateComposite(result);
    }

    public abstract EBTTaskResult UpdateComposite(EBTTaskResult PreviousResult);
    protected bool MoveToNext()
    {
        if(CurrentRunningChild!=null)
        {
            CurrentRunningChild.Finish();
        }
        CurrentActiveChildIndex = CurrentActiveChildIndex + 1;
        if(CurrentActiveChildIndex >= Children.Count)
        {
            return false;
        }
        CurrentRunningChild = Children[CurrentActiveChildIndex];
        return true;
    }
}

public class Selector : Composite
{
    public Selector(AIController aIController) : base(aIController)
    {
    }

    public override EBTTaskResult UpdateComposite(EBTTaskResult PreviousResult)
    {
        if(PreviousResult == EBTTaskResult.Faliure)
        {
            if(MoveToNext())
            {
                return EBTTaskResult.Running;
            }
            else
            {
                return EBTTaskResult.Faliure;
            }
        }

        if(PreviousResult == EBTTaskResult.Success)
        {
            return EBTTaskResult.Success;
        }

        return EBTTaskResult.Running;
    }
}

public class Sequence : Composite
{
    public Sequence(AIController aIController) : base(aIController)
    {
    }

    public override EBTTaskResult UpdateComposite(EBTTaskResult PreviousResult)
    {
        if(PreviousResult == EBTTaskResult.Success)
        {
            if(MoveToNext())
            {
                return EBTTaskResult.Running;
            }else
            {
                return EBTTaskResult.Success;
            }
        }

        if(PreviousResult == EBTTaskResult.Faliure)
        {   
            return EBTTaskResult.Faliure;
        }

        return EBTTaskResult.Running;
    }
}