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
	private float deathStart;
	private bool deactivating;
	private float alpha = 1f;
	private SpriteRenderer sprite;

	void SetNote(int noteVal)
	{

	}

	public void Activate(int height)
	{
		transform.position = new Vector3 (25, height, 0);
		deactivating = false;
		alpha = 1f;
		if(sprite != null)
			sprite.color = new Color(1,1,1,alpha);
	}

	public void Deactivate()
	{
		deactivating = true;
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager");
		if (GetComponentInChildren<SpriteRenderer> () != null)
			sprite = GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		speed = manager.GetComponent<StaveManager>().noteSpeed;
		transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        if (transform.position.x < -28)
        {
            gameObject.SetActive(false);
        }
		if (deactivating) {
			if(sprite != null)
				sprite.color = new Color(1,1,1,alpha);
			if(alpha > 0)
				alpha -= 0.01f;
		}
	}

    void FixedUpdate()
    {
        
    }
}
