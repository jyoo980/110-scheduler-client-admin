using UnityEngine;

public class APIConstants {

    public static string NO_URL_WARNING = "No server URL detected! Please click 'Settings' and add URL provided by course coordinator.";
    public static string DELETE_SCHEDULE_PARAM_NAME = "{taName}";
    public static string DELETE_SCHEDULE_PARAM_TYPE = "{scheduleType}";

    //Request URIs
    public static string DELETE_SCHEDULE_API = "schedule/deleteSchedule/" + DELETE_SCHEDULE_PARAM_NAME + "/" + DELETE_SCHEDULE_PARAM_TYPE;
    public static string GET_ALL_SCHEDULES_API = "schedule/getAllSchedules";

    public static string GET_SCHEDULE_TYPES_API = "schedule/getScheduleTypes";
    public static string SEND_SCHEDULE_API = "schedule/setSchedule";
    
    //HTTP methods
    public static string POST_METHOD = "POST";
    public static string GET_METHOD = "GET";
    public static string DELETE_METHOD = "DELETE";

    //Request Headers
    public static string HEADER_CONTENT_TYPE = "Content-Type";
    public static string CONTENT_TYPE_JSON = "application/json";

    //Loading text status updates
    public static string ERROR_DELETING_SCHEDULE = "Failed to delete selected schedule";
    public static string LOADING_SCHED_TYPE = "Loading schedule types... please be patient" +
		" - if you are the first person using this in a while, the server needs to warm up!";
	public static string LOADING_SCHED = "Loading schedule... please be patient" +
		" - if you are the first person using this in a while, the server needs to warm up!";
    public static string SENDING_SCHED = "Sending schedule...";
    public static string GENERAL_CONNECTION_ERROR = "Connection error - please confirm server URL is correct in settings";
    public static string SUCCESS_LOADING_SCHED = "Successfully retrieved schedule";
    public static string SUCCESS_LOADING_SCHED_TYPES = "Successfully retrieved schedule types";
    public static string FAILURE_LOADING_SCHED = "Failed to retrieve schedule - doesn't exist on server" +
        " Please confirm name and schedule type is correct, or submit first if this is your first time";
    public static string FAILURE_LOADING_SCHED_TYPES = "Failed to retrieve schedule types - none exist. Contact course coordinator";
    public static string SUCCESS_SENDING_SCHED = "Successfully sent schedule";
    public static string FAILURE_SENDING_SCHED = "Failed to send schedule";

    public static string SUCCESS_LOADING_SCHEDS = "Successfully retrieved list of schedules";
    public static string FAILURE_LOADING_SCHEDS = "Failed to retrieve schedule list - nothing there!";

    public static string DEFAULT_SCHEDULE_TYPE = "Regular";
}
