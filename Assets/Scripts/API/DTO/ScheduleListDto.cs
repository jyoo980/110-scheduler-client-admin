using UnityEngine;

[System.Serializable]
public class ScheduleListDto : JsonEncodableData {

    [SerializeField]
    ScheduleDto[] schedules = new ScheduleDto[0];

    public ScheduleListDto(ScheduleDto[] schedules) {
        this.schedules = schedules;
    }

    public ScheduleDto[] GetSchedules() {
        return schedules;
    }

    public void SetSchedules(ScheduleDto[] schedules) {
        this.schedules = schedules;
    }
}
