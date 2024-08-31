//액티브 스킬이 가지는 인터페이스
using UnityEngine;

public interface IActiveSkill
{
    //사용
    public void Use();
    public void Cancle();
    //쿨타임
    public float Cooltime { get; }
    public float Cooltimer { get; }
}