using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    //[SerializeField] GameObject projectilePrefab;
    //[SerializeField] float projectileSpeed = 10f;
    //[SerializeField] float projectileLifTime = 5f; //5 seconds
    //[SerializeField] float firingRate = 2f;

    //public bool isFiring;

    //Coroutine firingCoroutine;
    //[SerializeField] Transform firingPosition;

    ////For firing to mouse position
    //private Vector2 lookDirection;
    //private float lookAngle;


    // Update is called once per frame
 //   void Update()
 //   {
 //       Fire();
 //   }

 //   void Fire()
	//{
 //       if(isFiring && firingCoroutine == null)
	//	{
 //           firingCoroutine = StartCoroutine(FireContinuously());
 //       }
 //       else if (!isFiring && firingCoroutine != null)
	//	{
 //           StopCoroutine(firingCoroutine);
 //           firingCoroutine = null;
	//	}
        
	//}

    /// <summary>
    /// This returns the direction between mouse point and player
    /// </summary>
    /// <returns></returns>
 //   private Vector2 FireDirection()
	//{
 //       return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
	//}

 //   IEnumerator FireContinuously()
	//{
 //       while(true)
	//	{
 //           GameObject instance = Instantiate(projectilePrefab, firingPosition.transform.position, Quaternion.identity);
 //           instance.transform.up = FireDirection();
 //           Debug.Log("Instantiated Prefab");


 //           Rigidbody2D rb = instance.GetComponentInChildren<Rigidbody2D>();
 //           if(rb != null)
	//		{
 //               rb.velocity = FireDirection() * projectileSpeed;
	//		}

 //           Destroy(instance, projectileLifTime);
 //           yield return new WaitForSeconds(firingRate);
	//	}
	//}
}
