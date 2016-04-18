using UnityEngine;
using System.Collections;

public class NoteBehaviour : MonoBehaviour {

    public float speed = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        if (transform.position.x < -30)
        {
            gameObject.SetActive(false);
        }
	}

    void FixedUpdate()
    {
        
    }
}
