using UnityEngine;
using UnityEngine.UI;
public class SelectedScheduleDisplayHelper : MonoBehaviour {

    public void SetupNameAndType(string firstName, string lastName, string date, string scheduleType) {
        Text[] textFields = GetComponentsInChildren<Text>();
        textFields[0].text = firstName + " " + lastName;
        textFields[1].text = date.Split('.')[0];
        textFields[2].text = scheduleType;
    }

    public void ClearNameAndType() {
        Text[] textFields = GetComponentsInChildren<Text>();
        textFields[0].text = "";
        textFields[1].text = "";
        textFields[2].text = "";
    }
}
