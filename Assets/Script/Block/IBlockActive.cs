using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockActive 
{
    /// <summary>
    /// ������ ��� ��Ŭ�� �ߴ°�
    /// </summary>
    /// <param name="item"></param>
    public void Active(Item item, World.BlockLaycast blockLaycast);
}
