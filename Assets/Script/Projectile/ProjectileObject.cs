using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    //발동하는 효과
    IProjectile projectile;
    public IProjectile Projectile { get => projectile; set { projectile = value; } }

    // Start is called before the first frame update
    void Start()
    {
        if(projectile == null)
        {
            Debug.Log("풀링오브젝트를 사용하지 않는 destroy");
            Destroy(gameObject);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile == null)
        {
            Debug.Log("풀링오브젝트를 사용하지 않는 destroy");
            Destroy(gameObject);
        }
        projectile?.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        projectile?.Trigger(other);
    }
}
