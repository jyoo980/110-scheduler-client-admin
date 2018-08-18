using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddScheduleHelper : MonoBehaviour 
{
	public ServerAPI serverApi;
	public Text newScheduleType;
	public ScheduleTypeListHandler listHandler;

	public void AddSchedule()
	{
		string formattedScheduleType = GetFormattedScheduleType(newScheduleType);
		if (!ScheduleExists(formattedScheduleType) && formattedScheduleType != "")
		{
			serverApi.AddScheduleType(formattedScheduleType, listHandler.AddScheduleTypes);
			newScheduleType.text = "";
		}
	}

	private void GetFormattedScheduleType(Text scheduleType)
	{
		var formattedScheduleType = scheduleType.text.Trim().ToLower();
		return (formattedScheduleType.Length != 0) ? formattedScheduleType : "";
	}

	private void ScheduleExists(string scheduleType)
	{
		return listHandler.DoesContainGivenScheduleType(scheduleType);
	}
}
