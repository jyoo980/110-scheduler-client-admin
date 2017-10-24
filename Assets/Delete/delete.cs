using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class delete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
    public void addcontent() {
        RectTransform trans = GetComponent<RectTransform>();
        trans.localPosition = new Vector2(trans.localPosition.x, trans.localPosition.y+5);
        //trans.sizeDelta = new Vector2(trans.sizeDelta.x, trans.sizeDelta.y + 5);

    }
}
