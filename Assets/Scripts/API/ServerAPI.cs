using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class ServerAPI : MonoBehaviour {

    // Credentials
    private string AppKey;
    private string AppSecret;
    private string AppToken;

  
    delegate void HandleResponse(UnityWebRequest request);

    [SerializeField]
    private Text StatusText;

    [SerializeField]
    private Color InfoColor;

    [SerializeField]
    private Color ErrorColor;

    [SerializeField]
    private Color SuccessColor;

   public void DeleteSchedule(string taName, string scheduleType, Action handleScheduleDeletedFinished) {
        string uri = GenerateServerURI() + APIConstants.DELETE_SCHEDULE_API;
        uri = uri.Replace(APIConstants.SCHEDULE_PARAM_NAME, taName);
        uri = uri.Replace(APIConstants.SCHEDULE_PARAM_TYPE, scheduleType);
		StartCoroutine(SendScheduleDeleteRequest(uri, handleScheduleDeletedFinished, APIConstants.DELETING_SCHEDULE, 
			APIConstants.ERROR_DELETING_SCHEDULE));
    }

	public void DeleteScheduleType(string scheduleType, Action handleScheduleTypeDeletedFinished) {
		string uri = GenerateServerURI() + APIConstants.DELETE_SCHEDULE_TYPE_API;
		uri = uri.Replace(APIConstants.SCHEDULE_PARAM_TYPE, scheduleType);
		StartCoroutine(SendScheduleDeleteRequest(uri, handleScheduleTypeDeletedFinished, APIConstants.DELETING_SCHEDULE_TYPE, 
			APIConstants.ERROR_DELETING_SCHEDULE_TYPE));
	}

	public void AddScheduleType(string scheduleType, Action handleScheduleTypeAdded) {
		string uri = GenerateServerURI() + APIConstants.ADD_SCHEDULE_TYPE_API;
		uri = uri.Replace(APIConstants.SCHEDULE_PARAM_TYPE, scheduleType);
		StartCoroutine(SendAddScheduleTypeRequest(uri, handleScheduleTypeAdded, APIConstants.ADDING_SCHEDULE, 
			APIConstants.ERROR_ADDING_SCHEDULE));
	}

    public void GetAllSchedules(Action<ScheduleListDto> handleSchedulesLoadFinished){
        string uri = GenerateServerURI() + APIConstants.GET_ALL_SCHEDULES_API;
        StartCoroutine(SendScheduleGetAllRequest(uri, handleSchedulesLoadFinished));
    }
		
	public void GetAllScheduleTypes(Action<ScheduleTypesDto> handleSchedulesTypesLoadFinished){
		string uri = GenerateServerURI() + APIConstants.GET_SCHEDULE_TYPES_API;
		StartCoroutine(SendScheduleGetAllTypesRequest(uri, handleSchedulesTypesLoadFinished));
	}

	IEnumerator SendScheduleDeleteRequest(string uri, Action handleScheduleDeletedFinished, string loadingText, string errorText) {
        UnityWebRequest request = new UnityWebRequest(uri, APIConstants.DELETE_METHOD);
        request.downloadHandler = new DownloadHandlerBuffer();

        SetRequestHeaders(request);

		SetInfoText(loadingText);
        yield return request.SendWebRequest();
        if (request.error != null) {
			SetErrorText(errorText);
        }
        else {
            handleScheduleDeletedFinished();
        }
    }

	IEnumerator SendAddScheduleTypeRequest(string uri, Action handleScheduleAddFinished, string loadingText, string errorText) {
		UnityWebRequest request = new UnityWebRequest(uri, APIConstants.POST_METHOD);
		request.downloadHandler = new DownloadHandlerBuffer();

		SetRequestHeaders(request);

		SetInfoText(loadingText);
		yield return request.SendWebRequest();
		if (request.error != null) {
			SetErrorText(errorText);
		}
		else {
			handleScheduleAddFinished();
		}
	}

    IEnumerator SendScheduleGetAllRequest(string uri, Action<ScheduleListDto> handleSchedulesLoadFinished) {
        UnityWebRequest request = new UnityWebRequest(uri, APIConstants.GET_METHOD);
        request.downloadHandler = new DownloadHandlerBuffer();

        SetRequestHeaders(request);

        SetInfoText(APIConstants.LOADING_SCHED);
        yield return request.SendWebRequest();
        byte[] result = request.downloadHandler.data;
        ScheduleListDto dto = JsonConvert.DeserializeObject<ScheduleListDto>(Encoding.UTF8.GetString(result));
        bool retrievalSuccess = false;
        
        if (dto != null && dto.GetSchedules().Length > 0) {
            retrievalSuccess = true;
        }
        
        handleSchedulesLoadFinished(dto);
        HandleAllScheduleGetResponse(request, retrievalSuccess);
    }

	IEnumerator SendScheduleGetAllTypesRequest(string uri, Action<ScheduleTypesDto> handleScheduleTypesLoadFinished) {
		UnityWebRequest request = new UnityWebRequest(uri, APIConstants.GET_METHOD);
		request.downloadHandler = new DownloadHandlerBuffer();

		SetRequestHeaders(request);

		SetInfoText(APIConstants.LOADING_SCHED_TYPE);
		yield return request.SendWebRequest();
		byte[] result = request.downloadHandler.data;
		ScheduleTypesDto dto = JsonConvert.DeserializeObject<ScheduleTypesDto>(Encoding.UTF8.GetString(result));
		bool retrievalSuccess = false;

		if (dto != null && dto.GetSchedulesTypes().Length > 0) {
			retrievalSuccess = true;
		}
        handleScheduleTypesLoadFinished(dto);
        HandleAllScheduleTypeGetResponse(request, retrievalSuccess);
	}

    private void HandleAllScheduleGetResponse(UnityWebRequest request, bool retrievalSuccess)
    {
        if (request.error != null && request.error.ToLower().Contains("cannot resolve"))
        {
            SetErrorText(APIConstants.GENERAL_CONNECTION_ERROR);
        }
        else if (!retrievalSuccess)
        {
            SetErrorText(APIConstants.FAILURE_LOADING_SCHEDS);
        }
        else
        {
            SetSuccessText(APIConstants.SUCCESS_LOADING_SCHEDS);
        }
    }

	private void HandleAllScheduleTypeGetResponse(UnityWebRequest request, bool retrievalSuccess)
	{
		if (request.error != null && request.error.ToLower().Contains("cannot resolve"))
		{
			SetErrorText(APIConstants.GENERAL_CONNECTION_ERROR);
		}
		else if (!retrievalSuccess)
		{
			SetErrorText(APIConstants.FAILURE_LOADING_SCHED_TYPES);
		}
		else
		{
			SetSuccessText(APIConstants.SUCCESS_LOADING_SCHED_TYPES);
		}
	}

    IEnumerator SendScheduleTypeGetRequest(string uri, Action<ScheduleTypesDto> handleScheduleTypeLoadFinished) {
        UnityWebRequest request = new UnityWebRequest(uri, APIConstants.GET_METHOD);
        request.downloadHandler = new DownloadHandlerBuffer();

        SetRequestHeaders(request);

        SetInfoText(APIConstants.LOADING_SCHED_TYPE);
        yield return request.SendWebRequest();
        byte[] result = request.downloadHandler.data;
        bool retrievalSuccess = false;
        ScheduleTypesDto scheduleTypesDto = JsonUtility.FromJson<ScheduleTypesDto>(Encoding.UTF8.GetString(result));
        if(scheduleTypesDto != null) {
            retrievalSuccess = true;
            handleScheduleTypeLoadFinished(scheduleTypesDto);
        }
        HandleScheduleTypeGetResponse(request, retrievalSuccess);
    }

    private void HandleScheduleTypeGetResponse(UnityWebRequest request, bool retrievalSuccess) {
        if (request.error != null && request.error.ToLower().Contains("cannot resolve")) {
            SetErrorText(APIConstants.GENERAL_CONNECTION_ERROR);
        }
        else if (!retrievalSuccess) {
            SetErrorText(APIConstants.FAILURE_LOADING_SCHED_TYPES);
        }
        else {
            SetSuccessText(APIConstants.SUCCESS_LOADING_SCHED_TYPES);
        }
    }

    void TestHandler(UnityWebRequest request) {
        Debug.LogWarning(request.responseCode);
        Debug.LogWarning(request.error);
        Debug.LogWarning(request.downloadHandler.text);
    }

    void SetRequestHeaders(UnityWebRequest request) {
        request.SetRequestHeader(APIConstants.HEADER_CONTENT_TYPE, APIConstants.CONTENT_TYPE_JSON);
    }

    private void SetErrorText(string errorText) {
        StatusText.color = ErrorColor;
        StatusText.text = errorText;
    }

    private void SetSuccessText(string successText) {
        StatusText.color = SuccessColor;
        StatusText.text = successText;
    }

    private void SetInfoText(string infoText) {
        StatusText.color = InfoColor;
        StatusText.text = infoText;
    }

    private string GenerateServerURI() {
        string uri = PlayerPrefs.GetString(Settings.URL_KEY);
        uri = uri.Trim();
        if (!uri.EndsWith("/"))
        {
            uri = uri + "/";
        }
        return uri;
    }
}
