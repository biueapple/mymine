using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveSystem : MonoBehaviour
{
    //움직임의 입력에 대해 o
    //움직임의 입력이 있다고 해도 움직일 수 없는 상황 o
    //움직임의 조건 x
    //움직임에 대한 콜백 x
    [SerializeField]
    private float _weight = 3;
    public float Weight { get => _weight; set => _weight = value; }

    //클래스들이 변화시킬 운동량 (입력에 대한 움직임은 바로 적용됨)
    [SerializeField]
    private Vector3 _velocity;
    //추가로 적용되는 힘 (입력을 제외한 움직임은 서서히 적용되도록)
    [SerializeField]
    private Vector3 _velocityMomemtum;
    //현재 상태
    [SerializeField]
    private StateMachine _machine = new ();
    public StateMachine Machine { get => _machine; }
    //움직이려는 입력이 있었는지
    private bool _input;
    public bool Input { get { return _input; } }

    //움직였는지
    private bool _moving;
    public bool Moving { get { return _moving; } }

    /// <summary>
    /// 움직이려는 입력이 있었는지
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
    /// 그 입력이 실제로 얼마의 움직임을 낼 수 있었는지
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
    /// 실제로 움직여진 양이 얼마인지
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
    //왜 외부적인 힘단계가 1단계였는지 모르겠음

    //입력단계 (움직임에 대한 입력을 받는 단계 {velocity})                                                   플레이어가 움직이기 위해 키를 누름
    private readonly List<IInputMove> inputMoves = new();
    //입력 제한 단계 (움직임에 대한 입력중 제거하거나 추가 하는 단계 {앞으로 갈 수 없는 상태} [velocity] )     플레이어가 누른 키중에 무시하거나 변화 할 움직임이 있음
    private readonly List<IInputLimit> inputLimit = new();
    //상태에 따른 입력 변화단계 (움직임의 변화 {달리기, 기절} {velocity})                                     플레이어가 누른 키가 어떤 변화를 일으키는지
    private readonly List<IStateShift> stateShifts = new();
    //자신의 의도로 움직이는것은 바로 적용 (velocity) 자신의 의도가 아닌것은 (momemtum)
    //외부적인 힘 단계 (밀려남, 중력 {momemtum})                                                            플레이어의 입력과는 상관 없이 추가로 가해지는 힘
    private readonly List<IExternalForce> externalForces = new();
    //물리적 한계 단계 (벽에 부딫힘 {velocity})                                                             모든 힘에 대하여 가능한지의 판단
    private readonly List<IPhysicsShift> physicsShifts = new();
    //움직임 (실제로 움직임 {velocity * momemtum})                                                          움직임

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _velocity = Vector3.zero;

        //입력단계
        inputMoves.ForEach(moveSystem => { moveSystem.InputMove(transform, ref _velocity, ref _velocityMomemtum); });

        //입력 변화 단계
        inputLimit.ForEach(moveSystem => { moveSystem.InputLimit(transform, ref _velocity, ref _velocityMomemtum); });
        //입력 콜백
        if(!_input && _velocity != Vector3.zero)
        {
            _input = true;
            //움직이려는 입력이 있었는지 콜백 한번
            _inputCallback.ForEach(callback => { callback.Move(_velocity); });
        }
        else if(_input && _velocity == Vector3.zero)
        {
            _input = false;
            //움직이려는 입력이 있었는지 콜백 한번 없음
        }

        //속도 변화 단계
        stateShifts.ForEach(moveSystem => { moveSystem.Shift(transform, ref _velocity); });
        //속도 콜백
        if(_velocity != Vector3.zero)
        {
            //얼마나 움직이려고 했는지 계속 콜백
            _moveCallback.ForEach(callback => callback.Move(_velocity));
        }

        //외부힘 적용 단계
        externalForces.ForEach(moveSystem => { moveSystem.Force(transform, ref _velocityMomemtum); });
        externalForces.RemoveAll(moveSystem => moveSystem.Remove == true);

        //물리적 한계 단계
        physicsShifts.ForEach(moveSystem => { moveSystem.Shift(transform, ref _velocity, ref _velocityMomemtum); });

        //실제 움직일 양 콜백
        if (/*!_moving && */_velocity != Vector3.zero)
        {
            _moving = true;
            //실제로 움직여 졌는지 콜백
            _movePhysicsCallback.ForEach(callback => callback.MovePhysics(_velocity));
        }
        else if(_moving && _velocity == Vector3.zero)
        {
            _moving = false;
            //실제로 움직여 졌는지 콜백 한번 없음
        }

        //움직임
        Move();

        //가속도 줄어듬
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

    //쉬운 사용을 위한 함수들

    /// <summary>
    /// 힘을 한번 준다
    /// </summary>
    /// <param name="dir">방향</param>
    /// <param name="power">힘</param>
    public void AddExternalForces(Vector3 power)
    {
        _velocityMomemtum += power;
    }
    /// <summary>
    /// 바닥에 닿으면 사라지는 힘을 한번 준다
    /// </summary>
    /// <param name="power"></param>
    public void AddExternalForcesGround(Vector3 power)
    {
        externalForces.Add(new GroindExternal(power, ref _velocityMomemtum, _machine));
    }
    /// <summary>
    /// 지속적인 힘을 준다
    /// </summary>
    /// <param name="dir">방향</param>
    /// <param name="power">힘</param>
    public IExternalForce AddExternalPower(Vector3 power)
    {
        IExternalForce externalForce = new ContinuedExternal(power);
        externalForces.Add(externalForce);
        return externalForce;
    }
    /// <summary>
    /// 지속적인 힘을 없앤다
    /// </summary>
    /// <param name="externalForce">AddExternalPower 로 받은 힘</param>
    public void RemoveExternalPower(IExternalForce externalForce)
    {
        externalForces.Remove(externalForce);
    }
    /// <summary>
    /// 지속적인 힘을 준다
    /// </summary>
    /// <param name="dir">방향</param>
    /// <param name="power">힘</param>
    public void AddExternalPower(Vector3 power, float timer)
    {
        externalForces.Add(new TimerExternal(power, timer));
    }

    /// <summary>
    /// 끝나지 않는 속박을 건다 (따로 없애줘야 한다 {RemoveMoveMode})
    /// </summary>
    public IInputLimit ShackleTimer()
    {
        Shackle shackle = new ();
        inputLimit.Add(shackle);
        return shackle;
    }
    /// <summary>
    /// 일정시간 동안 입력에 대한 움직임을 없앤다
    /// </summary>
    /// <param name="timer">멈출 시간</param>
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
    /// 끝나지 않는 슬로우를 건다 (따로 없애줘야 한다 {RemoveMoveMode})
    /// </summary>
    public SlowShift Slow(float power)
    {
        SlowShift slow = new (power);
        stateShifts.Add(slow);
        return slow;
    }
    /// <summary>
    /// 일정시간 동안 이동에 대한 움직임을 늦춘다
    /// </summary>
    /// <param name="timer">느려질 시간</param>
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

//입력단계 (움직임에 대한 입력을 받는 단계 {velocity})
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
        //남은 거리
        float distanceX = unit.Width * 2;
        //최종 추가되는 값
        float width = -unit.Width;
        //현재 얼마나 추가됬는지
        float measureX;
        //x축
        while (true)
        {
            float distanceZ = unit.Depth * 2;
            float depth = -unit.Depth;
            float measureZ;
            //z축
            while (true)
            {
                //충돌계산
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y, position.z + depth)))
                {
                    //점프
                    return false;
                }
                //남은 거리가 없다면
                if (distanceZ <= 0)
                {
                    break;
                }
                //남은 거리 줄이기
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

//입력 제한 단계 (움직임에 대한 입력중 제거하거나 추가 하는 단계 {앞으로 갈 수 없는 상태} [velocity])
public interface IInputLimit
{
    void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum);
}
//test용 (앞으로만 갈 수 있음) 작동 확인
public class Provoke1 : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        // 이동 벡터가 transform.forward 방향일 때만 유지
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
//test용 (모든 키가 앞으로 가는 키가 됨) 작동 확인
public class Provoke2 : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        velocity = transform.forward * velocity.magnitude;
    }
}
//test용 (입력으로 인한 움직임을 멈춤)
public class Shackle : IInputLimit
{
    public void InputLimit(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        velocity = Vector3.zero;
    }
}

//상태에 따른 속도 변화단계 (움직임의 변화 {달리기} {velocity})
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


//외부적인 힘 단계 (밀려남, 중력 {momemtum})
public interface IExternalForce
{
    bool Remove { get; }
    void Force(Transform transform, ref Vector3 momemtum);
}
//지속적 같은 힘 적용 (태풍)
//점점 가속 힘 적용 (중력)

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


//물리적 한계 단계 (벽에 부딫힘 {velocity})
public interface IPhysicsShift
{
    void Shift(Transform transform, ref Vector3 velocity, ref Vector3 momemtum);
}

//움직임 (실제로 움직임 {velocity * momemtum})
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