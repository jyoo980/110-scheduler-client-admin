using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExceptionHandler : MonoBehaviour
{
	public GameObject exceptionCanvas;
	public Text exceptionContent;

	void OnEnable()
	{
		Application.logMessageReceived += LogCallback;
	}

	void LogCallback(string condition, string stackTrace, LogType type)
	{
		if (type.Equals(LogType.Error) || type.Equals(LogType.Exception))
		{
			exceptionCanvas.SetActive(true);
			exceptionContent.text = stackTrace;
		}
	}

	void OnDisable()
	{
		Application.logMessageReceived -= LogCallback;
	}

}
