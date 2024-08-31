using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    //�ߵ��ϴ� ȿ��
    IProjectile projectile;
    public IProjectile Projectile { get => projectile; set { projectile = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if(projectile == null)
        {
            Debug.Log("Ǯ��������Ʈ�� ������� �ʴ� destroy");
            Destroy(gameObject);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile == null)
        {
            Debug.Log("Ǯ��������Ʈ�� ������� �ʴ� destroy");
            Destroy(gameObject);
        }
        projectile?.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        projectile?.Trigger(other);
    }
}
