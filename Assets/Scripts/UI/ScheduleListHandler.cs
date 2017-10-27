using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ScheduleListHandler : MonoBehaviour {

    public ServerAPI serverAPI;
	public WeekScheduler weekScheduler;
	public Text text;
    public GameObject scheduleAnchor;
    public GameObject schedulePrefab;
	public GameObject settingsPanel;
    public float verticalPadding = 32;

    public float originalVerticalSize = 320;
    private List<GameObject> scheduleList = new List<GameObject>();
	private ScheduleDto[] scheduleDtos = null;
    private ScheduleDto selectedSchedule;
    public SelectedScheduleDisplayHelper selectedDisplayHelper;


    public void HandleAddedSchedules(ScheduleListDto inboundSchedulesDto) {
        ScheduleDto[] inboundSchedules = inboundSchedulesDto.GetSchedules();
        Array.Sort(inboundSchedules, (schedOne, schedTwo) => {
            return schedOne.GetTaName().CompareTo(schedTwo.GetTaName());
        });
        //We add one here for esthetics, it's nice to have a bit of padding at the bottom of the list
        ResizeCanvas(inboundSchedules.Length + 1);
        DestroyOldSchedules();
		scheduleDtos = inboundSchedules;
        GameObject previousSchedule = scheduleAnchor;
        foreach(ScheduleDto dto in inboundSchedules) {
            GameObject newSchedule = CreateNewScheduleObject(previousSchedule, dto);
            scheduleList.Add(newSchedule);
            previousSchedule = newSchedule;
        }
    }

	public void HandleDownloadSchedules(){
		if (scheduleDtos == null || scheduleDtos.Length == 0) {
			text.text = "There are no schedules to download! Please load them first";
		} else {
			string path = weekScheduler.GenerateAllWeeklySchedules (scheduleDtos);
			text.text = "Saving complete! You can find your schedules in: " + path;
		}
	}

    public void AddSchedules(){
        serverAPI.GetAllSchedules(HandleAddedSchedules);
    }

    private GameObject CreateNewScheduleObject(GameObject previousSchedule, ScheduleDto dto) {
        GameObject newSchedule = Instantiate(schedulePrefab, previousSchedule.GetComponent<RectTransform>().position, Quaternion.identity);
        newSchedule.GetComponent<ScheduleButtonHelper>().SetupHelper(this, selectedDisplayHelper, dto);
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
        foreach (GameObject go in scheduleList) {
            Destroy(go);
        }
        scheduleList = new List<GameObject>();
	    scheduleDtos = null;
    }

	public void HandleSettingsButton(){
		settingsPanel.SetActive (true);
	}

    public void HandleSelectSchedule(ScheduleDto selectedSchedule) {
        this.selectedSchedule = selectedSchedule;
    }

    public ScheduleDto GetSelectedSchedule() {
        return selectedSchedule;
    }
}
