using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.IO;

public class WeekScheduler : MonoBehaviour {

    private const int DAYS_IN_WEEK = 7;
    private const int TIME_SLOTS_PER_DAY = 27;
	private const string IS_EXPERIENCED = "**INSERT_IS_EXPERIENCED_HERE**";
	private const string HOURS_AVAILABLE = "**INSERT_HOURS_AVAILABLE_HERE**";
    private const string AVAILABLE = "Available";
    private const string NOT_AVAILABLE = "Not Available";
    private const string PREFER_NOT = "Prefer Not";
    private const string NEW_LINE = "\n";
	private const string AVAILABLE_ABBREVIATED = "a";
	private const string NOT_AVAILABLE_ABBREVIATED = "n";
	private const string PREFER_NOT_ABBREVIATED = "p";
    public ScheduleListHandler listHandler;

    public GameObject weekPanel;

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

    private Dictionary<string, string> selectionDictionary;

    private void Awake () {
        LoadScheduleDictionary();
    }

    private void OnEnable() {
        ScheduleDto selectedSchedule = listHandler.GetSelectedSchedule();
        DistributeWeeklySchedule(selectedSchedule.GetSchedulesByDay());
    }

    public void HandleClosePanel() {
        weekPanel.SetActive(false);
    }

	private void LoadScheduleDictionary() {
		selectionDictionary = new Dictionary<string, string>();
		selectionDictionary[AVAILABLE_ABBREVIATED] = AVAILABLE;
		selectionDictionary[NOT_AVAILABLE_ABBREVIATED] = NOT_AVAILABLE;
		selectionDictionary[PREFER_NOT_ABBREVIATED] = PREFER_NOT;
	}

	public string GenerateWeeklyScheduleString(ScheduleDto scheduleDto) {
		LoadScheduleDictionary ();
		string[] dailySchedules = scheduleDto.GetSchedulesByDay ();
		string[] nameArray = scheduleDto.GetTaName ().Split ('_');
		string unformattedFirst = nameArray [0].Trim ();
		string unformattedLast = nameArray [1].Trim ();
		string formattedFirst = unformattedFirst.Trim ().Substring (0, 1).ToUpper () +
			(unformattedFirst.Length > 1 ? unformattedFirst.Trim().Substring(1, unformattedFirst.Length - 1) : ""); 
		string formattedLast = unformattedLast.Trim ().Substring (0, 1).ToUpper () +
			(unformattedLast.Length > 1 ? unformattedLast.Trim().Substring(1, unformattedLast.Length - 1) : "");         
		StringBuilder builder = new StringBuilder();
		builder.Append ("(");
		builder.Append(formattedFirst + formattedLast);
		builder.Append (" ");
		builder.Append (IS_EXPERIENCED);
		builder.Append (" ");
		builder.Append (HOURS_AVAILABLE);
		builder.Append (" ");
		builder.Append ("\"");
		builder.Append (NEW_LINE);
		for (int i = 0; i < TIME_SLOTS_PER_DAY; i++) {
			builder.Append(HOURS_IN_DAY[i]);
			for (int j = 0; j < DAYS_IN_WEEK; j++) {
				builder.Append (" ");
				builder.Append (selectionDictionary [dailySchedules[j].Substring(i, 1)]);
			}
			builder.Append (NEW_LINE);
		}
		builder.Append ("\"");
		builder.Append (")");
		return builder.ToString();
	} 

	public string GenerateAllWeeklySchedules(ScheduleDto[] allSchedules){
		StringBuilder builder = new StringBuilder ();
		for (int i = 0; i < allSchedules.Length; i++) {
			builder.Append (GenerateWeeklyScheduleString (allSchedules [i]));
			builder.Append (NEW_LINE);
			builder.Append (NEW_LINE);
		}
		//string path = Application.dataPath + "/ScheduleDump.text";
		string path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.DesktopDirectory) + "/ScheduleDump.text";
		File.WriteAllText (path, builder.ToString());
		return path;
	}

    public string[] GenerateAbbreviatedWeeklySchedule() {
        DayScheduler[] daySchedules = GetComponentsInChildren<DayScheduler>();
        string[] weeklySchedule = new string[7];
        for (int i = 0; i < daySchedules.Length; i++){
            weeklySchedule[i] = daySchedules[i].GenerateDailySelectionByAbbreviation();
        }
        return weeklySchedule;
    }

    public void DistributeWeeklySchedule(string[] abbreviatedSchedules) {
        DayScheduler[] daySchedules = GetComponentsInChildren<DayScheduler>();
        for (int i = 0; i < daySchedules.Length; i++) {
            daySchedules[i].DistributeDailyScheduleFromServer(abbreviatedSchedules[i]);
        }
    }
}
