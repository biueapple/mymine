using UnityEngine;

public interface IAttackMotion
{
    public string Name { get; }
    public Sprite Icon { get; }
    /// <summary>
    /// ������ �ʱ�ȭ �ɶ������� �ð�
    /// </summary>
    public float Timelimit { get; }
    /// <summary>
    /// ���� ������ �����Ҷ������� �ð�
    /// </summary>
    public float Delay { get; }
    public Vector3 Position { get; }
    public Vector3 Range { get; }
    public float Multiple { get; }
    public void Motion();
}
