using UnityEngine;

//Data transfer object to represent inbound and outbound schedule state. Please have this match 1:1 to server-side DTO.

[System.Serializable]
public class ScheduleTypesDto : JsonEncodableData {

    [SerializeField]
    string[] scheduleTypes = new string[0];

    public ScheduleTypesDto(string[] scheduleTypes) {
        this.scheduleTypes = scheduleTypes;
    }

    public void SetScheduleTypes(string[] scheduleTypes)
    {
        this.scheduleTypes = scheduleTypes;
    }

    public string[] GetSchedulesTypes()
    {
        return scheduleTypes;
    }
}
