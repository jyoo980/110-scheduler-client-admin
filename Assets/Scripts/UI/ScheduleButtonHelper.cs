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

	public void OnSelect(BaseEventData eventData) 
    {
        listHandler.HandleSelectSchedule(mySchedule);
        selectedDisplayHelper.SetupNameAndType(firstName, lastName, date, scheduleType);
    }

    public void SetupHelper(ScheduleListHandler handler, SelectedScheduleDisplayHelper helper, ScheduleDto dto) 
    {
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

    private void handleFirstAndLastNames(string[] fullName)
    {
        var firstNameRaw = fullName[0];
        var lastNameRaw = fullName[1];
        setFirstName(firstNameRaw);
        setLastName(lastNameRaw);
    }

    private void setFirstName(string firstName)
    {
        this.firstName = (String.IsNullOrEmpty(firstName))
            ? EMPTY_NAME
            : firstName.Substring(0, 1).ToUpper() + firstName.Substring(1, firstName.Length - 1);
    }

    private void setLastName(string lastName)
    {
        this.lastName = (String.IsNullOrEmpty(lastName))
            ? EMPTY_NAME
            : lastName = lastName.Substring(0, 1).ToUpper() + lastName.Substring(1, lastName.Length - 1);
    }
}
