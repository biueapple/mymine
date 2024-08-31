
using UnityEngine;

public interface IProjectile
{
    void Trigger(Collider other);
    void Update();
}
