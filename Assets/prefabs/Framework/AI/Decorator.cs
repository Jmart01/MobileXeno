using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Decorator : BTNode
{
    BTNode _child;
    public BTNode Child { get { return _child; } }
    public Decorator(AIController aIController, BTNode child) : base(aIController)
    {
        _child = child;
        _child.Parent = this;
    }

    public override void Finish()
    {
        base.Finish();
        _child.Finish();
    }
}
