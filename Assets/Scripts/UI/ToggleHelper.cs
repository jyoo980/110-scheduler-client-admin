using UnityEngine;
using UnityEngine.UI;

public class ToggleHelper : MonoBehaviour {

    private Toggle toggle;
    [SerializeField]
    private SelectionHandler selectionHandler;

    private void Awake() {
        toggle = GetComponent<Toggle>();
        //Set default selection behavior to be 'NotAvailable' when program starts
        selectionHandler.ToggleNotAvailable();
    }

    public void HandleToggleAvailable() {
        if (toggle.isOn) {
            selectionHandler.ToggleAvailable();
        }
    }

    public void HandleToggleNotAvailable() {
        if (toggle.isOn) {
            selectionHandler.ToggleNotAvailable();
        }
    }

    public void HandleTogglePreferNot() {
        if (toggle.isOn) {
            selectionHandler.TogglePreferNot();
        }
    }

}
