using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPool : MonoBehaviour, IObjectPoolDeallocationListener
{
    public static int poolSize = 40;
    public GameObject projectileBase;

    List<Projectile> _pool = new List<Projectile>();
    Stack<int> availableProjectiles = new Stack<int>(poolSize);

    bool _initialized = false;
    
    void Start()
    {
        if (_initialized) return;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectileObject = Instantiate(projectileBase, new Vector3(-5f, 0, 0), Quaternion.identity, transform);
            
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.projectileIndex = i;
            _pool.Add(projectile);

            projectile.RegisterAsListener(this);

            availableProjectiles.Push(i);
        }

        _initialized = true;
    }

    public void ShootProjectile(AmmoData firedProjectile, Alignment alignment, Vector2 initialPosition, Vector2 directionVector)
    {
        if (availableProjectiles.Count == 0)
        {
            Debug.Log("ProjectileObjectPool: Maximum capacity reached!");
            return;
        }

        int projectileToUse = availableProjectiles.Pop();
        _pool[projectileToUse].ShootProjectile(firedProjectile, alignment, initialPosition, directionVector);
    }

    public void ReceiveDeallocationSignal(int indexToDeallocate)
    {
        if (indexToDeallocate < 0) {
            Debug.Log("ProjectileObjectPool: Collision occurred with negative-indexed projectile!");
            return;
        }

        _pool[indexToDeallocate].DestroyProjectile();
        availableProjectiles.Push(indexToDeallocate);
    }
}
