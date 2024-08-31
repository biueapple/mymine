
using UnityEngine;
using UnityEngine.Rendering.Universal;

// �� move�� ���� �ݹ�

//������ �����̷��� �Է��� �־����� ������ �ݹ�
public interface IMoveInputCallback
{
    void Move(Vector3 direction);
}
//������ �����̷��� �Է��� ��������� �������� ����ų �� �ִ��� ��� ������ ������ ����
public interface IMoveCallback
{
    void Move(Vector3 distance);
}
//������ �󸶳� ���������� ��� ������ ������ ����
public interface IMovePhysicsCallback
{
    void Move(Vector3 distance);
}

// �� move�� ���� �ݹ�

// �� ������� ���� �ݹ�

//������� ���� �Ǳ� ������ ������� ������Ű�� �ݹ�
public interface IDamageIncreaseCallback
{
    //���ݿ� ���� ����
    public void DamageIncrease(AttackInformation attackInformation);
}

//������� ���� �� �Ŀ� ȣ���ϴ� �ݹ� (����)
public interface IDamageAfterCallback
{
    //���ݿ� ���� ���� , ������ �� �����
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

// �� ������� ���� �ݹ�

// �� �׼� �õ��� ���� �ݹ�

//�⺻���� �õ� �� ȣ���ϴ� �ݹ�
public interface IAttackTryCallback
{
    public void AttackTry(Stat[] stats);
}

//��ų �õ� �� ȣ���ϴ� �ݹ�
public interface ISkillTryCallback
{
    public void SkillTry();
}

public interface ISkillEndCallback
{
    public void SkillEnd(Stat[] stats);
}

// �� �׼� �õ��� ���� �ݹ�

// �� ��Ÿ �ݹ��

