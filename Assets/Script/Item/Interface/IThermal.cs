using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ �޴� ������
public interface IThermal
{
    //������ �޴� �ð�
    public float Thermal { get; }
    //������ ����� ����
    public Item Done();
}
