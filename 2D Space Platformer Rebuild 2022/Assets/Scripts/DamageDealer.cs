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

    //bool for determining if it's player weapon or not
    ProjectileMoveScript projectileMoveScript;
    public bool isPlayerWeapon;

	private void Awake()
	{
        projectileMoveScript = GetComponentInChildren<ProjectileMoveScript>();
	}
	public int GetDamager()
	{
        return damage;
	}

    public void Hit()
	{
        if(isPlayerWeapon)
		{
            projectileMoveScript.DestroyParticle(10f);
            Debug.Log("Destroy Particle");

        }
        //Destroy Enemy we hit or projectile that could be coming our way
        Destroy(gameObject);
    }
}
