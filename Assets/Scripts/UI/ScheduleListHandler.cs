using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
public class ScheduleListHandler : MonoBehaviour {

    public ServerAPI serverAPI;
    public ScheduleTypeListHandler typeListHandler;
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
    public Text selectedScheduleType;


    void Awake() {
        //Attempt to load URL from file so user doesn't have to manually enter
        string path = System.Environment.CurrentDirectory + "/url.txt";
        string url = File.ReadAllText(path);
        if (url != null && url.Length > 0) {
            PlayerPrefs.SetString(Settings.URL_KEY, url);
        }
    }

    public void HandleAllAddedSchedules(ScheduleListDto inboundSchedulesDto) {
        HandleAddedSchedules(inboundSchedulesDto, true);
    }

    public void HandleSelectedAddedSchedules(ScheduleListDto inboundSchedulesDto) {
        HandleAddedSchedules(inboundSchedulesDto, false);
    }

    private void HandleAddedSchedules(ScheduleListDto inboundSchedulesDto, bool allSchedules) {
        ScheduleDto[] inboundSchedules = inboundSchedulesDto.GetSchedules();
        Array.Sort(inboundSchedules, (schedOne, schedTwo) => {
            try
            {
                string[] firstDateTimePartition = schedOne.GetDate().Replace("/", " ").Replace(":", " ").Split(' ');
                int firstDateMonth = int.Parse(firstDateTimePartition[0]);
                int firstDateDay = int.Parse(firstDateTimePartition[1]);
                int firstDateYear = int.Parse(firstDateTimePartition[2]);
                int firstDateHour = int.Parse(firstDateTimePartition[3]);
                int firstDateMinute = int.Parse(firstDateTimePartition[4]);
                int firstDateSecond = int.Parse(firstDateTimePartition[5]);
                DateTime firstDate = new DateTime(firstDateYear, firstDateMonth, firstDateDay, firstDateHour, firstDateMinute, firstDateSecond);
                string[] secondDateTimePartition = schedTwo.GetDate().Replace("/", " ").Replace(":", " ").Split(' ');

                int secondDateMonth = int.Parse(secondDateTimePartition[0]);
                int secondDateDay = int.Parse(secondDateTimePartition[1]);
                int secondDateYear = int.Parse(secondDateTimePartition[2]);
                int secondDateHour = int.Parse(secondDateTimePartition[3]);
                int secondDateMinute = int.Parse(secondDateTimePartition[4]);
                int secondDateSecond = int.Parse(secondDateTimePartition[5]);
                DateTime secondDate = new DateTime(secondDateYear, secondDateMonth, secondDateDay, secondDateHour, secondDateMinute, secondDateSecond);
                return secondDate.CompareTo(firstDate);
            }
            //It's possible one of the dates have foreign characters, so let's just return 0 here
            catch (Exception e) {
                return 0;
            }
        });

        if (!allSchedules) {
            inboundSchedules = inboundSchedules.Where(schedule => schedule.GetScheduleType().Trim().ToLower().Equals(selectedScheduleType.text.Trim().ToLower())).ToArray();
        }
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
		} else if(typeListHandler.GetSelectedScheduleType() == null || typeListHandler.GetSelectedScheduleType().Length == 0){
			text.text = "Please select a schedule type first";
		} else {
			string path = weekScheduler.GenerateAllWeeklySchedules (scheduleDtos.Where(sched => 
				sched.GetScheduleType().Trim().ToLower().Equals(typeListHandler.GetSelectedScheduleType().Trim().ToLower())).ToArray());
			text.text = "Saving complete! You can find your schedules in: " + path;
		}
	}

    public void AddSchedules() {
        serverAPI.GetAllSchedules(HandleAllAddedSchedules);
    }

    public void AddSelectedSchedules() {
        serverAPI.GetAllSchedules(HandleSelectedAddedSchedules);
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
