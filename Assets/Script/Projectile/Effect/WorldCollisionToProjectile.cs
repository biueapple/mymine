using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체가 월드에 존재하는 블록과 충돌처리되는 클래스
/// </summary>
public class WorldCollisionToProjectile : IProjectile
{
    readonly IProjectile projectile;
    readonly ProjectileObject projectileObject;

    public WorldCollisionToProjectile(ProjectileObject transform, IProjectile projectile = null)
    {
        this.projectile = projectile;
        projectileObject = transform;
    }

    public void Trigger(Collider other)
    {
        projectile?.Trigger(other);
    }

    public void Update()
    {
        if(projectile != null)
        {
            if(World.Instance.WorldPositionToBlock(GameManager.Instance.ChunkIndex(projectileObject.transform.position)).ID != 0)
            {
                Debug.Log("풀링오브젝트를 사용하지 않는 destroy");
                GameObject.Destroy(projectileObject.gameObject);
            }
            projectile.Update();
        }
    }
}
