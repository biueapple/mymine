using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public int ItemID { get; }
    public string Name { get; }
    public string Description { get; }
    public Item_Rating Item_Rating { get; }
    public bool Stackable { get; }
}
