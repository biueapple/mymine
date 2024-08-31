using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Block
{
    public Stone(int itemID) : base(itemID, "µ¹", HardnessType.PICKAX, 2, false, true, 3, new int[6] { 308, 308, 308, 308, 308, 308 })
    {

    }
}
