using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AI : MonoBehaviour {

	//Actions allow the passing of functions as though they were
	//variables. behavior is set to the desired behavior function, 
	//which is then called every update to control the enemy.
	private Action behavior;

	//audioPlayer is the enemys audio player component. It is set to this on start,
	//so the enemy object must have an audio player component. deathSound is then 
	//set to be the audio players audio clip. 
	private AudioSource audioPlayer;
	public AudioClip deathSound;

	//Used to access the players position and script.
	private GameObject player;
	//Gives the enemy access to the players current stave level, via a variable.
	private PlayerScript playerScript;

	//Values used for moving vertically. They are used to access values in the 
	//levels array, which are then lerped between.
	private int currentLevel, targetLevel;

	//moveStartTime is used for lep movement, it is set at the start of the movement
	//to the current time. This occurs in the moveLevelUp and Down functions
	//and the moveHorizontalLeft and Right functions. X is for the latter, Y is 
	//for the later.
	private float moveStartTimeX, moveStartTimeY;

	//StartX and targetX are used for horizontal movement. They are values to 
	//lerp between and are set whenever moveHorizontalLeft and Right are called.
	//Width is the enemys width, set in start.
	//Time is filled with the current time at the start of moveHorizontal and moveVertical.
	//CurrentMovementTime is use for horizontal movement. It is filled with the time the
	//current movement is expected to take in the moveHorizontal left and right functions,
	//and is the n used to get the lerp 0-1 value.
	private float startX, targetX, width, time, currentMovementTime;
	private float[] levels;

	//Values used to indicating whether the enmey is moving. Movements will not be 
	//interupted whilst either of these is set to true.
	private bool moveVertical_ = false;
	private bool moveHorizontal_ = false;

	//Below, used for zigzag movement, to determine direction of vertical movement.
	private bool moveUpOrDown = false;

	public float staveStart, staveEnd;
	//Below, the time it takes to change stave level.
	public float moveLevelTime = 0.1f;
	//The time the current horizontal movement will take.
	public float speed = 80.0f;
	//Below, the gap between each stave level. Used for filling
	//the levels array.
	public float staveGap = 1f;
	//Used for setting the behavior of the enemy when start is called.
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

		//Uncomment once deathSound has been added.
		//audioPlayer = gameObject.GetComponent<AudioSource> ();
		//audioPlayer.clip = deathSound;		
	}

	// Update is called once per frame
	void Update () {
		
		if (transform.position.x <= -24.9f) {
			//Uncomment once deathSound has been added.
			//playSound();
			gameObject.SetActive(false);
			return;
		}
		behavior();
	}

	//MonoDevelop needs you to go tools->options->general->enable code folding to
	//use regions properly.
	#region VERTICAL_MOVEMENT
	/*The region below contains the functions required to execute vertical movement.
	 -moveLevelUp or moveLevelDown are called once at the start of any vertical movement.
	 -These two functions set the tartget level to be either one above or one below the 
	  current level. They also set moveVertical_ to true, and set the float moveStartTime
	  to the current time.
	 -Once this has been done, the enemys behavior function will call moveVertical untill
	  the target level has been reached. moveVertical lerps between current level and
	  target level, extracting a 0-1 value using moveStartTime, moveLevelTime, and the value 
	  time, which is set to the current time each call.
	 -At the start of each call of moveVertical, the function checks if the time elapsed since
	  the movement started is greater than the time required for the movement to be completed.
	  If this is the case, current level is set to target level, moveVertical_ is set to false,
	  and the function immidiately ends.*/


	//Instruct the enemy to begin moving up one stave level.
	//If the enemy is at the top of the stave, nothing will happen.
	void moveLevelUp(){
		//Check if enemy is not at top of stave.
		if (currentLevel == levels.Length)
			return;
		targetLevel = currentLevel + 1;
		moveVertical_ = true;
		moveStartTimeX = Time.time;
	}
	
	//Instruct the enemy to begin moving down one stave level.
	//If the enemy is at the bottom of the stave, nothing will happen.
	void moveLevelDown(){
		//Check if enemy is not at bottom of stave.
		if (currentLevel == 0)
			return;
		targetLevel = currentLevel - 1;
		moveVertical_ = true;
		moveStartTimeX = Time.time;
	}

	//This function is called each tick whilst the enemy is moving.
	//It lerps the enemy towards the target height, untill the enemy has 
	//reached the correct stave.
	void moveVertical(){
		time = Time.time;
		//Check if the movement has been completed, and return if it has.
		if(time - moveStartTimeX > moveLevelTime){
			moveVertical_ = false;
			currentLevel = targetLevel;
			return;
		}
		//Move towards the target level based on the time the enemy has been moving.
		transform.position = new Vector3(transform.position.x, 
			                                        Mathf.Lerp (levels[currentLevel], levels[targetLevel], 
			            			((time - moveStartTimeX) / moveLevelTime)),
			                                        transform.position.z);
	}
	#endregion VERTICAL_MOVEMENT

	#region HORIZONTAL_MOVEMENT
	/*The region below contains the functions required to execute horizontal movement.
	 -The functions moveHorizontalLeft and moveHorizontalRight are called at the start
	  of any horizontal movement.
	 -They set the values startX and targetX, which are later lerped between. They also
	  set moveHorizontal_ to true, and moveStartTime to the current time. Finally, they
	  get the currentMovementTime, which is the time the movement should take. They determine
	  currentMovementTime by dividing the distance to be moved by the speed of the enemy.*/

	//Instructs the enemy to begin moving towards destination.
	void moveHorizontalLeft(float destination){
		startX = transform.position.x;
		targetX = destination;
		moveHorizontal_ = true;
		moveStartTimeY = Time.time;
		currentMovementTime = Mathf.Abs(startX - targetX) / speed;
	}

	//Instructs the enemy to begin moving towards destination.
	void moveHorizontalRight(float destination){
		startX = transform.position.x;
		targetX = destination;
		moveHorizontal_ = true;
		moveStartTimeY = Time.time;
		currentMovementTime = Mathf.Abs(targetX - startX) / speed;
	}

	//Fuction is called each tick whilst the player is moving horizontally. The function
	//lerps between startX and targetX untill enemy has been moving longer than 
	//currentMovementTime.
	void moveHorizontal(){
		time = Time.time;
		if (time - moveStartTimeY > currentMovementTime) {
			moveHorizontal_ = false;
			transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
			return;
		}
		transform.position = new Vector3(Mathf.Lerp(startX, targetX,
			                                       (time - moveStartTimeY) / currentMovementTime),
			                                        transform.position.y, transform.position.z);
	}
	#endregion HORIZONTAL_MOVEMENT

	#region BEHAVIORS
	//Name charge.
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

	//Name flank.
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
		else if (transform.position.x > player.transform.position.x)
			moveHorizontalLeft (player.transform.position.x - (Mathf.Abs(player.transform.position.x - staveEnd)/2));
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

	//Name zigzag.
	void zigZag(){
		//Start a movement towards the end of the stave the first time the function is called.
		if (!moveHorizontal_) {
			moveHorizontalLeft(staveEnd);
			return;
		}
		//Move towards the end of the stave.
		moveHorizontal();
		//If you're already moving up, don't do anything else.
		if (moveVertical_) {
			moveVertical();
			return;
		}
		//If you're at the bottom, set move up or down.
		if (currentLevel == 0)
			moveUpOrDown = true;
		//If you're at the top, set move up or down.
		if (currentLevel == 8)
			moveUpOrDown = false;
		//If moveUporDown, move up one level, and have a 50/50 chance of switching vertical direction.
		if (moveUpOrDown) {
			moveLevelUp ();
			switch (UnityEngine.Random.Range (0, 2)) {
			case 0:
				moveUpOrDown = false;
				break;
			case 1:
				break;
			}
		} 
		//If !moveUporDown, move down one level, and have a 50/50 chance of switching vertical direction.
		else {
			moveLevelDown ();
			switch (UnityEngine.Random.Range (0, 2)) {
			case 0:
				moveUpOrDown = true;
				break;
			case 1:
				break;
			}
		}
	}
	//Sets behavior to the correct function based on the argument behavior name.
	Action getBehavior(string behaviorName)
	{
		Action behavior = playSound;
		if (behaviorName == "charge")
			behavior = chargeBehavior;
		else if (behaviorName == "flank")
			behavior = outFlankBehavior;
		else if (behaviorName == "zigzag")
			behavior = zigZag;
		return behavior;
	}

	//Sets the behavior of the player, and also sets it's current level and
	// it's movement booleans to false.
	public void setBehavior(string behavior_, int level){
		behavior = getBehavior (behavior_);
		moveHorizontal_ = false;
		moveVertical_ = false;
		currentLevel = level;
	}
	#endregion BEHAVIORS
	

	//Play the sound death clip on command.
	void playSound(){
		 audioPlayer.Play();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "player") {
			Destroy (other.gameObject);
		}
	}


}
