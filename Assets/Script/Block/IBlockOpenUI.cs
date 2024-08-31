using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockOpenUI 
{
    /// <summary>
    /// 누가 우클릭 했는가
    /// </summary>
    /// <param name="player"></param>
    public void OpenUI(Player player, World.BlockLaycast blockLaycast);
}
