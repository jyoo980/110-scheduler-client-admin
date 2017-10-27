using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class delete : MonoBehaviour {

    public Text text;
    public void Click() {
        Debug.Log(Application.dataPath);
        text.text = Application.dataPath;
        var file = File.CreateText(Application.dataPath + "/test.txt");

        file.WriteLine("hoogaaaaa");
        file.Close();
            
     }
}
