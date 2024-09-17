using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//불이 붙는 아이템
public interface IFlammable
{
    //몇초동안 불이 유지되는지
    public float Flammable { get; }
}
