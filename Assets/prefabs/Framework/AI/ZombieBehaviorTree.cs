using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ZombieBehaviorTree : BehaviorTree
{
    public override void Init(AIController aiController)
    {
        base.Init(aiController);
        AddBlackBoardKey("Target");
        AddBlackBoardKey("CheckLocation");
        Selector RootSelector = new Selector(AIC);
            Sequence MoveToAndAttackTargetSeq = new Sequence(aiController);
                BTTask_MoveTo MoveToPlayer = new BTTask_MoveTo(aiController, "Target", 1.5f);
                BlackboardDecorator MoveToPlayerDecortor = new BlackboardDecorator(aiController, MoveToPlayer, "Target", EKeyQuery.Set, EUpdateAbort.Both);
                MoveToAndAttackTargetSeq.AddChild(MoveToPlayerDecortor);
                BTTask_AttackTarget AttackPlayer = new BTTask_AttackTarget(aiController, "Target");
                MoveToAndAttackTargetSeq.AddChild(AttackPlayer);
        RootSelector.AddChild(MoveToAndAttackTargetSeq);

            Sequence MoveToCheckLocationSeq = new Sequence(AIC);
                BTTask_MoveTo MoveToCheckLoc = new BTTask_MoveTo(AIC, "CheckLocation", 2f);
                BlackboardDecorator MoveToCheckLocecorator = new BlackboardDecorator(aiController, MoveToCheckLoc, "CheckLocation", EKeyQuery.Set, EUpdateAbort.Both);
                MoveToCheckLocationSeq.AddChild(MoveToCheckLocecorator);
                BTTask_Wait waitAbit = new BTTask_Wait(aiController, 2f);
                MoveToCheckLocationSeq.AddChild(waitAbit);
                BTTask_ClearBlackboardKey clearCheckLocation = new BTTask_ClearBlackboardKey(aiController, "CheckLocation");
                MoveToCheckLocationSeq.AddChild(clearCheckLocation);
        RootSelector.AddChild(MoveToCheckLocationSeq);

            Sequence PatrollingSequence = new Sequence(AIC);
                PatrollingSequence.AddChild(new BTTask_MoveToNextPatrolPoint(AIC, 1f));
                PatrollingSequence.AddChild(new BTTask_Wait(AIC, 4));
        RootSelector.AddChild(PatrollingSequence);

        SetRootNode(RootSelector);
    }
}
