using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockOpenUI 
{
    /// <summary>
    /// ���� ��Ŭ�� �ߴ°�
    /// </summary>
    /// <param name="player"></param>
    public void OpenUI(Player player, World.BlockLaycast blockLaycast);
}
