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
    private string date;
    private const string EMPTY_NAME = "";

	public void OnSelect(BaseEventData eventData) {
        listHandler.HandleSelectSchedule(mySchedule);
        selectedDisplayHelper.SetupNameAndType(firstName, lastName, date, scheduleType);
    }

    public void SetupHelper(ScheduleListHandler handler, SelectedScheduleDisplayHelper helper, ScheduleDto dto) {
        this.selectedDisplayHelper = helper;
        this.listHandler = handler;
        this.mySchedule = dto;
        Text[] textFields = GetComponentsInChildren<Text>();
        string[] firstAndLast = dto.GetTaName().Split('_');
        handleFirstAndLastNames(firstAndLast);
        date = dto.GetDate();
        scheduleType = dto.GetScheduleType();
        textFields[0].text = firstName + " " + lastName;
        textFields[1].text = date.Split('.')[0];
        textFields[2].text = scheduleType;
    }

    private void handleFirstAndLastNames(string[] firstAndLastName)
    {
        string firstNameRaw = firstAndLastName[0];
        string lastNameRaw = firstAndLastName[1];
        if (firstNameRaw.Length == 0 || firstNameRaw.Equals("")){
            firstName = EMPTY_NAME;
        }
        else{
            firstName = firstNameRaw.Substring(0, 1).ToUpper() + firstNameRaw.Substring(1, firstNameRaw.Length - 1);
        }
        
        if (lastNameRaw.Length == 0 || lastNameRaw.Equals("")){
            lastName = EMPTY_NAME;
        }
        else{
            lastName = lastNameRaw.Substring(0, 1).ToUpper() + lastNameRaw.Substring(1, lastNameRaw.Length - 1);
        }
    }
}
