
using UnityEngine;
using UnityEngine.Rendering.Universal;

// ↓ move에 관한 콜백

//실제로 움직이려는 입력이 있었을때 방향을 콜백
public interface IMoveInputCallback
{
    void Move(Vector3 direction);
}
//실제로 움직이려는 입력이 어느정도의 움직임을 일으킬 수 있는지 양과 방향을 가지고 리턴
public interface IMoveCallback
{
    void Move(Vector3 distance);
}
//실제로 얼마나 움직였는지 양과 방향을 가지고 리턴
public interface IMovePhysicsCallback
{
    void Move(Vector3 distance);
}

// ↑ move에 관한 콜백

// ↓ 대미지에 관한 콜백

//대미지가 적용 되기 직전에 대미지를 증가시키는 콜백
public interface IDamageIncreaseCallback
{
    //공격에 대한 정보
    public void DamageIncrease(AttackInformation attackInformation);
}

//대미지가 적용 된 후에 호출하는 콜백 (흡혈)
public interface IDamageAfterCallback
{
    //공격에 대한 정보 , 실제로 들어간 대미지
    public void DamageAfterCall(AttackInformation attackInformation, float damage);
}

public interface IHitBeforeCallback
{
    public void HitBefore(AttackInformation attackInformation);
}

public interface IHitAfterCallback
{
    public void HitAfter(AttackInformation attackInformation);
}

// ↑ 대미지에 관한 콜백

// ↓ 액션 시도에 관한 콜백

//기본공격 시도 시 호출하는 콜백
public interface IAttackTryCallback
{
    public void AttackTry(Stat[] stats);
}

//스킬 시도 시 호출하는 콜백
public interface ISkillTryCallback
{
    public void SkillTry();
}

public interface ISkillEndCallback
{
    public void SkillEnd(Stat[] stats);
}

// ↑ 액션 시도에 관한 콜백

// ↓ 기타 콜백들

