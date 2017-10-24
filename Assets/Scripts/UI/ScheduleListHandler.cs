using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleListHandler : MonoBehaviour {

    public GameObject scheduleAnchor;
    public GameObject schedulePrefab;
    public float verticalPadding = 32;
    private List<GameObject> scheduleList = new List<GameObject>();

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void AddSchedule(List<ScheduleDto> inboundSchedules) {
        DestroySchedules();
        GameObject previousSchedule = scheduleAnchor;
        foreach(ScheduleDto dto in inboundSchedules) {
            GameObject newSchedule = Instantiate(schedulePrefab, previousSchedule.GetComponent<RectTransform>().position, Quaternion.identity);
            newSchedule.transform.SetParent(this.transform);
            newSchedule.transform.localScale = previousSchedule.transform.localScale;
            newSchedule.transform.localPosition = previousSchedule.transform.localPosition;
            RectTransform trans = newSchedule.GetComponent<RectTransform>();
            trans.localPosition = new Vector2(trans.localPosition.x, trans.localPosition.y - verticalPadding);

            scheduleList.Add(newSchedule);
            previousSchedule = newSchedule;
        }
    }

    private void SetPositionAndScale() {

    }

    public void AddSchedule() {
        ScheduleDto first = new ScheduleDto("andypandy", "midterm1", new string[0]);
        ScheduleDto second = new ScheduleDto("andy    pandy", "regular", new string[0]);
        ScheduleDto third = new ScheduleDto("bo bandyyyy", "midterm1", new string[0]);
        AddSchedule(new List<ScheduleDto>() { first, second, third });
    }

    private void DestroySchedules() {
        foreach (GameObject go in scheduleList) {
            Destroy(go);
        }
        scheduleList = new List<GameObject>();
    }
}
