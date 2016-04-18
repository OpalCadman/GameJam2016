using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaveManager : MonoBehaviour {

    public GameObject quaver;
	public GameObject minim;
	public GameObject crotchet;
	public GameObject rest;
	public GameObject half;
	public GameObject quarter;
	int maxObjects = 10;

	public float timeScale = 1.0f;
	private float spawnTimer = 1.0f;
	private int curHeight = 0;
	private List<GameObject> notes = new List<GameObject>();

	public void Reset()
	{
		for (int i = 0; i < maxObjects * 6; ++i)
			notes [i].SetActive (false);
		spawnTimer = 1.0f;
		timeScale = 1.0f;
		curHeight = 0;
	}

	void LoadInObjects(GameObject type)
	{
		for (int i = 0; i < maxObjects; ++i) 
		{
			GameObject newNote = Instantiate(type) as GameObject;
			newNote.transform.parent = transform;
			notes.Add (newNote);
		}
	}

	// Use this for initialization
	void Start () {
		LoadInObjects (quaver);
		LoadInObjects (minim);
		LoadInObjects (crotchet);
		LoadInObjects (rest);
		LoadInObjects (half);
		LoadInObjects (quarter);

		for (int i = 0; i < maxObjects * 6; ++i)
			notes [i].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
		if(spawnTimer < 0)
        {
			RandomizeNewNote();
        }
	}

	private void RandomizeNewNote()
	{
		int typeSpec = Random.Range(0,40);
		int heightChange = Random.Range(-2,3);
		curHeight += heightChange;
		if(curHeight > 4)
			curHeight = 4;
		else if(curHeight < -4)
			curHeight = -4;

		if (typeSpec < 10)
		{
			ActivateNote(0 * maxObjects);
			spawnTimer += timeScale * 0.125f;
		}
		else if (typeSpec < 20)
		{
			ActivateNote(1 * maxObjects);
			spawnTimer += timeScale * 0.25f;
		}
		else if (typeSpec < 30)
		{
			ActivateNote(2 * maxObjects);
			spawnTimer += timeScale * 0.5f;
		}
		else
		{
			ActivateNote(3 * maxObjects);
			spawnTimer += timeScale * 0.5f;
		}
	}

	private void ActivateNote(int offset)
	{
		Debug.Log ("Activating Note");
		int i;
		for (i = 0; i < maxObjects; ++i) 
		{
			if (!notes [offset + i].activeSelf) 
			{
				offset += i;
				break;
			}
		}

		notes[offset].SetActive(true);
		notes[offset].transform.position = new Vector3(25, curHeight, 0);
		if (curHeight > 1)
			notes [offset].transform.rotation = Quaternion.Euler(new Vector3 (180, 0, 0));
	}
}
