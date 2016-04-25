using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour {

	Action behavior;

	public AudioClip deathSound;

	AudioSource audioPlayer;

	//Change to whatever the player script with getLevel is.
	private PlayerScript playerScript;

	public bool moveVertical_ = false;
	public bool moveHorizontal_ = false;

	GameObject player;
	
	public int currentLevel, targetLevel;

	public float staveStart, staveEnd;
	public float startX, targetX;
	float width;
	float[] levels;
	float staveGap = 1f;
	//Below, the time it takes to change stave level.
	const float moveLevelTime = 0.5f;
	//Below, the time it takes to move from one end of the stave to the other.
	const float speed = 5.0f;
	//The time the current horizontal movement will take.
	public float currentMovementTime;
	public float moveStartTime = 0.0f;
	public string behaviorName;

	// Use this for initialization
	void Start () {
		levels = new float[9];
		float levelY = -4f;
		for (int i = 0; i < 9; i++) {
			levels[i] = levelY;
			levelY += staveGap;
		}

		width = transform.localScale.x;

		player = GameObject.Find("Player");
		playerScript = player.GetComponent<PlayerScript> ();

		//audioPlayer = gameObject.GetComponent<AudioSource> ();
		//audioPlayer.clip = deathSound;

		behavior = getBehavior(behaviorName);
	}

	public void setBehavior(string behavior_, int level){
		behavior = getBehavior (behavior_);
		moveHorizontal_ = false;
		moveVertical_ = false;
		currentLevel = level;
	}

	// Update is called once per frame
	void Update () {
		
		if (transform.position.x <= -24.9f) {
			Debug.Log(transform.position.y);
			gameObject.SetActive(false);
			return;
		}
		behavior();
	}

	//This function is called each tick whilst the enemy is moving.
	//It lerps the enemy towards the target height, untill the enemy has 
	//reached the correct stave.
	void moveVertical(){
		//Check if the movement has been completed, and return if it has.
		if(Time.time - moveStartTime > moveLevelTime){
			moveVertical_ = false;
			currentLevel = targetLevel;
			return;
		}
		//Move towards the target level based on the time the enemy has been moving.
		transform.position = new Vector3(transform.position.x, 
			                                        Mathf.Lerp (levels[currentLevel], levels[targetLevel], 
			            			((Time.time - moveStartTime) / (moveStartTime + moveLevelTime - Time.time))),
			                                        transform.position.z);
	}

	void moveHorizontal(){
		if (Time.time - moveStartTime > currentMovementTime) {
			moveHorizontal_ = false;
			return;
		}
		transform.position = new Vector3(Mathf.Lerp(startX, targetX,
			                                       (Time.time - moveStartTime) / (moveStartTime + currentMovementTime - Time.time)),
			                                        transform.position.y, transform.position.z);
	}

	//Name charge;
	void chargeBehavior(){
		//If you're already moving up, don't do anything.
		if (moveVertical_) {
			moveVertical ();
			return;
		} 
		//If you're already moving across, don't do anything.
		else if (moveHorizontal_) {
			moveHorizontal();
			return;
		}
		//If you're not on the players level, move to it.
		if (playerScript.currentLevel != currentLevel) {
			if (playerScript.currentLevel < currentLevel)
				moveLevelDown ();
			else if (playerScript.currentLevel > currentLevel)
				moveLevelUp ();
		} 
		else {
			moveHorizontalLeft(staveEnd);
		}
	}

	void outFlankBehavior(){
		//If you're already moving up, don't do anything.
		if (moveVertical_) {
			moveVertical ();
			return;
		} 
		//If you're already moving across, don't do anything.
		else if (moveHorizontal_) {
			moveHorizontal ();
			return;
		} 
		//If you're not on the players level, and are to the right of them,
		//move below or above them.
		else if (currentLevel == playerScript.currentLevel && 
			transform.position.x > player.transform.position.x) {
			if (currentLevel == 0)
				moveLevelUp ();
			else if (currentLevel == 9)
				moveLevelDown ();
			else if (currentLevel < 5)
				moveLevelUp ();
			else if (currentLevel > 4)
				moveLevelDown ();
		} 
		//If you're to the right of the player, move behind them.
		else if (transform.position.x > player.transform.position.x - width)
			moveHorizontalLeft (player.transform.position.x - width * 1.5f);
		//If you're not on the players level, move to it.
		else if (playerScript.currentLevel != currentLevel) {
			if (playerScript.currentLevel < currentLevel)
				moveLevelDown ();
			if (playerScript.currentLevel > currentLevel)
				moveLevelUp ();
		} 
		//Charge back along the stave.
		else if (transform.position.x < player.transform.position.x) {
			moveHorizontalRight (staveStart - width);
			behavior = chargeBehavior;
		}
	}

	//Instruct the enemy to begin moving up one stave level.
	//If the enemy is at the top of the stave, nothing will happen.
	void moveLevelUp(){
		//Check if enemy is not at top of stave.
		if (currentLevel == levels.Length)
			return;
		targetLevel = currentLevel + 1;
		moveVertical_ = true;
		moveStartTime = Time.time;
	}

	//Instruct the enemy to begin moving down one stave level.
	//If the enemy is at the bottom of the stave, nothing will happen.
	void moveLevelDown(){
		//Check if enemy is not at bottom of stave.
		if (currentLevel == 0)
			return;
		targetLevel = currentLevel - 1;
		moveVertical_ = true;
		moveStartTime = Time.time;
	}
	
	void moveHorizontalLeft(float destination){
		startX = transform.position.x;
		targetX = destination;
		moveHorizontal_ = true;
		moveStartTime = Time.time;
		currentMovementTime = Mathf.Abs(startX - targetX) / speed;
	}

	void moveHorizontalRight(float destination){
		startX = transform.position.x;
		targetX = destination;
		moveHorizontal_ = true;
		moveStartTime = Time.time;
		currentMovementTime = Mathf.Abs(targetX - startX) / speed;
	}
	//Play the sound death clip on command.
	void playSound(){
		 //audioPlayer.Play();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "player") {
			Destroy (other.gameObject);
		}
	}

	Action getBehavior(string behaviorName)
	{
		Action behavior = playSound;
		if (behaviorName == "charge")
			behavior = chargeBehavior;
		if (behaviorName == "flank")
			behavior = outFlankBehavior;
		return behavior;
	}
}
