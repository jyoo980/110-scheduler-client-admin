using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DayScheduler : MonoBehaviour {

    private Dictionary<SelectionHandler.Selection, string> selectionToAbbreviations;
    private Dictionary<string, SelectionHandler.Selection> abbreviationToSelection;
    private static string availableAbbreviation = "a";
    private static string notAvailableAbbreviation = "n";
    private static string preferNotAbbreviation = "p";

    private void Awake() {
        LoadAbbreviationDictionaries();
    }

    private void LoadAbbreviationDictionaries() {
        selectionToAbbreviations = new Dictionary<SelectionHandler.Selection, string>();
        selectionToAbbreviations[SelectionHandler.Selection.Available] = availableAbbreviation;
        selectionToAbbreviations[SelectionHandler.Selection.NotAvailable] = notAvailableAbbreviation;
        selectionToAbbreviations[SelectionHandler.Selection.PreferNot] = preferNotAbbreviation;

        abbreviationToSelection = new Dictionary<string, SelectionHandler.Selection>();
        abbreviationToSelection[availableAbbreviation] = SelectionHandler.Selection.Available;
        abbreviationToSelection[notAvailableAbbreviation] = SelectionHandler.Selection.NotAvailable;
        abbreviationToSelection[preferNotAbbreviation] = SelectionHandler.Selection.PreferNot;
    }

    public SelectionHandler.Selection[] GenerateDailySelections() {
        TimeSelection[] timeSelections = GetComponentsInChildren<TimeSelection>();
        SelectionHandler.Selection[] daySelections = new SelectionHandler.Selection[timeSelections.Length];
        for (int i = 0; i < daySelections.Length; i++) {
            daySelections[i] = timeSelections[i].GetTimeSlotSelection();
        }
        return daySelections;
    }

    public string GenerateDailySelectionByAbbreviation() {
        TimeSelection[] timeSelections = GetComponentsInChildren<TimeSelection>();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < timeSelections.Length; i++)
        {
            sb.Append(GetAbbreviatedTimeSlotSelection(timeSelections[i].GetTimeSlotSelection()));
        }
        return sb.ToString();
    }

    public void DistributeDailyScheduleFromServer(string dailySchedule) {
        TimeSelection[] timeSelections = GetComponentsInChildren<TimeSelection>();
        char[] scheduleByHalfHour = dailySchedule.ToCharArray();
        for (int i = 0; i < scheduleByHalfHour.Length; i++) {
            timeSelections[i].SetTimeSlotSelection(abbreviationToSelection[scheduleByHalfHour[i].ToString()]);
        }
    }

    public string GetAbbreviatedTimeSlotSelection(SelectionHandler.Selection selection)
    {
        return selectionToAbbreviations[selection];
    }
}
