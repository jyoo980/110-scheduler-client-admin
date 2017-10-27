using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedScheduleTypeDisplayHelper : MonoBehaviour {

	public void SetupType(string scheduleType) {
		Text text = GetComponentInChildren<Text>();
		text.text = scheduleType;
	}

	public void ClearType() {
		Text text = GetComponentInChildren<Text>();
		text.text = "";
	}
}
