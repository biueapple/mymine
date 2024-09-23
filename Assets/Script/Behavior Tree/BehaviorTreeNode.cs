using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BehaviorTreeNode
{
    protected NodeState _nodeState;
    public NodeState NodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FALIERE,
}
