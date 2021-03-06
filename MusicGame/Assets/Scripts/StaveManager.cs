﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaveManager : MonoBehaviour {

	public GameObject quaver;
	public GameObject crotchet;
	public GameObject minim;
	public GameObject rest;
	public GameObject enemy;
	public GameObject half;
	public GameObject quarter;
	public GameObject dblPower;

	public float noteSpeed = 15f;

	private const int maxObjects = 10;

	public float timeScale = 1.0f;
	private float spawnTimer = 1.0f;
	private int curHeight = 0;
	private List<GameObject> notePool = new List<GameObject>();

	private const float eighthLength = 0.125f;
	private const float quarterLength = 0.25f;
	private const float halfLength = 0.5f;

	private int totalChance;
	public int noteSpawnChance = 0;
	public int enemySpawnChance = 0;
	public int restSpawnChance = 0;

	public void Reset()
	{
		for (int i = 0; i < maxObjects * 5; ++i)
			notePool [i].SetActive (false);
		GameObject.Find ("Player").GetComponent<PlayerScript> ().speed = 15;
		spawnTimer = 1.0f;
		curHeight = 0;
		noteSpeed = 15f;
	}

	void LoadInObjects(GameObject type)
	{
		for (int i = 0; i < maxObjects; ++i) 
		{
			GameObject newNote = Instantiate(type) as GameObject;
			newNote.transform.parent = transform;
			notePool.Add (newNote);
		}
	}

	// Use this for initialization
	void Start () {
		LoadInObjects (quaver);
		LoadInObjects (minim);
		LoadInObjects (crotchet);
		LoadInObjects (enemy);
		LoadInObjects (rest);
		LoadInObjects (dblPower);

		totalChance = enemySpawnChance + (noteSpawnChance * 3) + restSpawnChance;

		for (int i = 0; i < maxObjects * 6; ++i)
			notePool [i].SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer -= Time.deltaTime;
		noteSpeed += 0.2f * Time.deltaTime;
		if(spawnTimer < 0)
        {
			RandomizeNewNote();
			SpawnPowerUp();
        }
	}

	private void SpawnPowerUp ()
	{
		switch(Random.Range(0,40))
		{
		case 0:
			ActivatePowerUp(5 * maxObjects);
			break;
		default:
			break;
		}
	}

	//Code for randomly selecting a node type and the change in height for the next node to spawn.
	private void RandomizeNewNote()
	{
		//Type specified for the next note
		int typeSpec = Random.Range(0,totalChance);

		//Stave height change for the next note
		int heightChange = Random.Range(-2,3);

		//Edit the height ensuring it remains legal
		curHeight += heightChange;
		if(curHeight > 4)
			curHeight = 4;
		else if(curHeight < -4)
			curHeight = -4;

		int a = noteSpawnChance;

		//Range based statement, used instead of a case to allow ranges and altered weightings to the random generation
		if (typeSpec < a)
		{
			ActivateNote(0 * maxObjects);
			spawnTimer += timeScale * eighthLength;
			return;
		}

		a += noteSpawnChance;
		if (typeSpec < a)
		{
			ActivateNote(1 * maxObjects);
			spawnTimer += timeScale * halfLength;
			return;
		}

		a += noteSpawnChance;
		if (typeSpec < a)
		{
			ActivateNote(2 * maxObjects);
			spawnTimer += timeScale * quarterLength;
			return;
		}

		a += enemySpawnChance;
		if (typeSpec < a)
		{
			ActivateEnemy(3 * maxObjects);
			spawnTimer += timeScale * halfLength;
			return;
		}

		ActivateNote(4 * maxObjects);
		spawnTimer += timeScale * halfLength;

	}

	//Finds the next active note within the object pool to activate
	private void ActivateNote(int offset)
	{
		int i;
		for (i = 0; i < maxObjects; ++i) 
		{
			if (!notePool [offset + i].activeSelf) 
			{
				offset += i;
				break;
			}
		}

		//Activates the correct note
		notePool[offset].SetActive(true);
		notePool [offset].GetComponent<NoteBehaviour> ().Activate (curHeight);
		//Sets the position of the newly activated note

		/*notePool[offset].transform.position = new Vector3(25, curHeight, 0);
		notePool[offset].GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,1f);
		//Ensures that if the note is above a certain line it is flipped as is seen in music
		if (curHeight > 1) {
			//notePool[offset].gameObject.GetComponentInChildren<Transform>().position = new Vector3(0,1.15f,0.1f);
			notePool [offset].transform.rotation = Quaternion.Euler (new Vector3 (180, 0, 0));
		} else {
			//notePool[offset].gameObject.GetComponentInChildren<Transform>().position = new Vector3(0,1.15f,-0.1f);
			notePool [offset].transform.rotation = new Quaternion (0, 0, 0, 0);
		}*/
	}

	private void ActivateEnemy(int offset)
	{
		int i;
		for (i = 0; i < maxObjects; ++i) 
		{
			if (!notePool [offset + i].activeSelf) 
			{
				offset += i;
				break;
			}
		}
		
		//Activates the correct note
		notePool[offset].SetActive(true);
		//Sets the position of the newly activated note
		notePool[offset].transform.position = new Vector3(25, curHeight, 0);

		switch (Random.Range (0, 3)) {
		case 0:
			notePool [offset].GetComponent<AI> ().setBehavior ("flank", curHeight + 4);
			break;
		case 1:
			notePool [offset].GetComponent<AI> ().setBehavior ("charge", curHeight + 4);
			break;
		case 2:
			notePool [offset].GetComponent<AI> ().setBehavior ("zigzag", curHeight + 4);
			break;
		}
	}

	//Exact same as the note activation but spawns at inverse height so the powerup rarely spawns close to current note.
	private void ActivatePowerUp(int offset)
	{
		int i;
		for (i = 0; i < maxObjects; ++i) 
		{
			if (!notePool [offset + i].activeSelf) 
			{
				offset += i;
				break;
			}
		}
		
		notePool[offset].SetActive(true);
		notePool[offset].transform.position = new Vector3(25, -curHeight, 0);
		switch (Random.Range (0, 3)) {
		case 0:
			notePool[offset].tag = "dblSpeed_PwrUp";
			break;
		case 1:
			notePool[offset].tag = "invuln_PwrUp";
			break;
		case 2:
			notePool[offset].tag = "trplSpeed_PwrUp";
			break;
		}
	}
}
