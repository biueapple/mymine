using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : Block , IBlockActive
{
    public Dirt(int id) : base(id, "��", HardnessType.SHOVELS, 1, false, true, 5, new int[6] { 200, 200, 200, 200, 200, 200 })
    {
        
    }

    public void Active(Item item, World.BlockLaycast blockLaycast)
    {
        //���� ������ ��Ŭ�� �ߴٸ� �������� ��ȯ
        if(item is Strength_Shovels shovels)
        {
            Debug.Log("������ ���� ��Ŭ�� �������� ��ȯ");
            World.Instance.WorldPositionEdit(blockLaycast.positionToInt, 5);
            if(item is IConsume consume)
            {
                consume.Consume();
                Debug.Log("���� ������ " + consume.Durability);
            }
        }
    }
}
