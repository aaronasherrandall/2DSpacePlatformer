using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildOfPlanet : MonoBehaviour
{
	TrackCurrentPlanet trackCurrentPlanet;
	
	public GameObject player;
	public GameObject NPC;

	private bool triggered;
	private void Awake()
	{
		trackCurrentPlanet = FindObjectOfType<TrackCurrentPlanet>();
	}

	private void Update()
	{
		if (player != null)
		{

			if (triggered)
			{
				player.transform.SetParent(transform);
				trackCurrentPlanet.currentPlanet = this.gameObject;
			}
		}

		if (NPC != null)
		{
			if (triggered)
			{
				NPC.transform.SetParent(transform);
			}
		}
		//else if(!triggered)
		//{
		//	player.transform.SetParent(null);
		//}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.gameObject.activeSelf)
		{
	
			player = collision.gameObject;
			triggered = true;
			
			
		}

		if (collision.CompareTag("NPC") && collision.gameObject.activeSelf)
		{
			
			NPC = collision.gameObject;
			triggered = true;
			
		}


	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.gameObject.activeSelf)
		{
			player = collision.gameObject;
			triggered = false;
		}

		if (collision.CompareTag("NPC") && collision.gameObject.activeSelf)
		{
			NPC = collision.gameObject;
			triggered = false;
		}
	}
}
