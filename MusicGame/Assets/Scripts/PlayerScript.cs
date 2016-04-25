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
	private const int multiRateOfChange = 1;
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
		//load GUI
		loadGUI ();
	}
	
	
	// Update is called once per frame
	void Update () {
		currentLevel = (int)transform.position.y + 4;
		speed += 0.2f * Time.deltaTime;
		handleInput ();
		handleGUI ();
		
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
		case statusEnum.NA:
			speedModifier = 1f;
			break;
		case statusEnum.invuln:
			Debug.Log("INVULNERABLE");
			speedModifier = 1f;
			break;
		case statusEnum.dblSpeed:
			Debug.Log("DOUBLE");
			speedModifier = 1.5f;
			break;
		case statusEnum.trplSpeed:
			Debug.Log("TRIPLE");
			speedModifier = 2f;
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
	
	private void loadGUI()
	{
		//Anchor explained:
		//anchor is similar to what is often described as the pivot, the point at which an objet is picked up from
		//an anchor of 0, 1 is left top, 0 being the left hand side on the x and 1 being the top hand side on the y
		//the anchor is considered the pivot of the canvas

		//when creating UI, the anchor is essential to placement
		Vector2 myUIAnch = new Vector2 (0, 1);
		Vector3 scorePos = new Vector3(10, -5, 0);

		//find the canvas and its script necessary to creating UI elements
		GameObject uiCanvas = GameObject.FindGameObjectWithTag ("canvasGUI");
		UIScript uiScript = uiCanvas.GetComponent<UIScript> ();

		//setting up the score UI
		GameObject scoreUI = uiScript.setupUIText ("scoreUI", "Score: 0", myUIAnch, scorePos, true);
		//the UI's height is the sizeDelta.y
		float scoreHeight = scoreUI.GetComponent<RectTransform>().sizeDelta.y;
		//setting up the multiplier UI and calculating its position
		Vector3 multiPos = scorePos + new Vector3 (0, -scoreHeight*1.5f, 0);
		uiScript.setupUIText ("multiUI", "X2 Multiplier", myUIAnch, multiPos, true);
		//alter the anchor for the powerup UI text
		myUIAnch.x = 1;
		Vector3 pwrupPos = new Vector3 (-10, -5, 0);
		//setting up the powerup UI
		GameObject pwrupUI = uiScript.setupUIText ("pwrupUI", "", myUIAnch, pwrupPos, true);
		//align text to the right for the powerup UI
		pwrupUI.GetComponent<Text> ().alignment = TextAnchor.MiddleRight;

		//creating the imageUI
		float pwrupTxtHeight = scoreUI.GetComponent<RectTransform>().sizeDelta.y;
		Vector3 pwrUpImgPos = pwrupPos + new Vector3(0, -pwrupTxtHeight*1.5f, 0);
		GameObject pwrupImg = uiScript.setupUIImage ("pwrupImg", "speedImage", myUIAnch, pwrUpImgPos, false);
	}
	
	//handles all the GUI
	private void handleGUI()
	{
		//update the score holder with the current score
		GameObject scoreUI = GameObject.Find("scoreUI");
		scoreUI.GetComponent<Text>().text = "Score: " + score;

		//update the multiplier holder with the current score
		GameObject multiUI = GameObject.Find("multiUI");
		multiUI.GetComponent<Text>().text = "X" + multiplier + " Multiplier";

		//update the powerup UI text
		GameObject pwrupUI = GameObject.Find ("pwrupUI");
		GameObject pwrupImg = GameObject.Find ("pwrupImg");

		switch(status)
		{
			//no effects currently in place beyond changing the 
			//material's colour, not too clued on this as of yet
		case statusEnum.NA:
			pwrupUI.GetComponent<Text>().text = "Power up: NA";
			pwrupImg.GetComponent<Image>().enabled = false;
			break;
		case statusEnum.invuln:
			pwrupUI.GetComponent<Text>().text = "Power up: Invulnerability";
			pwrupImg.GetComponent<Image>().enabled = true;
			break;
		case statusEnum.dblSpeed:
			pwrupUI.GetComponent<Text>().text = "Power up: Speed Boost";
			pwrupImg.GetComponent<Image>().enabled = true;
			break;
		case statusEnum.trplSpeed:
			pwrupUI.GetComponent<Text>().text = "Power up: Mega Speed Boost";
			pwrupImg.GetComponent<Image>().enabled = true;
			break;
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		GameObject pwrupImg = GameObject.Find ("pwrupImg");
		switch(other.gameObject.tag)
		{
		case "invuln_PwrUp":
			//do the invulnerability bit
			other.gameObject.SetActive(false);
			status = statusEnum.invuln;
			statusTime = pwrUpTime;
			score += (int)status * multiplier;
			changeMultiplier();
			//update the power up img in the UI
			pwrupImg.SetActive(true);
			pwrupImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("invulnImage");
			break;
		case "dblSpeed_PwrUp":
			//do the double speed bit
			other.gameObject.SetActive(false);
			status = statusEnum.dblSpeed;
			statusTime = pwrUpTime;
			score += (int)status * multiplier;
			changeMultiplier();
			//update the power up img in the UI
			pwrupImg.SetActive(true);
			pwrupImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("speedImage");
			break;
		case "trplSpeed_PwrUp":
			//do the triple speed bit
			other.gameObject.SetActive(false);
			status = statusEnum.trplSpeed;
			statusTime = pwrUpTime;
			score += (int)status * multiplier;
			changeMultiplier();
			//update the power up img in the UI
			pwrupImg.SetActive(true);
			pwrupImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("speedImage2");
			break;
		case "note":
			//changes when hitting a musical note
			
			//possibly reference the musical note's own script and play the note corresponding
			//to its place and its type
			
			//once that is done, destroy the note? or change its colour I don't know which would be preferred
			//other.transform.parent.gameObject.SetActive(false);
			other.GetComponent<ParticleSystem>().Emit(100);
			
			other.GetComponentInChildren<NoteBehaviour>().Deactivate();
			score += multiplier;
			changeMultiplier();
			break;
		case "rest":
			if(status != statusEnum.invuln)
			{
				GameObject.Find("BlackPanel").GetComponent<Fade>().StartFadeOut(2);
			}
			break;
		case "enemy":
			if(status != statusEnum.invuln)
			{
				GameObject.Find("BlackPanel").GetComponent<Fade>().StartFadeOut(2);
			}
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
