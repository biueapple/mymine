
using UnityEngine;

public class DestroyToProjectile : IProjectile
{
    readonly Player player;
    readonly GameObject projectile;

    public DestroyToProjectile(Player player, GameObject projectile)
    {
        this.player = player;
        this.projectile = projectile;
    }

    public void Trigger(Collider other)
    {
        //충돌은 이것을 사용
        if (other.TryGetComponent(out Substance substance))
        {
            if (substance.Unit == player)
                return;
            Debug.Log("풀링오브젝트를 사용하지 않는 destroy");
            GameObject.Destroy(projectile);
        }
    }

    public void Update() { }
}