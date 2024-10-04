using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldColleagueNode : BehaviorTreeNode
{
    private readonly Enemy[] colleague;
    private List<Cover> covers;
    public List<Cover> Covers => covers;

    public ShieldColleagueNode(Enemy[] colleague)
    {
        this.colleague = colleague;
    }

    public override NodeState Evaluate()
    {
        if (covers == null)
        {
            covers = new List<Cover>();
            for (int i = 0; i < colleague.Length; i++)
            {
                if (colleague[i].TryGetComponent(out Cover c))
                {
                    covers.Add(c);
                }
            }
        }
        else
        {
            for(int i = covers.Count - 1; i >= 0; i--)
            {
                if (!covers[i].gameObject.activeSelf)
                {
                    covers.RemoveAt(i);
                }
            }
        }

        if(covers.Count > 0)
            return NodeState.SUCCESS;

        return NodeState.FALIERE;
    }
}
