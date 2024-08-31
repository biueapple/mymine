using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedrock : Block
{
    public Bedrock(int id) : base(id, "¹èµå¶ô", HardnessType.NONE, 0, false, true, 0, new int[6] { 100,100,100,100,100,100 })
    {

    }
}
