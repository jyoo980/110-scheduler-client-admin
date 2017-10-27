using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSelectedHelper : MonoBehaviour {

    public GameObject weekPanel;
    public ScheduleListHandler handler;

    public void OpenWeekPanel() {
        if (handler.GetSelectedSchedule() != null) {
            weekPanel.SetActive(true);
        }
    }

    public void CloseWeekPanel() {
        weekPanel.SetActive(false);
    }
}
