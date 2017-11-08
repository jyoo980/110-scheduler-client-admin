using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScheduleTypeButtonHelper : MonoBehaviour, ISelectHandler
{
	private string myScheduleType;
	private ScheduleTypeListHandler listHandler;
	private SelectedScheduleTypeDisplayHelper selectedDisplayHelper;
	private string firstName;
	private string lastName;

	public void OnSelect(BaseEventData eventData) {
		listHandler.HandleSelectScheduleType(myScheduleType);
		selectedDisplayHelper.SetupType(myScheduleType);
	}

	public void SetupHelper(ScheduleTypeListHandler handler, SelectedScheduleTypeDisplayHelper helper, string type) {
		this.selectedDisplayHelper = helper;
		this.listHandler = handler;
		this.myScheduleType = type;
		Text typeField = GetComponentInChildren<Text>();
		typeField.text = type;
	}

	public string GetMyScheduleType(){
		return myScheduleType;
	}
}
