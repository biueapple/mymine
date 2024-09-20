using UnityEngine;

public class GravityForce : IExternalForce
{
    public bool Remove { get { return false; } }
    private float power = 0;
    private readonly float maxPower = 2.5f;
    private readonly StateMachine stateMachine;

    public GravityForce(StateMachine stateMachine)
    { this.stateMachine = stateMachine; }

    public void Force(Transform transform, ref Vector3 momemtum)
    {
        if (stateMachine.isGround)
            power = 1;
        else
        {
            power += Time.deltaTime;
            power = Mathf.Min(power, maxPower);
        }
            
        momemtum.y -= 9.8f * Time.deltaTime * power;
    }
}