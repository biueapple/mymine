using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSetting
{
    //���콺 ����
    [SerializeField]
    public float verticalSpeed = 1;
    [SerializeField]
    public float horizontalSpeed = 1;
    [SerializeField]
    public KeyCode inventory = KeyCode.I;
    [SerializeField]
    public KeyCode equip = KeyCode.E;
    [SerializeField]
    public KeyCode skill = KeyCode.K;
    [SerializeField]
    public KeyCode mode = KeyCode.B;
    [SerializeField]
    public KeyCode motion = KeyCode.L;
}
