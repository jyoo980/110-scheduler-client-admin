using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSchedulerTypeHelper : MonoBehaviour {

    private string selectedScheduleType;
    public ServerAPI serverApi;
	public ScheduleTypeListHandler listHandler;
	public SelectedScheduleTypeDisplayHelper displayHelper;

	public void TogglePanel() {
		selectedScheduleType = listHandler.GetSelectedScheduleType();
		if (selectedScheduleType != null) {
			gameObject.SetActive(true);
		}
	}

    public void CancelDelete() {
        this.gameObject.SetActive(false);
    }

    public void ConfirmDelete() {
		serverApi.DeleteScheduleType(selectedScheduleType, listHandler.AddScheduleTypes);
		selectedScheduleType = null;
		listHandler.HandleSelectScheduleType(null);
		displayHelper.ClearType();
		gameObject.SetActive(false);    
	}
}
