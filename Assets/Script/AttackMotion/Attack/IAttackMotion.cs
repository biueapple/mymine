using UnityEngine;

public interface IAttackMotion
{
    public string Name { get; }
    public Sprite Icon { get; }
    /// <summary>
    /// 공격이 초기화 될때까지의 시간
    /// </summary>
    public float Timelimit { get; }
    /// <summary>
    /// 다음 공격이 가능할때까지의 시간
    /// </summary>
    public float Delay { get; }
    public Vector3 Position { get; }
    public Vector3 Range { get; }
    public float Multiple { get; }
    public void Motion();
}
