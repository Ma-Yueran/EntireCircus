using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    Aiming,
    Charging
};

public class Tent : MonoBehaviour, IHealthChangeSignaller
{
    public Alignment alignment;
    public GameObject cannonObject;
    public PlayerAmmo playerAmmo;
    public ProjectileObjectPool projectileObjectPool;

    public int maxHealth;
    int _currentHealth;

    // State variables
    PlayerState _playerState = PlayerState.Aiming;
    Vector2 _pointTowardsNormalized = new Vector2(0, 0);
    int _selectedAmmo = -1;

    List<IHealthChangeListener> _healthChangeListeners = new List<IHealthChangeListener>();

    void Start()
    {
        _currentHealth = maxHealth;

        foreach (IHealthChangeListener listener in _healthChangeListeners)
        {
            listener.ReceiveHealthChangeSignal(alignment, _currentHealth, maxHealth);
        }
    }

    public void RegisterAsHealthChangeListener(IHealthChangeListener listener)
    {
        _healthChangeListeners.Add(listener);
    }

    // Actions available during aiming
    public void UpdateMousePosition(Vector3 newMousePosition)
    {
        if (_playerState != PlayerState.Aiming) return;

        Vector2 pointTowards = newMousePosition - cannonObject.transform.position;
        _pointTowardsNormalized = pointTowards.normalized;
        float rawAngle = Mathf.Atan2(Mathf.Abs(pointTowards.y), Mathf.Abs(pointTowards.x)) * Mathf.Rad2Deg;

        bool pointRight = pointTowards.x >= 0;
        bool pointUp = pointTowards.y >= 0;

        float trueAngle = rawAngle; // Default case, 1st Quadrant

        if (pointRight)
        {
            if (!pointUp) trueAngle = 360 - rawAngle; //4th Quadrant
        }
        else
        {
            if (pointUp)
            { 
                trueAngle = 180 - rawAngle; //2nd Quadrant
            }
            else
            {
                trueAngle = 180 + rawAngle; // 3rd Quadrant
            }
        }

        if (trueAngle <= 52 || trueAngle >= 340) 
        {
            cannonObject.transform.rotation = Quaternion.Euler(0, 0, trueAngle);
        }
    }

    public void AISetAngle(float trueAngle)
    {
        if (_playerState != PlayerState.Aiming) return;

        if (trueAngle <= 52 || trueAngle >= 340) 
        {
            _pointTowardsNormalized = new Vector2(-1, Mathf.Tan(trueAngle * Mathf.Deg2Rad)).normalized;

            if (trueAngle < 90)
            {
                trueAngle = 180 - trueAngle;
            }
            else if (trueAngle > 270)
            {
                trueAngle = 540 - trueAngle;
            }

            cannonObject.transform.rotation = Quaternion.Euler(0, 0, trueAngle);
        }
    }

    public void BeginChargingAttack()
    {
        if (_playerState != PlayerState.Aiming) return;
        if (!playerAmmo.AmmoAtIndexFireable(_selectedAmmo)) return;

        _playerState = PlayerState.Charging;
    }

    public void SelectAmmo(int selectedAmmo)
    {
        if (_playerState != PlayerState.Aiming) return;
        Debug.Log("Selected Ammo: " + selectedAmmo);

        ChangeSelectedAmmo(selectedAmmo);
    }

    // Actions available during charging
    public void CancelChargingAttack()
    {
        if (_playerState != PlayerState.Charging) return;

        EndChargeEnterAiming();
    }

    public void FireChargedAttack()
    {
        if (_playerState != PlayerState.Charging) return;
        
        AmmoData ammoToFire = playerAmmo.FireAmmoAtIndex(_selectedAmmo);
        
        projectileObjectPool.ShootProjectile(ammoToFire, alignment, transform.position, _pointTowardsNormalized);

        EndChargeEnterAiming();
        ChangeSelectedAmmo(-1);
    }

    void ChangeSelectedAmmo(int newSelectedAmmo)
    {
        _selectedAmmo = newSelectedAmmo;
    }

    void EndChargeEnterAiming()
    {
        _playerState = PlayerState.Aiming;
    }

    public void TakeDamage(int damageReceived)
    {
        _currentHealth -= damageReceived;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        foreach (IHealthChangeListener listener in _healthChangeListeners)
        {
            listener.ReceiveHealthChangeSignal(alignment, _currentHealth, maxHealth);
        }
    }
}
