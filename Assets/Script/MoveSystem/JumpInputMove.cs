using UnityEngine;

[System.Serializable]
public class JumpInputMove : IInputMove
{
    readonly StateMachine _machine;
    public StateMachine Machine { get { return _machine; } }
    [SerializeField]
    private bool jump = false;
    public bool IsJump { get { return jump; } set { jump = value; } }
    public JumpInputMove(StateMachine machine)
    {
        _machine = machine;
    }
    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!jump)
            {
                Jump(ref velocityMomemtum);
            }
        }
        if (_machine.isGround)
            jump = false;
    }
    public void Jump(ref Vector3 velocityMomemtum)
    {
        velocityMomemtum.y = 9;
        jump = true;
        _machine.isAir = true;
        _machine.isGround = false;
    }
    public void Jump(ref Vector3 velocityMomemtum, float power)
    {
        velocityMomemtum.y = power;
        jump = true;
        _machine.isAir = true;
        _machine.isGround = false;
    }
}