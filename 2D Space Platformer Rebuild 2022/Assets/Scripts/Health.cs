using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 50;

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //Check to see if thing we are colliding with is a DamageDealer
        //First create a local variable damageDealer that gets component DamageDealer from collider
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if(damageDealer != null)
		{
            //Take damage
            TakeDamage(damageDealer.GetDamager());
            //Tell damage dealer that it hit something so that it can destroy itself
            damageDealer.Hit();
        }
    }

    private void TakeDamage(int damage)
	{
        health -= damage;
        if(health <= 0)
		{
            Destroy(gameObject);
		}
	}
}
