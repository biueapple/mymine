using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : Block , IBlockActive
{
    public Dirt(int id) : base(id, "흙", HardnessType.SHOVELS, 1, false, true, 5, new int[6] { 200, 200, 200, 200, 200, 200 })
    {
        
    }

    public void Active(Item item, World.BlockLaycast blockLaycast)
    {
        //만약 삽으로 우클릭 했다면 경작지로 변환
        if(item is Strength_Shovels shovels)
        {
            Debug.Log("삽으로 흙을 우클릭 경작지로 변환");
            World.Instance.WorldPositionEdit(blockLaycast.positionToInt, 5);
            if(item is IConsume consume)
            {
                consume.Consume();
                Debug.Log("남은 내구도 " + consume.Durability);
            }
        }
    }
}
