using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class WeekScheduler : MonoBehaviour {

    private const int DAYS_IN_WEEK = 7;
    private const int TIME_SLOTS_PER_DAY = 27;
    private const string AVAILABLE = "Available ";
    private const string NOT_AVAILABLE = "Not Available ";
    private const string PREFER_NOT = "Prefer Not ";
    private const string NEW_LINE = "\n";

    //Day strings
    private string[] HOURS_IN_DAY = new string[] {
        "8:00 ",
        "8:30 ",
        "9:00 ",
        "9:30 ",
        "10:00 ",
        "10:30 ",
        "11:00 ",
        "11:30 ",
        "12:00 ",
        "12:30 ",
        "13:00 ",
        "13:30 ",
        "14:00 ",
        "14:30 ",
        "15:00 ",
        "15:30 ",
        "16:00 ",
        "16:30 ",
        "17:00 ",
        "17:30 ",
        "18:00 ",
        "18:30 ",
        "19:00 ",
        "19:30 ",
        "20:00 ",
        "20:30 ",
        "21:00 "};

    private Dictionary<SelectionHandler.Selection, string> selectionDictionary;

    [SerializeField]
    private Dropdown targetSchedules;

    [SerializeField]
    private Text targetScheduleLabel;

    [SerializeField]
    private InputField firstNameField;

    [SerializeField]
    private InputField lastNameField;

    [SerializeField]
    private ServerAPI serverApi;

    [SerializeField]
    private Dropdown scheduleType;

    private void Awake () {
        LoadScheduleDictionary();
        //LoadFirstAndLastName();
        //HandleLoadScheduleTypes();
    }

    private void LoadScheduleDictionary() {
        selectionDictionary = new Dictionary<SelectionHandler.Selection, string>();
        selectionDictionary[SelectionHandler.Selection.Available] = AVAILABLE;
        selectionDictionary[SelectionHandler.Selection.NotAvailable] = NOT_AVAILABLE;
        selectionDictionary[SelectionHandler.Selection.PreferNot] = PREFER_NOT;
    }

    public string GenerateWeeklyScheduleString() {
        DayScheduler[] daySchedules = GetComponentsInChildren<DayScheduler>();
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < TIME_SLOTS_PER_DAY; i++) {
            builder.Append(HOURS_IN_DAY[i]);
            for (int j = 0; j < DAYS_IN_WEEK; j++) {
                builder.Append(selectionDictionary[daySchedules[j].GenerateDailySelections()[i]]);
            }
            builder.Append(NEW_LINE);
        }
        return builder.ToString();
    }

    public string[] GenerateAbbreviatedWeeklySchedule() {
        DayScheduler[] daySchedules = GetComponentsInChildren<DayScheduler>();
        string[] weeklySchedule = new string[7];
        for (int i = 0; i < daySchedules.Length; i++){
            weeklySchedule[i] = daySchedules[i].GenerateDailySelectionByAbbreviation();
        }
        return weeklySchedule;
    }

    public void DistributeWeeklyScheduleFromServer(string[] abbreviatedSchedules) {
        DayScheduler[] daySchedules = GetComponentsInChildren<DayScheduler>();
        for (int i = 0; i < daySchedules.Length; i++) {
            daySchedules[i].DistributeDailyScheduleFromServer(abbreviatedSchedules[i]);
        }
    }

    public void HandleLoadSchedule() {
        serverApi.GetSchedule(GenerateTAName(), targetScheduleLabel.text.Trim().ToLower(), HandleScheduleLoadFinished);
    }

    public void HandleScheduleLoadFinished(ScheduleDto scheduleDto) {
        DistributeWeeklyScheduleFromServer(scheduleDto.GetSchedulesByDay());
    }

    public void HandleLoadScheduleTypes() {
        serverApi.GetScheduleTypes(HandleScheduleTypeLoadFinished);
    }

    public void HandleScheduleTypeLoadFinished(ScheduleTypesDto scheduleTypesDto) {
        scheduleType.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        string[] scheduleTypes = scheduleTypesDto.GetSchedulesTypes();
        for (int i = 0; i < scheduleTypes.Length; i++) {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = scheduleTypes[i].First().ToString().ToUpper() + scheduleTypes[i].Substring(1);
            options.Add(option);
        }
        scheduleType.AddOptions(options);
    }

    public void HandleSaveSchedule() {
        SaveFirstAndLastName();
        string[] weeklySchedule = GenerateAbbreviatedWeeklySchedule();
        string taName = GenerateTAName();
        string selectedScheduleType = targetScheduleLabel.text;
        ScheduleDto scheduleDto = new ScheduleDto(taName, selectedScheduleType, weeklySchedule);
        serverApi.SendSchedule(scheduleDto);
    }

    private string GenerateTAName() {
        return firstNameField.text.Trim().ToLower() + "_" + lastNameField.text.Trim().ToLower();
    }

    private void SaveFirstAndLastName() {
        PlayerPrefs.SetString("firstName", firstNameField.text);
        PlayerPrefs.SetString("lastName", lastNameField.text);
    }

    private void LoadFirstAndLastName() {
        firstNameField.text = PlayerPrefs.GetString("firstName");
        lastNameField.text = PlayerPrefs.GetString("lastName");
    }
}
