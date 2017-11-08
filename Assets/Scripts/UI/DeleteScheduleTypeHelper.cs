using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteScheduleTypeHelper : MonoBehaviour {

    private string selectedScheduleType;
    public ServerAPI serverApi;
	public ScheduleListHandler listHandler;
	public ScheduleTypeListHandler listTypeHandler;
	public SelectedScheduleTypeDisplayHelper displayHelper;

	public void TogglePanel() {
		selectedScheduleType = listTypeHandler.GetSelectedScheduleType();
		if (selectedScheduleType != null) {
			gameObject.SetActive(true);
		}
	}

    public void CancelDelete() {
        this.gameObject.SetActive(false);
    }

    public void ConfirmDelete() {
		serverApi.DeleteScheduleType(selectedScheduleType, RefreshSchedulesAndTypes);
		selectedScheduleType = null;
		listTypeHandler.HandleSelectScheduleType(null);
		displayHelper.ClearType();
		gameObject.SetActive(false);    
	}

	private void RefreshSchedulesAndTypes(){
		listTypeHandler.AddScheduleTypes ();
		listHandler.AddSchedules ();
	}
}
