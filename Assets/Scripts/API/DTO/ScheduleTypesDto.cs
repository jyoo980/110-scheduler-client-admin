using UnityEngine;

//Data transfer object to represent inbound and outbound schedule state. Please have this match 1:1 to server-side DTO.

[SerializeField]
public class ScheduleTypesDto : JsonEncodableData {

    [SerializeField]
    string[] scheduleTypes = null;

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
