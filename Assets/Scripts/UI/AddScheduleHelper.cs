using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddScheduleHelper : MonoBehaviour {

	public ServerAPI serverApi;
	public Text newScheduleType;
	public ScheduleTypeListHandler listHandler;

	public void AddSchedule(){
		string formattedScheduleType = newScheduleType.text.Trim ().ToLower ();
		if (formattedScheduleType.Length == 0) {
			return;
		}
		if (!listHandler.DoesContainGivenScheduleType (formattedScheduleType)) {
			serverApi.AddScheduleType (formattedScheduleType, listHandler.AddScheduleTypes);
			newScheduleType.text = "";
		}
	}
}
