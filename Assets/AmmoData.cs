using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AmmoData : ScriptableObject
{
    public Sprite sprite;
    public float weight;
    public float shootingForce;
    public float gravityScale = 1f;
    public int structuralDamage;
    public int antiProjectileHealth;
    public bool fireable = true;
}
