
using UnityEngine;

public class MoveToProjectile : IProjectile
{
    readonly IProjectile projectile;
    readonly Transform tf;
    readonly float speed = 1;

    public MoveToProjectile(Transform transform, float speed, IProjectile projectile = null)
    {
        tf = transform;
        this.speed = speed;
        this.projectile = projectile;
    }

    public void Trigger(Collider other)
    {
        projectile?.Trigger(other);
    }

    public void Update()
    {
        tf.position = Vector3.MoveTowards(tf.position, tf.position + tf.forward, Time.deltaTime * speed);
        projectile?.Update();
    }
}