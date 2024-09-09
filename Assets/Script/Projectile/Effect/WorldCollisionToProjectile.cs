using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ü�� ���忡 �����ϴ� ��ϰ� �浹ó���Ǵ� Ŭ����
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
                Debug.Log("Ǯ��������Ʈ�� ������� �ʴ� destroy");
                GameObject.Destroy(projectileObject.gameObject);
            }
            projectile.Update();
        }
    }
}
