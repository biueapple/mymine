
using UnityEngine;

public class RunStateShift : IStateShift
{
    public void Shift(Transform transform, ref Vector3 velocity)
    {
        velocity *= 2;
    }
}