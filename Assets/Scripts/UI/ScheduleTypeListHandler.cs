using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ScheduleTypeListHandler : MonoBehaviour {

	public ServerAPI serverAPI;
	public WeekScheduler weekScheduler;
	public Text text;
	public GameObject scheduleTypeAnchor;
	public GameObject scheduleTypePrefab;
	public float verticalPadding = 32;

	public float originalVerticalSize = 320;
	private List<GameObject> scheduleTypeList = new List<GameObject>();
	private string selectedScheduleType;
	public SelectedScheduleTypeDisplayHelper selectedTypeDisplayHelper;


	public void HandleAddedSchedules(ScheduleTypesDto inboundScheduleTypes) {
		string[] inboundSchedules = inboundScheduleTypes.GetSchedulesTypes();
		Array.Sort (inboundSchedules);
		//We add one here for esthetics, it's nice to have a bit of padding at the bottom of the list
		ResizeCanvas(inboundSchedules.Length + 1);
		DestroyOldSchedules();
		GameObject previousSchedule = scheduleTypeAnchor;
		foreach(string type in inboundSchedules) {
			GameObject newSchedule = CreateNewScheduleObject(previousSchedule, type);
			scheduleTypeList.Add(newSchedule);
			previousSchedule = newSchedule;
		}
	}

	public void AddScheduleTypes(){
		serverAPI.GetAllScheduleTypes(HandleAddedSchedules);
	}

	private GameObject CreateNewScheduleObject(GameObject previousSchedule, string type) {
		GameObject newSchedule = Instantiate(scheduleTypePrefab, previousSchedule.GetComponent<RectTransform>().position, Quaternion.identity);
		newSchedule.GetComponent<ScheduleTypeButtonHelper>().SetupHelper(this, selectedTypeDisplayHelper, type);
		newSchedule.transform.SetParent(this.transform);
		newSchedule.transform.localScale = previousSchedule.transform.localScale;
		newSchedule.transform.localPosition = previousSchedule.transform.localPosition;
		RectTransform trans = newSchedule.GetComponent<RectTransform>();
		trans.localPosition = new Vector2(trans.localPosition.x, trans.localPosition.y - verticalPadding);
		return newSchedule;
	}

	//We need to make sure our scrollable list canvas is the correct size, and this is a function of the number
	// of elements of our list
	private void ResizeCanvas(int numberOfElements) {
		RectTransform trans = GetComponent<RectTransform>();
		float curHeight = trans.rect.height;
		float desiredHeight = numberOfElements * verticalPadding;
		trans.sizeDelta = new Vector2(trans.sizeDelta.x, desiredHeight);

	}
	private void DestroyOldSchedules() {
		foreach (GameObject go in scheduleTypeList) {
			Destroy(go);
		}
		scheduleTypeList = new List<GameObject>();
	}

	public void HandleSelectScheduleType(string selectedScheduleType) {
		this.selectedScheduleType = selectedScheduleType;
	}

	public string GetSelectedScheduleType() {
		return selectedScheduleType;
	}

	public bool DoesContainGivenScheduleType(string scheduleType){
		foreach (GameObject go in scheduleTypeList) {
			ScheduleTypeButtonHelper helper = go.GetComponent<ScheduleTypeButtonHelper> ();
			if (helper != null) {
				if (helper.GetMyScheduleType ().Trim ().ToLower ().Equals (scheduleType)) {
					return true;
				}
			}
		}
		return false;
	}
}
