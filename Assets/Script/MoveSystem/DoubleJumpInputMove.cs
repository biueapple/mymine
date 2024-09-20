using UnityEngine;

public class DoubleJumpInputMove : JumpInputMove , IInputMove
{
    readonly StateMachine _machine;
    readonly JumpInputMove _system;
    bool jump = false;
    float lastClickTime = 0f;
    readonly float delay = 0.2f;
    public DoubleJumpInputMove(StateMachine machine, JumpInputMove jumpSystem) : base(machine)
    {
        _machine = machine;
        _system = jumpSystem;
    }

    new public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!_system.IsJump)
            {
                _system.Jump(ref velocityMomemtum, 7);
            }
            else if(!jump)
            {
                if (Time.time - lastClickTime > delay)
                {
                    _system.Jump(ref velocityMomemtum, 7);
                    jump = true;
                    lastClickTime = Time.time;
                }
            }
        }
        if (_machine.isGround)
        {
            jump = false;
            _system.IsJump = false;
        }
    }
}
