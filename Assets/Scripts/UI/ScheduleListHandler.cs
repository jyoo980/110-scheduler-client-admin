using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleListHandler : MonoBehaviour {

    public ServerAPI serverAPI;
    public GameObject scheduleAnchor;
    public GameObject schedulePrefab;
	public GameObject settingsPanel;
    public float verticalPadding = 32;

    public float originalVerticalSize = 320;
    private List<GameObject> scheduleList = new List<GameObject>();

    public void HandleAddedSchedules(ScheduleDto[] inboundSchedules) {
        //We add one here for esthetics, it's nice to have a bit of padding at the bottom of the list
        ResizeCanvas(inboundSchedules.Length + 1);
        DestroySchedules();
        GameObject previousSchedule = scheduleAnchor;
        foreach(ScheduleDto dto in inboundSchedules) {
            GameObject newSchedule = CreateNewScheduleObject(previousSchedule);
            scheduleList.Add(newSchedule);
            previousSchedule = newSchedule;
        }
    }

    public void AddSchedules(){
        serverAPI.GetAllSchedules(HandleAddedSchedules);
    }

    private GameObject CreateNewScheduleObject(GameObject previousSchedule) {
        GameObject newSchedule = Instantiate(schedulePrefab, previousSchedule.GetComponent<RectTransform>().position, Quaternion.identity);
        newSchedule.transform.SetParent(this.transform);
        newSchedule.transform.localScale = previousSchedule.transform.localScale;
        newSchedule.transform.localPosition = previousSchedule.transform.localPosition;
        RectTransform trans = newSchedule.GetComponent<RectTransform>();
        trans.localPosition = new Vector2(trans.localPosition.x, trans.localPosition.y - verticalPadding);
        return newSchedule;
    }

    //We need to make sure our scrollable list canvas is the correct size, and this is a function of the number
    // of elements of our list
    private void ResizeCanvas(int numberOfElements){
        RectTransform trans = GetComponent<RectTransform>();
        float curHeight = trans.rect.height;
        float desiredHeight = numberOfElements * verticalPadding;
        if(curHeight < desiredHeight){
            trans.sizeDelta = new Vector2(trans.sizeDelta.x, desiredHeight);
        }
    }
    private void DestroySchedules() {
        foreach (GameObject go in scheduleList) {
            Destroy(go);
        }
        scheduleList = new List<GameObject>();
    }

	public void HandleSettingsButton(){
		settingsPanel.SetActive (true);
	}
}
