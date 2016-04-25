using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	
	//create any variables here
	public enum statusEnum{NA, dblSpeed, trplSpeed, invuln};

	//player base speed multiplyer
	public float speed;

	public int currentLevel;

	//constant time variables
	private const float pwrUpTime = 5f;		//the preset time values used to set whenever
	private const float multiTimePreset = 2f;	//a power up is found or the multiplier is changed

	//constant multiplier values (the rate of change and max/min vals
	private const int multiRateOfChange = 2;
	private const int maxMulti = 8;
	private const int minMulti = 2;
	private const int Ceil = 4;
	private const int Wall = 15;
	
	//modifiable variables
	private statusEnum status;
	private float statusTime;
	private float multiplierTime;
	private int multiplier;
	private int score;
	private float speedModifier = 1f;
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		//initialise any values
		player = GameObject.Find ("Player");
		status = statusEnum.NA;
		statusTime = 0f;
		multiplier = minMulti;
		multiplierTime = 0f;
		score = 0;
		currentLevel = 0;
	}
	
	
	// Update is called once per frame
	void Update () {
		currentLevel = (int)transform.position.y + 4;
		speed += 0.2f * Time.deltaTime;
		handleInput ();
		
		float deltaTime = Time.deltaTime;
		
		//something to constantly be up to date on the player's status
		if(statusTime <= 0f)
			status = statusEnum.NA;
		else
			statusTime -= deltaTime;
		
		//update the multiplier timer
		if (multiplierTime < 0)
			multiplier = minMulti;
		else
			multiplierTime -= deltaTime;
		
		//switch statement to dictate what should effects should occur
		switch(status)
		{
			//no effects currently in place beyond changing the 
			//material's colour, not too clued on this as of yet
		case statusEnum.NA:
			//player.renderer.material.color = Color.green;
			speedModifier = 1f;
			break;
		case statusEnum.invuln:
			//player.renderer.material.color = Color.magenta;
			speedModifier = 1f;
			break;
		case statusEnum.dblSpeed:
			//player.renderer.material.color = Color.yellow;
			speedModifier = 1.5f;
			break;
		case statusEnum.trplSpeed:
			speedModifier = 2f;
			//player.gameObject.renderer.material.color = Color.white;
			break;
		}
	}
	
	private void handleInput()
	{
		///////////////////////////////////
		///	Store all analogue values	///
		///////////////////////////////////
		
		//update left analogue data
		float leftAnalogueX = Input.GetAxis("LeftAnalogueX");
		float leftAnalogueY = Input.GetAxis("LeftAnalogueY");
	
		//update right analogue data
		float rightAnalogueX = Input.GetAxis("RightAnalogueX");
		float rightAnalogueY = Input.GetAxis("RightAnalogueY");
		
		//update the back trigger data
		float leftTrigger = Input.GetAxis("LeftTrigger");
		float rightTrigger = Input.GetAxis("RightTrigger");
		
		///////////////////////////////////
		///	Handle all analogue inputs	///
		///////////////////////////////////
		
		//left and right on the left analogue (the stopping point is declared as a constant at the top, edit to which seems more suitable)
		if(leftAnalogueX != 0)
		{
			if(leftAnalogueX > 0 && transform.position.x < Wall)
				transform.position += new Vector3(speed, 0, 0) * speedModifier * Time.deltaTime;
			else if(leftAnalogueX < 0 && transform.position.x > -Wall)
				transform.position += new Vector3(-speed, 0, 0) * speedModifier * Time.deltaTime;
			
		}
		//check if there is any recognised input to begin with
		if (leftAnalogueY != 0) 
		{
			//now check for a negative or positive reading to know whether it is going up or down
			if(leftAnalogueY < 0 && transform.position.y < Ceil)
				transform.position += new Vector3(0, speed, 0) * speedModifier * Time.deltaTime;
			else if(leftAnalogueY > 0 && transform.position.y > -Ceil)
				transform.position += new Vector3(0, -speed, 0) * speedModifier * Time.deltaTime;
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		switch(other.gameObject.tag)
		{
		case "invuln_PwrUp":
			//do the invulnerability bit
			Destroy(other.gameObject);
			status = statusEnum.invuln;
			statusTime = pwrUpTime;
			score += (int)status * multiplier;
			changeMultiplier();
			break;
		case "dblSpeed_PwrUp":
			//do the double speed bit
			other.gameObject.SetActive(false);
			status = statusEnum.dblSpeed;
			statusTime = pwrUpTime;
			score += (int)status * multiplier;
			changeMultiplier();
			break;
		case "trplSpeed_PwrUp":
			//do the triple speed bit
			Destroy(other.gameObject);
			status = statusEnum.trplSpeed;
			statusTime = pwrUpTime;
			//open to interpretation, possibly add to score using the status 
			//enum converted to int times by the multiplier
			score += (int)status * multiplier;
			changeMultiplier();
			break;
		case "note":
			//changes when hitting a musical note
			
			//possibly reference the musical note's own script and play the note corresponding
			//to its place and its type
			//other.gameObject.GetComponent()
			
			//once that is done, destroy the note? or change its colour I don't know which would be preferred
			//other.transform.parent.gameObject.SetActive(false);
			other.GetComponent<ParticleSystem>().Emit(100);
			score += multiplier;
			changeMultiplier();
			break;
		case "rest":
			GameObject.Find("GameManager").GetComponent<StaveManager>().Reset();
			score = 0;
			break;
		case "enemy":
			GameObject.Find("GameManager").GetComponent<StaveManager>().Reset();
			score = 0;
			break;
		}
	}
	
	//use to add to the multiplier, rather than having the if statement copy pasted on every case
	private void changeMultiplier()
	{
		if (multiplier < maxMulti) 
		{
			multiplier += multiRateOfChange;
		}
		multiplierTime = multiTimePreset;
	}
}
