using UnityEngine;

public class DamageToProjectile : IProjectile
{
    readonly IProjectile projectile;
    readonly Player player;
    readonly AttackInformation attackInfo;

    public DamageToProjectile(Player player, IProjectile projectile, AttackInformation attackInfo)
    {
        this.player = player;
        this.projectile = projectile;
        this.attackInfo = attackInfo;
    }

    public void Trigger(Collider other)
    {
        //충돌은 이것을 사용
        if(other.TryGetComponent(out Substance substance))
        {
            if(substance.Unit != player)
                substance.Hit(attackInfo);
        }
        projectile?.Trigger(other);
    }

    public void Update()
    {
        projectile?.Update();
    }
}
