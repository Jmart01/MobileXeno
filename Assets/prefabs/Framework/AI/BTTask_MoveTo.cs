using UnityEngine;
using UnityEngine.AI;
class BTTask_MoveTo : BTNode
{
    NavMeshAgent _agent;
    string _keyName;
    float _acceptableRaidus;
    public BTTask_MoveTo(AIController aIController, string keyname, float acceptableRaidus) : base(aIController)
    {
        _keyName = keyname;
        _agent = aIController.GetComponent<NavMeshAgent>();
        _acceptableRaidus = acceptableRaidus;
    }

    public override void EndTask()
    {
        if(_agent.isActiveAndEnabled)
        {
            _agent.isStopped = true;
        }
    }

    public override EBTTaskResult Execute()
    {

        Vector3 Destination = GetDestination();
        if(Destination == Vector3.negativeInfinity)
        {
            return EBTTaskResult.Faliure;
        }

        _agent.isStopped = false;
        _agent.destination = Destination;
        return EBTTaskResult.Running;        
    }

    public override EBTTaskResult UpdateTask()
    {

        Vector3 Destination = GetDestination();
        if (Destination == Vector3.negativeInfinity)
        {
            return EBTTaskResult.Faliure;
        }
        _agent.destination = Destination;

        if (Vector3.Distance(AIC.transform.position, Destination) <= _acceptableRaidus)
        {
            return EBTTaskResult.Success;
        }
        return EBTTaskResult.Running;
    }
    
    Vector3 GetDestination()
    {
        AIC.GetBehaviorTree().GetBlackboardValue(_keyName, out object value);
        Vector3 Position = Vector3.negativeInfinity;
        if(value != null)
        {
            if (value.GetType() == typeof(GameObject))
            {
                GameObject gameObject = (GameObject)value;
                if(gameObject!=null)
                {
                    Position = gameObject.transform.position;
                }
            }
            if (value.GetType() == typeof(Vector3))
            {
                Position = (Vector3)value;
            }
        }
        return Position;
    }
}

