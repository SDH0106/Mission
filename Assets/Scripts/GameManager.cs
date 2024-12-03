using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    System.DateTime _checkTime;
    public static System.DateTime _dailyLogoutTime;
    public static System.DateTime _weeklyLogoutTime;
    public static System.DateTime _monthlyLogoutTime;

    void Start()
    {
        GameData.Instance.currentTime = System.DateTime.Now;
        _checkTime = GameData.Instance.currentTime;

        StartCoroutine(CheckTime());
    }

    private void OnApplicationQuit()
    {
        _dailyLogoutTime = System.DateTime.Now;

        User.Instance.Save();
    }

    IEnumerator CheckTime()
    {
        //_checkTime = GameData.Instance.currentTime;

        yield return new WaitForSeconds(1f);

        //GameData.Instance.currentTime = System.DateTime.Now;

        TempTimeCheck();
    }

    public bool IsSameDayWithCurrent()
    {
        if (_checkTime.Year != _dailyLogoutTime.Year)
            return false;
        if (_checkTime.Month != _dailyLogoutTime.Month)
            return false;
        if (_checkTime.Day != _dailyLogoutTime.Day)
            return false;
        return true;
    }

    public bool IsSameWeekWithCurrent()
    {
        System.TimeSpan resultTime = _checkTime - _weeklyLogoutTime;

        if (resultTime.Days > 7) return false;
        if ((int)_checkTime.DayOfWeek < (int)_weeklyLogoutTime.DayOfWeek) return false;

        return true;
    }
    public bool IsSameMonthWithCurrent()
    {
        if (_checkTime.Year != _monthlyLogoutTime.Year)
            return false;
        if (_checkTime.Month != _monthlyLogoutTime.Month)
            return false;
        return true;
    }

    void TempTimeCheck()
    {
        if (!IsSameMonthWithCurrent())
        {
            GameData.Instance.ClearMission(LifeCycleType.Monthly);
            _monthlyLogoutTime = System.DateTime.Now;
        }
        if (!IsSameWeekWithCurrent())
        {
            GameData.Instance.ClearMission(LifeCycleType.Weekly);
            _weeklyLogoutTime = System.DateTime.Now;
        }
        if (!IsSameDayWithCurrent())
        {
            Debug.Log("??");

            GameData.Instance.ClearMission(LifeCycleType.Daily);
            _dailyLogoutTime = System.DateTime.Now;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset Mission Count"))
        {
            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
            GameData.Instance.ClearAllMissionCount();
        }
        if (GUILayout.Button("RefreshAllRandomMission"))
        {
            GameData.Instance.RefreshAllRandomMission();
            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
            User.Instance.Save();
        }
        if (GUILayout.Button("ResetRefreshCount"))
        {
            User.Instance.ClearUserMissionRefreshCount((int)LifeCycleType.Daily);
            User.Instance.ClearUserMissionRefreshCount((int)LifeCycleType.Weekly);
            User.Instance.ClearUserMissionRefreshCount((int)LifeCycleType.Monthly);
            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Day Up"))
        {
            _checkTime = _checkTime.AddDays(1);

            TempTimeCheck();

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Week Up"))
        {
            _checkTime = _checkTime.AddDays(8);

            TempTimeCheck();

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Month Up"))
        {
            _checkTime = _checkTime.AddMonths(1);

            TempTimeCheck();

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
    }
}
