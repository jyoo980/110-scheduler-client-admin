using UnityEngine;
using UnityEngine.UI;

public class TimeSelection : MonoBehaviour {

    private SelectionHandler selectionHandler;
    private Image backgroundImage;
    private SelectionHandler.Selection currentSelection;

	void Awake () {
        selectionHandler = GameObject.FindObjectOfType<SelectionHandler>();
        backgroundImage = GetComponent<Image>();
    }

    public SelectionHandler.Selection GetTimeSlotSelection() {
        return currentSelection;
    }

    public void SetTimeSlotSelection(SelectionHandler.Selection newSelection) {
        currentSelection = newSelection;
        backgroundImage.color = selectionHandler.GetColorFromSelection(currentSelection);
    }
}
