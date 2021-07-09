using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IObjectPoolDeallocationSignaller
{
    static int _spentProjectileLayer = 12;
    static float _timeToDespawn = 3f;
    static float _bounds = 4.5f;

    public int projectileIndex = -1;

    Rigidbody2D _rigidbody;
    BoxCollider2D _boxCollider;
    SpriteRenderer _spriteRenderer;
    AmmoData _ammoData;
    int _remainingProjectileHealth = 0;
    float _despawnTimer = 0f;

    bool _hitGround = false;
    List<IObjectPoolDeallocationListener> _listeners = new List<IObjectPoolDeallocationListener>();

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        gameObject.SetActive(false);
    }

    void Update()
    {
        bool beyondVerticalBounds = transform.position.x <= -_bounds || transform.position.x >= _bounds;
        bool beyondHorizontalBounds = transform.position.y <= -_bounds || transform.position.y >= _bounds;

        if (beyondHorizontalBounds || beyondVerticalBounds)
        {
            MarkProjectileForDeallocation();
        }

        if (_hitGround)
        {
            _despawnTimer += Time.deltaTime;
            
            if (_despawnTimer >= _timeToDespawn)
            {
                MarkProjectileForDeallocation();
            }
        }
    }

    public void RegisterAsListener(IObjectPoolDeallocationListener listener)
    {
        _listeners.Add(listener);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedWith = collision.collider.gameObject;

        if (collidedWith.tag == "Structure")
        {
            // Deal damage to structure
            // Delete this projectile
            Tent hitTent = collidedWith.GetComponent<Tent>();

            int structuralDamage = _ammoData.structuralDamage;
            hitTent.TakeDamage(structuralDamage);
            DamageNumberObjectPool.instance.ShowDamageNumber(structuralDamage, Vector2.Lerp(transform.position, collision.collider.transform.position, 0.5f));

            _remainingProjectileHealth = 0;
            MarkProjectileForDeallocation();
        }

        if (collidedWith.tag == "Projectile")
        {
            Projectile otherProjectile = collidedWith.GetComponent<Projectile>();

            int newHealthValue = otherProjectile._ammoData.antiProjectileHealth >= _ammoData.antiProjectileHealth ? 0 : _remainingProjectileHealth - 1;

            if (_remainingProjectileHealth == 0)
            {
                gameObject.layer = _spentProjectileLayer;
                _rigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (collidedWith.tag == "Ground")
        {
            gameObject.layer = _spentProjectileLayer;
            _hitGround = true;
        }
    }

    public void MarkProjectileForDeallocation()
    {
        foreach (IObjectPoolDeallocationListener listener in _listeners)
        {
            listener.ReceiveDeallocationSignal(projectileIndex);
        }
    }

    public void ShootProjectile(AmmoData firedProjectile, Alignment alignment, Vector2 initialPosition, Vector2 directionVector)
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;

        gameObject.layer = StaticUtil.projectileAlignmentLayerMap[alignment];

        _ammoData = firedProjectile;

        _remainingProjectileHealth = _ammoData.antiProjectileHealth;

        _despawnTimer = 0f;
        _hitGround = false;

        _spriteRenderer.sprite = _ammoData.sprite;
        _spriteRenderer.flipX = alignment == Alignment.PlayerTwo;
        float torqueToAdd = alignment == Alignment.PlayerTwo? 5f : -5f;

        _rigidbody.mass = _ammoData.weight;
        _rigidbody.gravityScale = _ammoData.gravityScale;
        _rigidbody.AddForce(directionVector * _ammoData.shootingForce);
        _rigidbody.AddTorque(torqueToAdd);
    }

    public void DestroyProjectile()
    {
        gameObject.SetActive(false);
    }
}
