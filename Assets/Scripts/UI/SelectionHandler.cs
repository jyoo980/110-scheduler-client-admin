using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour {

    private Vector2 boxOriginPosition = Vector2.zero;
    private Vector2 boxEndPosition = Vector2.zero;
    public Rect selectionRectangle;

    [SerializeField]
    private Texture availableTexture;
    [SerializeField]
    private Texture preferNotTexture;
    [SerializeField]
    private Texture notAvailableTexture;
    private Texture currentSelectionTexture;

    [SerializeField]
    private Color availableColor;
    [SerializeField]
    private Color preferNotColor;
    [SerializeField]
    private Color notAvailableColor;

    public enum Selection {
        Available,
        NotAvailable,
        PreferNot
    }

    private Selection currentSelection;

    private Dictionary<SelectionHandler.Selection, Color> colorDictionary = new Dictionary<SelectionHandler.Selection, Color>();

    void Awake() {
        LoadColorDictionary();
    }

    private void LoadColorDictionary() {
        colorDictionary.Add(Selection.Available, availableColor);
        colorDictionary.Add(Selection.NotAvailable, notAvailableColor);
        colorDictionary.Add(Selection.PreferNot, preferNotColor);
    }

    public Color GetColorFromSelection(Selection selection) {
        return colorDictionary[selection];
    }

    public Selection GetCurrentSelection() {
        return currentSelection;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                boxOriginPosition = Input.mousePosition;
            else {
                boxEndPosition = Input.mousePosition;
            }
        }
        else {
            boxEndPosition = boxOriginPosition = Vector2.zero;
        }
    }

    private void OnGUI() {
        DrawRectangle();
    }

    private void DrawRectangle() {
        if (boxOriginPosition != Vector2.zero && boxEndPosition != Vector2.zero) {
            selectionRectangle = new Rect(boxOriginPosition.x, Screen.height - boxOriginPosition.y,
                                boxEndPosition.x - boxOriginPosition.x,
                                -1 * (boxEndPosition.y - boxOriginPosition.y));

            //This is a small tweak needed due to Unity's unfortunate Rect.Contains() method - the Rect constructor
            //allows us to pass in negative widths, but the Contains method does not take this into account. So,
            // we convert negative heights and widths to avoid this.
            if (selectionRectangle.width < 0) {
                selectionRectangle.x += selectionRectangle.width;
                selectionRectangle.width *= -1;
            }

            if (selectionRectangle.height < 0) {
                selectionRectangle.y += selectionRectangle.height;
                selectionRectangle.height *= -1;
            }
            
            GUI.DrawTexture(selectionRectangle, currentSelectionTexture);
        }
        else {
            selectionRectangle = Rect.zero;
        }
    }

    public void ToggleAvailable() {
        currentSelectionTexture = availableTexture;
        currentSelection = Selection.Available;
    }

    public void ToggleNotAvailable() {
        currentSelectionTexture = notAvailableTexture;
        currentSelection = Selection.NotAvailable;
    }

    public void TogglePreferNot() {
        currentSelectionTexture = preferNotTexture;
        currentSelection = Selection.PreferNot;
    }
}
