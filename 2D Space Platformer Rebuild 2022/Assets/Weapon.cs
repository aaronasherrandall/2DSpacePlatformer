using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSprite;
    //[SerializeField] Sprite spriteUp, spriteDown, spriteSide;
    [SerializeField] Transform firingPoint;
    //[SerializeField] Transform gunUp, gunDown, gunLeft, gunRight;
    [SerializeField] Transform pivot;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifTime = 5f; //5 seconds
    [SerializeField] float firingRate = 2f;
    [SerializeField] Transform followTarget;

    public bool isFiring;

    Coroutine firingCoroutine;
    [SerializeField] Transform firingPosition;

    //For firing to mouse position
    private Vector2 lookDirection;
    private float lookAngle;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed Left Mouse Button");
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }

        
        FireWeapon();
    }

	private void FixedUpdate()
	{
        FollowTarget();
    }

	private void FollowTarget()
	{
        transform.position = Vector3.Lerp(transform.position, followTarget.transform.position, 1.5f * Time.deltaTime);
	}

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }

    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectilePrefab, firingPosition.transform.position, Quaternion.identity);
            instance.transform.up = FireDirection();
            Debug.Log("Instantiated Prefab");


            Rigidbody2D rb = instance.GetComponentInChildren<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = FireDirection() * projectileSpeed;
            }

            Destroy(instance, projectileLifTime);
            Debug.Log("Bullet has been destroyed");
            yield return new WaitForSeconds(firingRate);
        }
    }


    //Make a function that the player can call to make the gun face a certain direction
    public void ChangeWeaponFlip(bool flipX)
	{
        weaponSprite.flipX = flipX;
	}

    private Vector2 FireDirection()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }

    public void FireWeapon()
	{
        //Store direction towards mouse in fireDirection
        var fireDirection = FireDirection();
        //Calculate the different in value between upwarda and firing direction: -1 to 1 range;
        //Where 1 is matching and -1 is opposite

        pivot.up = fireDirection;
        
        //var upwardsFacingAmount = Vector2.Dot(fireDirection, Vector2.up);
  //      if(upwardsFacingAmount >= .75)
		//{
  //          //Upwards direction
  //          firingPoint.position = gunUp.position;
  //          //Swap Sprite
  //          weaponSprite.sprite = spriteUp;
  //          //Set firing point
  //          weaponSprite.sortingOrder = -1;
  //      }
  //      else if (upwardsFacingAmount <= -.75)
		//{
  //          //Downwards direction
  //          firingPoint.position = gunDown.position;
  //          //Swap Sprite
  //          weaponSprite.sprite = spriteDown;
  //          //Set firing point
  //          weaponSprite.sortingOrder = 1;
  //      }
  //      else //or else do this
		//{
  //          weaponSprite.sprite = spriteSide;
  //          weaponSprite.sortingOrder = 1;
  //          //Set it left and right
  //          if (!weaponSprite.flipX)
		//	{
  //              firingPoint.position = gunLeft.position;
  //          }
  //          else
		//	{
  //              firingPoint.position = gunRight.position;
  //          }
  //      }

        Fire();


    }

    public void ChangeFiringPoint(bool flipX)
	{

	}
}
