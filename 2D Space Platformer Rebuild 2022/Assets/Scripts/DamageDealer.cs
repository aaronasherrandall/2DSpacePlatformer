using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the amount of dama that the object this script is attached to is going to do
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    // Start is called before the first frame update
    public int GetDamager()
	{
        return damage;
	}

    public void Hit()
	{
        //Destroy Enemy we hit or projectile that could be coming our way
        Destroy(gameObject);
    }
}
