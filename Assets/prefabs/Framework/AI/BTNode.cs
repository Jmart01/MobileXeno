using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum EBTTaskResult
{
	Success,
	Running,
	Faliure
}

public abstract class BTNode
{
	BTNode _Parent;
	public BTNode Parent { 
		set { _Parent = value; } 
		get { return _Parent; }
	}
	public BTNode(AIController aIController)
	{
		AIC = aIController;
	}

	public int GetParentIndex()
    {
		Composite parentAsComposit = (Composite)Parent;
		if(parentAsComposit!=null)
        {
			return parentAsComposit.GetChildIndex(this);
        }
		return 0;
    }

	public EBTTaskResult Start()
	{
		if (!Started)
		{
			Finished = false;
			Started = true;
			bShouldAbortSelf = false;
			AIC.GetBehaviorTree().SetCurrentRunningNode(this);
			return Execute();
		}
		return EBTTaskResult.Running;
	}

	public virtual void Finish()
	{
		if (!Finished && Started)
		{
			Finished = true;
			Started = false;
			EndTask();
		}
	}
	public EBTTaskResult Update()
	{
		if (ShouldAboutSelf())
        {
			return EBTTaskResult.Faliure;
        }
		return UpdateTask();
	}
	public abstract EBTTaskResult Execute();
	public abstract EBTTaskResult UpdateTask();
	public abstract void EndTask();

	public bool HasStarted()
	{
		return Started;
	}
	public bool HasFinished()
	{
		return Finished;
	}
	public AIController AIC { get; }
	bool Started = false;
	bool Finished = false;
	bool bShouldAbortSelf = false;
	
	public void AbortTask()
	{
		bShouldAbortSelf = true;
	}

	public bool ShouldAboutSelf()
    {
		return bShouldAbortSelf;
    }
}
