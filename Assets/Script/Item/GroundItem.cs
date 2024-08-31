using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    [SerializeField]
    private ItemSculpture itemSculpture;
    public ItemSculpture ItemSculpture { get { return itemSculpture; } set { itemSculpture = value; } }
}
