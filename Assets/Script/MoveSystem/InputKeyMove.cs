using UnityEngine;

public class InputKeyMove : IInputMove
{
    private Stat stat;
    public InputKeyMove(Stat stat)
    { this.stat = stat; }

    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        velocity = ((transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"))).normalized * Time.deltaTime * stat.Speed;
    }
}