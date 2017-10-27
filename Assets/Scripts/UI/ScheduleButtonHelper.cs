using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScheduleButtonHelper : MonoBehaviour, ISelectHandler
{
    private ScheduleDto mySchedule;
    private ScheduleListHandler listHandler;
    private SelectedScheduleDisplayHelper selectedDisplayHelper;
    private string firstName;
    private string lastName;
    private string scheduleType;

	public void OnSelect(BaseEventData eventData) {
        listHandler.HandleSelectSchedule(mySchedule);
        selectedDisplayHelper.SetupNameAndType(firstName, lastName, scheduleType);
    }

    public void SetupHelper(ScheduleListHandler handler, SelectedScheduleDisplayHelper helper, ScheduleDto dto) {
        this.selectedDisplayHelper = helper;
        this.listHandler = handler;
        this.mySchedule = dto;
        Text[] textFields = GetComponentsInChildren<Text>();
        string[] firstAndLast = dto.GetTaName().Split('_');
        firstName = firstAndLast[0].Substring(0, 1).ToUpper() + firstAndLast[0].Substring(1, firstAndLast[0].Length - 1);
        lastName = firstAndLast[1].Substring(0, 1).ToUpper() + firstAndLast[1].Substring(1, firstAndLast[1].Length - 1);
        scheduleType = dto.GetScheduleType();
        textFields[0].text = firstName + " " + lastName;
        textFields[1].text = scheduleType;
    }
}
