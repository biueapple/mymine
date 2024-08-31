using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotionTree
{
    private readonly List<AttackMotionTreeList> attackMotions = new ();
    public List<AttackMotionTreeList> AttackMotionTrees { get { return attackMotions; } }

    public AttackMotionTree(Player player)
    {
        attackMotions.Add(new (new HorizontalCutting(player), 0, 0));
        attackMotions.Add(new (new VerticalCutting(player), 1, 0));
    }

    public class AttackMotionTreeList
    {
        private readonly int x;
        public int X { get { return x; } }
        private readonly int y;
        public int Y { get { return y; } }
        private IAttackMotion attackMotion;
        public IAttackMotion AttackMotion { get { return attackMotion; } }
        //조건

        public AttackMotionTreeList(IAttackMotion attackMotion, int x, int y)
        {
            this.attackMotion = attackMotion;
            this.x = x;
            this.y = y;
        }
        //나중에 조건 생기면 만들기로
        public bool Condition()
        {
            return true;
        }
    }
}
