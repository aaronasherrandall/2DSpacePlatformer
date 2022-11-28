using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    private Rigidbody2D AIAgentBody;
    public bool onSurfaceEnemy;
    public bool isInLowOrbitEnemy;
    // Start is called before the first frame update

    //For Dialogue
    //public DialogueTrigger dialogueTrigger;

	private void Awake()
	{
        AIAgentBody = GetComponent<Rigidbody2D>();
        //dialogueTrigger = FindObjectOfType<DialogueTrigger>();

	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            onSurfaceEnemy = true;
            //animatorEnemy.SetBool("onSurfaceEnemy", true);
            //Debug.Log("ENEMY ON SURFACE");

        }

  //      if(collision.gameObject.tag == "Player")
		//{
  //          dialogueTrigger.StartDialogue();
		//}
    }

    //Control body drag when isInLowOrbit
    public void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Planet"))
        {
            //Keep object grounde
            AIAgentBody.drag = 3;
            isInLowOrbitEnemy = true; // need to set variable for enemy
          
            float distance = Mathf.Abs(obj.GetComponent<GravityPoint>().planetRadius - Vector2.Distance(transform.position, obj.transform.position));

        }

    }
}
