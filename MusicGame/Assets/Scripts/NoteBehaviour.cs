using UnityEngine;
using System.Collections;

public class NoteBehaviour : MonoBehaviour {

	enum Note
	{
		D,
		E,
		F,
		G,
		A,
		B,
		C
	}

	private Note note;
	private float speed;
	private GameObject manager;

	void SetNote(int noteVal)
	{

	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		speed = manager.GetComponent<StaveManager>().noteSpeed;
		transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        if (transform.position.x < -28)
        {
            gameObject.SetActive(false);
        }
	}

    void FixedUpdate()
    {
        
    }
}
