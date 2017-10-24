using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public static string URL_KEY = "SERVER_URL";

    [SerializeField]
    private InputField inputField;

    void OnEnable() {
        inputField.text = PlayerPrefs.GetString(URL_KEY);
    }

    //Right now we only have a single setting, so this is very simple. Can extend later if needed
    public void HandleSaveAndClose() {
        PlayerPrefs.SetString(URL_KEY, inputField.text);
        gameObject.SetActive(false);
    }
}
