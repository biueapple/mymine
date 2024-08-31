using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemSlotUI : ItemSlotUI
{
    [SerializeField]
    protected Equipment_Part[] parts;
    public Equipment_Part[] Parts { get { return parts; } }


    [SerializeField]
    protected bool input;
    public bool Input { get { return input; } }
    [SerializeField]
    protected bool output;
    public bool Output { get { return output; } }
}
