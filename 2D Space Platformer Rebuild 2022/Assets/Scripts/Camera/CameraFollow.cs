using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    //[SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offSetCamera;

	private void Awake()
	{
		playerTransform = FindObjectOfType<PlayerMovement>().transform;
	}

	// Update is called once per frame
	private void LateUpdate()
    {
        transform.position = playerTransform.position + offSetCamera;
    }
}
