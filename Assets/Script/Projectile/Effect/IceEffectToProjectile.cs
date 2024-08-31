using UnityEngine;

public class IceEffectToProjectile : IProjectile
{
    readonly IProjectile projectile;
    readonly Player player;

    public IceEffectToProjectile(Player player, IProjectile projectile = null)
    {
        this.player = player;
        this.projectile = projectile;
    }

    public void Trigger(Collider other)
    {
        //충돌은 이것을 사용
        Unit unit = other.GetComponent<Unit>();
        if (unit == null)
        {
            unit = other.GetComponent<Substance>()?.Unit;
        }
        if (unit != null && unit != player)
        {
            Debug.Log("상대를 얼리기");
        }
        projectile?.Trigger(other);
    }

    public void Update()
    {
        projectile?.Update();
    }
}
