using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleListHandler : MonoBehaviour {

    public GameObject scheduleAnchor;
    public GameObject schedulePrefab;

    private List<GameObject> scheduleList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSchedule() {
        GameObject newSchedule = Instantiate(schedulePrefab, scheduleAnchor.GetComponent<RectTransform>().position, Quaternion.identity);
        newSchedule.transform.parent = this.transform;
        newSchedule.transform.localScale = scheduleAnchor.transform.localScale;
        //newSchedule.GetComponent<RectTransform>().localPosition = scheduleAnchor.GetComponent<RectTransform>().localPosition;
    }
}
