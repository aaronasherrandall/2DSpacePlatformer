using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[Header("Visual Cue")]
	[SerializeField] private GameObject visualCue;

	[Header("Ink JSON")]
	[SerializeField] private TextAsset inkJSON;

	private bool playerInRange;

	private void Awake()
	{
		playerInRange = false;
		visualCue.SetActive(false);	
	}

	private void Update()
	{
		if (playerInRange)
		{
			visualCue.SetActive(true);
			if(Input.GetKeyDown(KeyCode.E))
			{
				Debug.Log(inkJSON.text);
			}
		}
		else
		{
			visualCue.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			playerInRange = true;
			Debug.Log("Player is in range with NPC");
		}
		
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerInRange = false;
			Debug.Log("Player is out of range with NPC");
		}
	}
}
