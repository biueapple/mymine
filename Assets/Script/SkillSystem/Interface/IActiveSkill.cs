//��Ƽ�� ��ų�� ������ �������̽�
using UnityEngine;

public interface IActiveSkill
{
    //���
    public void Use();
    public void Cancle();
    //��Ÿ��
    public float Cooltime { get; }
    public float Cooltimer { get; }
}