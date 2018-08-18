using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScheduleTypeListHandler : MonoBehaviour 
{
	public ServerAPI serverAPI;
	public WeekScheduler weekScheduler;
	public Text text;
	public GameObject scheduleTypeAnchor;
	public GameObject scheduleTypePrefab;
	public float verticalPadding = 32;
	public float originalVerticalSize = 320;
	public SelectedScheduleTypeDisplayHelper selectedTypeDisplayHelper;
	private List<GameObject> scheduleTypeList = new List<GameObject>();
	private string selectedScheduleType;

	public void HandleAddedSchedules(ScheduleTypesDto inboundScheduleTypes) {
		string[] inboundSchedules = inboundScheduleTypes.GetSchedulesTypes();
		ResetCanvas(inboundSchedules);
		GameObject previousSchedule = scheduleTypeAnchor;
		foreach (var type in inboundSchedules) 
		{
			var newSchedule = CreateNewScheduleObject(previousSchedule, type);
			scheduleTypeList.Add(newSchedule);
			previousSchedule = newSchedule;
		}
	}

	private void ResetCanvas(string[] inboundSchedules)
	{
		Array.Sort(inboundSchedules);
		ResizeCanvas(inboundSchedules.Length + 1);
		DestroyOldSchedules();
	}

	public void AddScheduleTypes() 
	{
		serverAPI.GetAllScheduleTypes(HandleAddedSchedules);
	}

	private GameObject CreateNewScheduleObject(GameObject previousSchedule, string type) 
	{
		GameObject newSchedule = Instantiate(scheduleTypePrefab, previousSchedule.GetComponent<RectTransform>().position, Quaternion.identity);
		newSchedule.GetComponent<ScheduleTypeButtonHelper>().SetupHelper(this, selectedTypeDisplayHelper, type);
		newSchedule.transform.SetParent(this.transform);
		newSchedule.transform.localScale = previousSchedule.transform.localScale;
		newSchedule.transform.localPosition = previousSchedule.transform.localPosition;
		RectTransform trans = newSchedule.GetComponent<RectTransform>();
		trans.localPosition = new Vector2(trans.localPosition.x, trans.localPosition.y - verticalPadding);
		return newSchedule;
	}

	private void ResizeCanvas(int numberOfElements) 
	{
		RectTransform trans = GetComponent<RectTransform>();
		float curHeight = trans.rect.height;
		float desiredHeight = numberOfElements * verticalPadding;
		trans.sizeDelta = new Vector2(trans.sizeDelta.x, desiredHeight);

	}

	private void DestroyOldSchedules()
	{
		foreach (var gameObj in scheduleTypeList)
		{
			Destroy(gameObj);
		}
		scheduleType = new List<GameObject>();
	}

	public void HandleSelectScheduleType(string selectedScheduleType) 
	{
		this.selectedScheduleType = selectedScheduleType;
	}

	public string GetSelectedScheduleType() 
	{
		return selectedScheduleType;
	}

	public bool DoesContainGivenScheduleType(string scheduleType)
	{
		foreach (var gameObj in scheduleTypeList)
		{
			var helper = gameObj.GetComponent<ScheduleTypeButtonHelper>();
			if (helper != null && IsEqualScheduleType(helper, scheduleType))
				return true;
		}
		return false;
	}

	private bool IsEqualScheduleType(ScheduleTypeButtonHelper helper, string scheduleType)
	{
		return helper.GetMyScheduleType().Trim().ToLower().Equals(scheduleType);
	}
}
