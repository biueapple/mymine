using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockActive 
{
    /// <summary>
    /// 무엇을 들고 우클릭 했는가
    /// </summary>
    /// <param name="item"></param>
    public void Active(Item item, World.BlockLaycast blockLaycast);
}
