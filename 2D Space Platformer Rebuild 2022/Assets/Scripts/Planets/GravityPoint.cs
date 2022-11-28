using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways()]
public class GravityPoint : MonoBehaviour
{

    public float reentryOffset;
    public float reEntryRotationForce;

    float playerLerpTimerEnterPlanet;

    //Reference for SpriteRenderer used for mini map
    //[SerializeField] private SpriteRenderer mySpriteRenderer;

    //We are being pulled towards center of the Earth by gravitational force: gravityScale
    public float gravityScale = 12f, planetRadius = 1f, gravityMinRange = 1.5f, gravityMaxRange = 2f, minRSOffSet = 1.75f, maxRSOffSet = 1.75f, movementOffset;
    //Adjust this if player is not properly standing on planet
    [SerializeField] private float planetRadiusOffset;
    [SerializeField] private float gravitationalOffset;
    [SerializeField] private GameObject surface, minRange, maxRange;

    //Dynamically Change Colliders when edits are made to Gravity Point script variables
    [SerializeField] private CircleCollider2D planetCircleCollider2D;
    [SerializeField] private CircleCollider2D surfaceCircleCollider2D;
    [SerializeField] private CircleCollider2D maxRangeCircleCollider2D;

	

	// Start is called before the first frame update
	void Start()
    {
        playerLerpTimerEnterPlanet = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        //Change scale for min
        minRange.transform.localScale = new Vector3((planetRadius + gravityMinRange) * minRSOffSet, (planetRadius + gravityMinRange) * minRSOffSet, 1);
        maxRange.transform.localScale = new Vector3((planetRadius + gravityMinRange + gravityMaxRange) * maxRSOffSet, (planetRadius + gravityMinRange + gravityMaxRange) * maxRSOffSet, 1);
        planetCircleCollider2D.radius = planetRadius + gravityMinRange + gravityMaxRange;
        surface.transform.localScale = new Vector3(1f, 1f, 1f) * planetRadius * 2;

        surfaceCircleCollider2D.radius = 1f + planetRadiusOffset;

    }

    public void OnTriggerStay2D(Collider2D obj)
    {
        //Change magnitude of gravitational force being applied to player
        float gravitationalPower = gravityScale / planetRadius;
        //Check if players distance is outside the min range
        float dist = Vector2.Distance(obj.transform.position, transform.position);

        if (dist > (planetRadius + gravityMinRange))
        {
            float min = planetRadius + gravityMinRange + 0.5f;
            gravitationalPower = gravitationalPower * (((min + gravityMaxRange) - dist) / gravityMaxRange);
            //Turn on Jet Pack controls if we are too far away
            //obj.GetComponent<PlayerMovement>().jetPackBoostControlsOn();
        }

        //Calculate force that needs to be applied to object
        //Get direction from object to the planet's center: subtract object's position from planet's position
        Vector3 dir = (transform.position - obj.transform.position) * gravitationalPower; //this gives us gravitational force

        //Apply gravitational force to Rigidbody attached to object
        obj.GetComponent<Rigidbody2D>().AddForce(dir);

        //Gravity when on planet
        if (obj.CompareTag("Player") || (obj.CompareTag("NPC")))
        {
            obj.transform.up = Vector3.MoveTowards(obj.transform.up, -dir, gravitationalPower * Time.deltaTime + gravitationalOffset); //+ offSet);
            //Debug.Log("ENTERED " + gameObject.name);
            //if (!planetDiscovered)
            //{
            //    planetDiscovered = true;
            //    circle_SpriteRenderer.color = discoveredColor;

            //}


            //obj.transform.up = Vector2.MoveTowards(obj.transform.up, -dir, gravitationalPower * Time.deltaTime + offSet);
        }
    }

    /// <summary>
    /// Apply force to Player in the direction of the planet
    /// </summary>
    /// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
        float gravitationalPower = gravityScale / planetRadius;

        if (collision.CompareTag("Player"))
		{
            //Get the rotation of the player when entering collider
            //Quaternion rotationWhenEntering = collision.gameObject.transform.rotation;
            //Debug.Log("rotationWhenEntering " + rotationWhenEntering);

            if(collision.GetComponent<PlayerMovement>().playerState == PlayerMovement.PlayerState.OffPlanet)
			{
                //Calculate force that needs to be applied to object
                //Get direction from object to the planet's center: subtract object's position from planet's position
                Vector3 dir = (transform.position - collision.transform.position) * gravitationalPower * reentryOffset; //this gives us gravitational force
                //Rotate Player
                //playerLerpTimerEnterPlanet += Time.deltaTime;
                //collision.gameObject.transform.rotation = Quaternion.Slerp(rotationWhenEntering, surface.transform.rotation, 1f);
                //if (collision.transform.localEulerAngles.z > 90 && collision.transform.localEulerAngles.z < 270)
                //{
                //    Vector3 dirForRotationReEntry = (transform.position - collision.transform.position) * gravitationalPower * reEntryRotationForce;
                //    //collision.transform.localEulerAngles = new Vector3(0, 0, 0);
                //    //ReEntryLerpTimed(collision.transform.localEulerAngles, new Vector3(0, 0, 0), 10f);
                //    Debug.Log("Adjusting Rotation of Player");
                //    //Apply gravitational force to Rigidbody attached to object                                                                                                      
                //    collision.GetComponent<Rigidbody2D>().AddForce(dir);
                //}

                //Quaternion.Euler(surface.transform.position.x, surface.transform.position.y, surface.transform.position.z);
                //Quaternion.Slerp(rotationWhenEntering, Quaternion.identity, playerLerpTimerEnterPlanet);
                //Apply gravitational force to Rigidbody attached to object
                collision.GetComponent<Rigidbody2D>().AddForce(dir);
                Debug.Log("Force being applied to pull in player");
                
            }
           
        }
	}

    private Vector3 ReEntryLerpTimed(Vector3 collisionVector, Vector3 targetVector, float lerpTime)
	{
        return Vector3.Lerp(collisionVector, targetVector, lerpTime);
	}
}
