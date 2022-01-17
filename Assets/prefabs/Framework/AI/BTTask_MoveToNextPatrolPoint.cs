using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
class BTTask_MoveToNextPatrolPoint : BTNode
{
    NavMeshAgent _navAgent;
    GameObject _nextPatrolPoint;
    float _acceptableRadius;
    public BTTask_MoveToNextPatrolPoint(AIController aIController, float acceptableRadius) : base(aIController)
    {
        _acceptableRadius = acceptableRadius; 
    }

    public override void EndTask()
    {
        _navAgent.isStopped = true;
        _navAgent = null;
        _nextPatrolPoint = null;
    }

    public override EBTTaskResult Execute()
    {
        if (AIC)
        {
            PatrollingComponent patrollingComp =
            AIC.GetComponent<PatrollingComponent>();
            _navAgent = AIC.GetComponent<NavMeshAgent>();
            if (patrollingComp!=null)
            {
                _nextPatrolPoint = patrollingComp.GetNextPatrolPoint();
                if(_nextPatrolPoint!=null)
                {
                    _navAgent.SetDestination(_nextPatrolPoint.transform.position);
                    _navAgent.isStopped = false;
                    return EBTTaskResult.Running;
                }
            }
        }
        return EBTTaskResult.Faliure;
    }

    public override EBTTaskResult UpdateTask()
    {
        if(Vector3.Distance(AIC.transform.position, _nextPatrolPoint.transform.position) <= _acceptableRadius)
        {
            return EBTTaskResult.Success;
        }
        return EBTTaskResult.Running;
    }
}

