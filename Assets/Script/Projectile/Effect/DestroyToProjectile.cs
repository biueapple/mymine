
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
        //�浹�� �̰��� ���
        if (other.TryGetComponent(out Substance substance))
        {
            if (substance.Unit == player)
                return;
            Debug.Log("Ǯ��������Ʈ�� ������� �ʴ� destroy");
            GameObject.Destroy(projectile);
        }
    }

    public void Update() { }
}