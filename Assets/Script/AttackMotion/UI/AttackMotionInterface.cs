using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AttackMotionInterface : MonoBehaviour
{
    [SerializeField]
    private RectTransform content;

    private int X_START;
    private int Y_START;
    //ui간의 얼마만큼 떨어져 있는지 한줄에 몇개가 들어가는지
    [SerializeField]
    private int X_SPACE_BETWWEN_ITEM;
    [SerializeField]
    private int Y_SPACE_BETWWEN_ITEMS;

    private AttackMotionTree attackMotionTree;
    [SerializeField]
    private AttackMotionView attackMotionView;
    private List<AttackMotionView> attackMotionList;

    public void Init(AttackMotionTree attackMotionTree)
    {
        this.attackMotionTree = attackMotionTree;
        attackMotionList = new();
        CreateView();
    }

    private void CreateView()
    {
        Debug.Log("오브젝트풀링을 사용하지 않는 생성");
        int x = attackMotionTree.AttackMotionTrees.Max(x => x.X);
        int y = attackMotionTree.AttackMotionTrees.Max(y => y.Y);

        //콘텐트의 크기조절
        SetContentSize(x, y + 1);


        for (int i = 0; i < attackMotionTree.AttackMotionTrees.Count; i++)
        {
            AttackMotionView view = Instantiate(attackMotionView, content != null ? content : transform);

            view.Init(attackMotionTree.AttackMotionTrees[i].AttackMotion);

            view.transform.localPosition = GetPosition(attackMotionTree.AttackMotionTrees[i].X, attackMotionTree.AttackMotionTrees[i].Y);

            attackMotionList.Add(view);
        }
    }

    private void SetContentSize(int x, int y)
    {
        if (content != null)
        {
            content.sizeDelta = new Vector2(500 - (x * X_SPACE_BETWWEN_ITEM), y * Y_SPACE_BETWWEN_ITEMS);
            X_START = 50;
            Y_START = y * Y_SPACE_BETWWEN_ITEMS + 75;
        }
    }
    //new Vector3(50 + x * 175, y * 175, 0)
    private Vector3 GetPosition(int x, int y)
    {
        return new Vector3(x * X_SPACE_BETWWEN_ITEM + X_START,
            y * Y_SPACE_BETWWEN_ITEMS - Y_START,
            0);
    }
}
