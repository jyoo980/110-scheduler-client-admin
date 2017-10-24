using UnityEngine;
using UnityEngine.UI;

public class TimeSelection : MonoBehaviour {

    private SelectionHandler selectionHandler;
    private RectTransform rectTransform;
    private Image backgroundImage;
    private SelectionHandler.Selection currentSelection;

	void Start () {
        selectionHandler = GameObject.FindObjectOfType<SelectionHandler>();
        rectTransform = GetComponent<RectTransform>();
        backgroundImage = GetComponent<Image>();
        HandleSelected();
    }

	void Update () {
        CheckIfSelected();
    }

    Rect newRect;
    private void CheckIfSelected() {
        if (selectionHandler.selectionRectangle != Rect.zero) {
            Vector2 centerPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
            centerPoint.y = Screen.height - centerPoint.y;

            newRect = new Rect(centerPoint.x - 30, centerPoint.y, 60, 1);
            if (selectionHandler.selectionRectangle.Overlaps(newRect))
            {
                HandleSelected();
            }
        }
    }

    public void HandleSelected() {
        currentSelection = selectionHandler.GetCurrentSelection();
        backgroundImage.color = selectionHandler.GetColorFromSelection(currentSelection);
    }

    public void HandleClick() {
        //If we already have an availability selected, clicking the item should reset the availability to Not Available
        if (currentSelection == selectionHandler.GetCurrentSelection()) {
            currentSelection = SelectionHandler.Selection.NotAvailable;
            backgroundImage.color = selectionHandler.GetColorFromSelection(currentSelection);
        }
        else {
            HandleSelected();
        }
    }

    public SelectionHandler.Selection GetTimeSlotSelection() {
        return currentSelection;
    }

    public void SetTimeSlotSelection(SelectionHandler.Selection newSelection) {
        currentSelection = newSelection;
        backgroundImage.color = selectionHandler.GetColorFromSelection(currentSelection);
    }
}
