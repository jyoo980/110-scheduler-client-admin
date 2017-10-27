using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSchedulerTypeHelper : MonoBehaviour {

    private ScheduleDto selectedSchedule;
    public ServerAPI serverApi;

    public void CancelDelete()
    {
        this.gameObject.SetActive(false);
    }

    public void ConfirmDelete() {
       // ServerAPI.
    }
}
