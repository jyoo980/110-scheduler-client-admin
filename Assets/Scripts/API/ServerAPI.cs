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
        uri = uri.Replace(APIConstants.DELETE_SCHEDULE_PARAM_NAME, taName);
        uri = uri.Replace(APIConstants.DELETE_SCHEDULE_PARAM_TYPE, scheduleType);
        StartCoroutine(SendScheduleGetRequest(uri, handleScheduleDeletedFinished));
    }

    public void GetAllSchedules(Action<ScheduleListDto> handleSchedulesLoadFinished){
        string uri = GenerateServerURI() + APIConstants.GET_ALL_SCHEDULES_API;
        StartCoroutine(SendScheduleGetAllRequest(uri, handleSchedulesLoadFinished));
    }

    public void GetScheduleTypes(Action<ScheduleTypesDto> handleScheduleTypeLoadFinished) {
        StartCoroutine(SendScheduleTypeGetRequest(GenerateServerURI() + APIConstants.GET_SCHEDULE_TYPES_API, handleScheduleTypeLoadFinished));
    }

    public void SendSchedule(ScheduleDto newSchedule) {
        StartCoroutine(SendPostRequest(GenerateServerURI() + APIConstants.SEND_SCHEDULE_API,  newSchedule.GenerateRequestBodyBytes()));
    }

    IEnumerator SendPostRequest(string uri, byte[] body) {
        UnityWebRequest request = new UnityWebRequest(uri, APIConstants.POST_METHOD);
        request.downloadHandler = new DownloadHandlerBuffer();

        SetRequestHeaders(request);

        if (body.Length != 0) {
            request.uploadHandler = new UploadHandlerRaw(body);
        }

        SetInfoText(APIConstants.SENDING_SCHED);
        yield return request.SendWebRequest();
        HandleScheduleSendResponse(request);
    }

    private void HandleScheduleSendResponse(UnityWebRequest request)
    {
        if (request.error != null && request.error.ToLower().Contains("cannot resolve")) {
            SetErrorText(APIConstants.GENERAL_CONNECTION_ERROR);
        }
        else {
            SetSuccessText(APIConstants.SUCCESS_SENDING_SCHED);
        }
    }

    IEnumerator SendScheduleGetRequest(string uri, Action handleScheduleDeletedFinished) {
        UnityWebRequest request = new UnityWebRequest(uri, APIConstants.DELETE_METHOD);
        request.downloadHandler = new DownloadHandlerBuffer();

        SetRequestHeaders(request);

        SetInfoText(APIConstants.LOADING_SCHED);
        yield return request.SendWebRequest();
        if (request.error != null) {
            SetErrorText(APIConstants.ERROR_DELETING_SCHEDULE);
        }
        else {
            handleScheduleDeletedFinished();
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
            handleSchedulesLoadFinished(dto);
            retrievalSuccess = true;
        }
        HandleAllScheduleGetResponse(request, retrievalSuccess);
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

    private void HandleScheduleGetResponse(UnityWebRequest request, bool retrievalSuccess) {
        if (request.error != null && request.error.ToLower().Contains("cannot resolve")) {
            SetErrorText(APIConstants.GENERAL_CONNECTION_ERROR);
        }
        else if(!retrievalSuccess) {
            SetErrorText(APIConstants.FAILURE_LOADING_SCHED);
        }
        else {
            SetSuccessText(APIConstants.SUCCESS_LOADING_SCHED);
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
