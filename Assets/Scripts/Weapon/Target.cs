using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable {

    public float health;

    public void TakeDamage(float damage) {
        Debug.Log($"{gameObject} Taking Damage = {damage}");
        health -= damage;
        if(health <= 0) Destroy(gameObject);
    }
}