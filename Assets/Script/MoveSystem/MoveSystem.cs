using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveSystem : MonoBehaviour
{
    //�������� �Է¿� ���� o
    //�������� �Է��� �ִٰ� �ص� ������ �� ���� ��Ȳ o
    //�������� ���� x
    //�����ӿ� ���� �ݹ� x
    [SerializeField]
    private float _weight = 3;
    public float Weight { get => _weight; set => _weight = value; }

    //Ŭ�������� ��ȭ��ų ��� (�Է¿� ���� �������� �ٷ� �����)
    [SerializeField]
    private Vector3 _velocity;
    //�߰��� ����Ǵ� �� (�Է��� ������ �������� ������ ����ǵ���)
    [SerializeField]
    private Vector3 _velocityMomemtum;
    //���� ����
    [SerializeField]
    private StateMachine _machine = new ();
    public StateMachine Machine { get => _machine; }
    //�����̷��� �Է��� �־�����
    private bool _input;
    public bool Input { get { return _input; } }

    //����������
    private bool _moving;
    public bool Moving { get { return _moving; } }

    /// <summary>
    /// �����̷��� �Է��� �־�����
    /// </summary>
    private readonly List<IMoveInputCallback> _inputCallback = new ();
    public List<IMoveInputCallback> InputCallbacks { get { return _inputCallback; } }
    //public IMoveInputCallback InputCallback 
    //{ set
    //    {
    //        if (_inputCallback.Contains(value))
    //            _inputCallback.Remove(value);
    //        else
    //            _inputCallback.Add(value);
    //    }
    //}
    /// <summary>
    /// �� �Է��� ������ ���� �������� �� �� �־�����
    /// </summary>
    private readonly List<IMoveCallback> _moveCallback = new();
    public List<IMoveCallback> MoveCallbacks { get { return _moveCallback; } }
    //public IMoveCallback MoveCallback
    //{
    //    set
    //    {
    //        if (_moveCallback.Contains(value))
    //            _moveCallback.Remove(value);
    //        else
    //            _moveCallback.Add(value);
    //    }
    //}
    /// <summary>
    /// ������ �������� ���� ������
    /// </summary>
    private readonly List<IMovePhysicsCallback> _movePhysicsCallback = new ();
    public List<IMovePhysicsCallback> MovePhysicsCallbacks { get { return _movePhysicsCallback; } }
    //public IMovePhysicsCallback MovePhysicsCallback
    //{
    //    set
    //    {
    //        if (_movePhysicsCallback.Contains(value))
    //            _movePhysicsCallback.Remove(value);
    //        else
    //            _movePhysicsCallback.Add(value);
    //    }
    //}
    //�� �ܺ����� ���ܰ谡 1�ܰ迴���� �𸣰���

    //�Է´ܰ� (�����ӿ� ���� �Է��� �޴� �ܰ� {velocity})                                                   �÷��̾ �����̱� ���� Ű�� ����
    private readonly List<IInputMove> inputMoves = new();
    //�Է� ���� �ܰ� (�����ӿ� ���� �Է��� �����ϰų� �߰� �ϴ� �ܰ� {������ �� �� ���� ����} [velocity] )     �÷��̾ ���� Ű�߿� �����ϰų� ��ȭ �� �������� ����
    private readonly List<IInputLimit> inputLimit = new();
    //���¿� ���� �Է� ��ȭ�ܰ� (�������� ��ȭ {�޸���, ����} {velocity})                                     �÷��̾ ���� Ű�� � ��ȭ�� ����Ű����
    private readonly List<IStateShift> stateShifts = new();
    //�ڽ��� �ǵ��� �����̴°��� �ٷ� ���� (velocity) �ڽ��� �ǵ��� �ƴѰ��� (momemtum)
    //�ܺ����� �� �ܰ� (�з���, �߷� {momemtum})                                                            �÷��̾��� �Է°��� ��� ���� �߰��� �������� ��
    private readonly List<IExternalForce> externalForces = new();
    //������ �Ѱ� �ܰ� (���� �΋H�� {velocity})                                                             ��� ���� ���Ͽ� ���������� �Ǵ�
    private readonly List<IPhysicsShift> physicsShifts = new();
    //������ (������ ������ {velocity * momemtum})                                                          ������

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _velocity = Vector3.zero;

        //�Է´ܰ�
        inputMoves.ForEach(moveSystem => { moveSystem.InputMove(transform, ref _velocity, ref _velocityMomemtum); });

        //�Է� ��ȭ �ܰ�
        inputLimit.ForEach(moveSystem => { moveSystem.InputLimit(transform, ref _velocity, ref _velocityMomemtum); });
        //�Է� �ݹ�
        if(!_input && _velocity != Vector3.zero)
        {
            _input = true;
            //�����̷��� �Է��� �־����� �ݹ� �ѹ�
            _inputCallback.ForEach(callback => { callback.Move(_velocity); });
        }
        else if(_input && _velocity == Vector3.zero)
        {
            _input = false;
            //�����̷��� �Է��� �־����� �ݹ� �ѹ� ����
        }

        //�ӵ� ��ȭ �ܰ�
        stateShifts.ForEach(moveSystem => { moveSystem.Shift(transform, ref _velocity); });
        //�ӵ� �ݹ�
        if(_velocity != Vector3.zero)
        {
            //�󸶳� �����̷��� �ߴ��� ��� �ݹ�
            _moveCallback.ForEach(callback => callback.Move(_velocity));
        }

        //�ܺ��� ���� �ܰ�
        externalForces.ForEach(moveSystem => { moveSystem.Force(transform, ref _velocityMomemtum); });
        externalForces.RemoveAll(moveSystem => moveSystem.Remove == true);

        //������ �Ѱ� �ܰ�
        physicsShifts.ForEach(moveSystem => { moveSystem.Shift(transform, ref _velocity, ref _velocityMomemtum); });

        //���� ������ �� �ݹ�
        if (/*!_moving && */_velocity != Vector3.zero)
        {
            _moving = true;
            //������ ������ ������ �ݹ�
            _movePhysicsCallback.ForEach(callback => callback.MovePhysics(_velocity));
        }
        else if(_moving && _velocity == Vector3.zero)
        {
            _moving = false;
            //������ ������ ������ �ݹ� �ѹ� ����
        }

        //������
        Move();

        //���ӵ� �پ��
        if(_machine.isGround)
        {
            _velocityMomemtum -= Time.deltaTime * _weight * _velocityMomemtum;
        }
        else
        {
            _velocityMomemtum -=  Time.deltaTime * _velocityMomemtum;
        }

        if (_velocityMomemtum.x <= 0.1f && _velocityMomemtum.x >= -0.1f)
            _velocityMomemtum.x = 0;
        if (_velocityMomemtum.z <= 0.1f && _velocityMomemtum.z >= -0.1f)
            _velocityMomemtum.z = 0;
    }

    private void Move()
    {
        _velocity += _velocityMomemtum * Time.deltaTime;
        transform.Translate(_velocity, Space.World);
    }

    //���� ����� ���� �Լ���

    /// <summary>
    /// ���� �ѹ� �ش�
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="power">��</param>
    public void AddExternalForces(Vector3 power)
    {
        _velocityMomemtum += power;
    }
    /// <summary>
    /// �ٴڿ� ������ ������� ���� �ѹ� �ش�
    /// </summary>
    /// <param name="power"></param>
    public void AddExternalForcesGround(Vector3 power)
    {
        externalForces.Add(new GroindExternal(power, ref _velocityMomemtum, _machine));
    }
    /// <summary>
    /// �������� ���� �ش�
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="power">��</param>
    public IExternalForce AddExternalPower(Vector3 power)
    {
        IExternalForce externalForce = new ContinuedExternal(power);
        externalForces.Add(externalForce);
        return externalForce;
    }
    /// <summary>
    /// �������� ���� ���ش�
    /// </summary>
    /// <param name="externalForce">AddExternalPower �� ���� ��</param>
    public void RemoveExternalPower(IExternalForce externalForce)
    {
        externalForces.Remove(externalForce);
    }
    /// <summary>
    /// �������� ���� �ش�
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="power">��</param>
    public void AddExternalPower(Vector3 power, float timer)
    {
        externalForces.Add(new TimerExternal(power, timer));
    }

    /// <summary>
    /// ������ �ʴ� �ӹ��� �Ǵ� (���� ������� �Ѵ� {RemoveMoveMode})
    /// </summary>
    public IInputLimit ShackleTimer()
    {
        Shackle shackle = new ();
        inputLimit.Add(shackle);
        return shackle;
    }
    /// <summary>
    /// �����ð� ���� �Է¿� ���� �������� ���ش�
    /// </summary>
    /// <param name="timer">���� �ð�</param>
    public void ShackleTimer(float timer)
    {
        Shackle shackle = new ();
        inputLimit.Add(shackle);
        StartCoroutine(ShackleCoroutine(shackle, timer));
    }
    private IEnumerator ShackleCoroutine(Shackle shackle, float timer)
    {
        yield return new WaitForSeconds(timer);
        RemoveMoveMode(shackle);
    }

    /// <summary>
    /// ������ �ʴ� ���ο츦 �Ǵ� (���� ������� �Ѵ� {RemoveMoveMode})
    /// </summary>
    public SlowShift Slow(float power)
    {
        SlowShift slow = new (power);
        stateShifts.Add(slow);
        return slow;
    }
    /// <summary>
    /// �����ð� ���� �̵��� ���� �������� �����
    /// </summary>
    /// <param name="timer">������ �ð�</param>
    public void SlowTimer(float power, float timer)
    {
        SlowShift slow = new (power);
        stateShifts.Add(slow);
        StartCoroutine(SlowCoroutine(slow, timer));
    }
    private IEnumerator SlowCoroutine(SlowShift slow, float timer)
    {
        yield return new WaitForSeconds(timer);
        stateShifts.Remove(slow);
    }

    public T FindMoveMode<T>() where T : class
    {
        for(int i = 0; i < externalForces.Count; i++)
            if (externalForces[i] is T external)
                return external;
        for(int i = 0; i < inputMoves.Count; i++)
            if (inputMoves[i] is T input)
                return input;
        for (int i = 0; i < inputLimit.Count; i++)
            if (inputLimit[i] is T limit)
                return limit;
        for (int i = 0; i < stateShifts.Count; i++)
            if (stateShifts[i] is T state)
                return state;
        for (int i = 0; i < physicsShifts.Count; i++)
            if (physicsShifts[i] is T physics)
                return physics;
        return null;
    }
    public void AddMoveMode(IExternalForce externalForce)
    {
        externalForces.Add(externalForce);
    }
    public void AddMoveMode(IInputMove inputMove)
    {
        inputMoves.Add(inputMove);
    }
    public void AddMoveMode(IInputLimit inputLimit)
    {
        this.inputLimit.Add(inputLimit);
    }
    public void AddMoveMode(IStateShift stateShift)
    {
        stateShifts.Add(stateShift);
    }
    public void AddMoveMode(IPhysicsShift physicsShift)
    {
        physicsShifts.Add(physicsShift);
    }
    public void RemoveMoveMode(IExternalForce externalForce)
    {
        externalForces.Remove(externalForce);
    }
    public void RemoveMoveMode(IInputMove inputMove)
    {
        inputMoves.Remove(inputMove);
    }
    public void RemoveMoveMode(IInputLimit inputLimit)
    {
        this.inputLimit.Remove(inputLimit);
    }
    public void RemoveMoveMode(IStateShift stateShift)
    {
        stateShifts.Remove(stateShift);
    }
    public void RemoveMoveMode(IPhysicsShift physicsShift)
    {
        physicsShifts.Remove(physicsShift);
    }
}

//�Է´ܰ� (�����ӿ� ���� �Է��� �޴� �ܰ� {velocity})
public interface IInputMove
{
    void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum);
}

public class AutoJump : IInputMove
{
    readonly JumpInputMove inputMove;
    readonly Unit unit;
    public AutoJump(JumpInputMove inputMove, Unit unit)
    {
        this.inputMove = inputMove;
        this.unit = unit;
    }

    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        Vector3 dir = new Vector3(velocity.x, 0, velocity.z);

        if (inputMove.Machine.isGround)
            inputMove.isJump = false;

        if (!Empty(transform.position + dir))
        {
            if (inputMove != null && !inputMove.isJump)
                inputMove.Jump(ref velocityMomemtum);
        }
    }

    private bool Empty(Vector3 position)
    {
        //���� �Ÿ�
        float distanceX = unit.Width * 2;
        //���� �߰��Ǵ� ��
        float width = -unit.Width;
        //���� �󸶳� �߰������
        float measureX;
        //x��
        while (true)
        {
            float distanceZ = unit.Depth * 2;
            float depth = -unit.Depth;
            float measureZ;
            //z��
            while (true)
            {
                //�浹���
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y, position.z + depth)))
                {
                    //����
                    return false;
                }
                //���� �Ÿ��� ���ٸ�
                if (distanceZ <= 0)
                {
                    break;
                }
                //���� �Ÿ� ���̱�
                measureZ = Mathf.Min(1, distanceZ);
                depth += measureZ;
                distanceZ -= measureZ;
            }

            if (distanceX <= 0)
            {
                break;
            }

            measureX = Mathf.Min(1, distanceX);
            width += measureX;
            distanceX -= measureX;
        }
        return true;
    }
}
public class AutoMove : IInputMove
{
    readonly Unit unit;
    List<Vector3> points;
    public List<Vector3> Points { get { return points; } }

    public AutoMove(Unit unit)
    {
        this.unit = unit;
    }

    public void SetTartget(List<Vector3> points)
    {
        this.points = points;
    }

    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        if(points != null && points.Count > 0)
        {
            if (Vector3.Distance(unit.transform.position, points[0]) < 0.2f)
            {
                points.RemoveAt(0);
            }
            else
            {
                Vector3 vectpr = (points[0] - unit.transform.position);
                velocity = new Vector3(vectpr.x, 0, vectpr.z).normalized * Time.deltaTime * unit.STAT.Speed;
            }
        }
    }
}

//�Է� ���� �ܰ� (�����ӿ� ���� �Է��� �����ϰų� �߰� �ϴ� �ܰ� {������ �� �� ���� ����} [velocity])
public interface IInputLimit
{
    void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum);
}
//test�� (�����θ� �� �� ����) �۵� Ȯ��
public class Provoke1 : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        // �̵� ���Ͱ� transform.forward ������ ���� ����
        if (velocity != Vector3.zero && Vector3.Dot(velocity.normalized, transform.forward) > 0.5f)
        {
            velocity = transform.forward * velocity.magnitude;
        }
        else
        {
            velocity = Vector3.zero;
        }
    }
}
//test�� (��� Ű�� ������ ���� Ű�� ��) �۵� Ȯ��
public class Provoke2 : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        velocity = transform.forward * velocity.magnitude;
    }
}
//test�� (�Է����� ���� �������� ����)
public class Shackle : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        velocity = Vector3.zero;
    }
}

//���¿� ���� �ӵ� ��ȭ�ܰ� (�������� ��ȭ {�޸���} {velocity})
public interface IStateShift
{
    void Shift(Transform transform, ref Vector3 velocity);
}
public class SlowShift : IStateShift
{
    private readonly float multiple;
    public SlowShift(float multiple)
    {
        this.multiple = Mathf.Min(multiple, 1);
    }
    public void Shift(Transform transform, ref Vector3 velocity)
    {
        velocity *= multiple;
    }
}


//�ܺ����� �� �ܰ� (�з���, �߷� {momemtum})
public interface IExternalForce
{
    bool Remove { get; }
    void Force(Transform transform, ref Vector3 momemtum);
}
//������ ���� �� ���� (��ǳ)
//���� ���� �� ���� (�߷�)

public class GroindExternal : IExternalForce
{
    private bool remove = false;
    public bool Remove => remove;
    private Vector3 velocity;
    private readonly StateMachine state;
    public GroindExternal(Vector3 power, ref Vector3 momentum, StateMachine stateMachine)
    {
        velocity = power;
        momentum += power;
        state = stateMachine;
        state.isGround = false;
    }

    public void Force(Transform transform, ref Vector3 momemtum)
    {
        if (state.isGround && momemtum.y < 0)
        {
            remove = true;
            momemtum -= Vector3.Min(velocity, momemtum);
        }
        else
        {
            velocity -= Time.deltaTime * momemtum;
        }
    }
}
public class TimerExternal : IExternalForce
{
    readonly Vector3 dir;
    float timer;
    public bool Remove { 
        get
        {
            if(timer <= 0)
                return true;
            else
                return false;
        }
    }

    public TimerExternal(Vector3 dir, float timer)
    {
        this.dir = dir;
        this.timer = timer;
    }
    public void Force(Transform transform, ref Vector3 momemtum)
    {
        momemtum += Time.deltaTime  * dir;
        timer -= Time.deltaTime;
    }
}
public class ContinuedExternal : IExternalForce
{
    readonly Vector3 dir;
    public bool Remove { get { return false; } }

    public ContinuedExternal(Vector3 dir)
    {
        this.dir = dir;
    }
    public void Force(Transform transform, ref Vector3 momemtum)
    {
        momemtum += Time.deltaTime * dir;
    }
}


//������ �Ѱ� �ܰ� (���� �΋H�� {velocity})
public interface IPhysicsShift
{
    void Shift(Transform transform, ref Vector3 velocity, ref Vector3 momemtum);
}

//������ (������ ������ {velocity * momemtum})
[Serializable]
public class StateMachine
{
    public StateMachine()
    {
        isGround = true;
        isAir = false;
    }
    public enum STATE
    {

    }
    //public STATE state;
    public bool isGround;
    public bool isAir;
}