using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelChange : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler  {

	public bool isSelected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isSelected) {
			if (Input.GetAxis ("LeftTrigger") > 0) 
			{
				Application.LoadLevel (1);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventdata)
	{
	}
	public void OnSelect(BaseEventData eventdata)
	{
		isSelected = true;

	}

	public void OnDeselect(BaseEventData eventdata)
	{
		isSelected = false;
	}
}
