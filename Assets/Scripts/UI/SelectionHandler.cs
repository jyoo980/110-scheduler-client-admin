using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour {
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

}
