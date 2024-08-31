using UnityEngine;

public class WalkStateShift : IStateShift
{
    public void Shift(Transform transform, ref Vector3 velocity)
    {
        velocity *= 3;
    }
}