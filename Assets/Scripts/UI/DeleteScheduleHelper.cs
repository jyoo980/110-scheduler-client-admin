using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScheduleHelper : MonoBehaviour {

    private ScheduleDto selectedSchedule;
    public ServerAPI serverApi;
    public ScheduleListHandler listHandler;
    public SelectedScheduleDisplayHelper displayHelper;

    public void TogglePanel() {
        selectedSchedule = listHandler.GetSelectedSchedule();
        if (selectedSchedule != null) {
            gameObject.SetActive(true);
        }
    }

    public void CancelDelete() {
        gameObject.SetActive(false);
    }

    public void ConfirmDelete() {
        serverApi.DeleteSchedule(selectedSchedule.GetTaName(), selectedSchedule.GetScheduleType(), listHandler.AddSchedules);
        selectedSchedule = null;
        listHandler.HandleSelectSchedule(null);
        displayHelper.ClearNameAndType();
        gameObject.SetActive(false);
    }
}
