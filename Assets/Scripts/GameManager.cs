using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    System.DateTime _checkTime;
    public static System.DateTime _dailyLogoutTime;
    public static System.DateTime _weeklyLogoutTime;
    public static System.DateTime _monthlyLogoutTime;

    public static int currentChapter = 1;
    string chapterInput = "";
    string petInput = "";
    string bodyInput = "";

    private void Awake()
    {
        GameData.Instance.currentTime = System.DateTime.Now;
        _checkTime = GameData.Instance.currentTime;

        GameData.Instance.GenerateAllRandomMissions();
    }

    void Start()
    {
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
            GameData.Instance.ResetMissions(LifeCycleType.Monthly);
            _monthlyLogoutTime = System.DateTime.Now;
        }
        if (!IsSameWeekWithCurrent())
        {
            GameData.Instance.ResetMissions(LifeCycleType.Weekly);
            _weeklyLogoutTime = System.DateTime.Now;
        }
        if (!IsSameDayWithCurrent())
        {
            GameData.Instance.ResetMissions(LifeCycleType.Daily);
            _dailyLogoutTime = System.DateTime.Now;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset Mission Complete Count"))
        {
            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
            foreach(var mission in GameData.Instance.Missions) 
                User.Instance.SetMissionCount(mission.Key, 0);
        }
        if (GUILayout.Button("RefreshAllRandomMission"))
        {
            GameData.Instance.RefreshAllRandomMissions();
            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
            User.Instance.Save();
        }
        if (GUILayout.Button("ResetRefreshCount"))
        {
            User.Instance.SetMissionRefreshCount(0, 0);
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
        if (GUILayout.Button("Warp Mission Count Up"))
        {
            GameData.Instance.missionReporter.ReportWarpClear(currentChapter);

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Random MonsterKill Mission Count Up"))
        {
            ulong monsterId = GameData.Instance.Monsters.ElementAt(Random.Range(0, GameData.Instance.Monsters.Count)).Key;
            Debug.Log($"처치한 몬스터 이름: {GameData.Instance.Monsters[monsterId]}");
            GameData.Instance.missionReporter.ReportMonsterKill(monsterId, currentChapter, User.Instance._userBody, 1);

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Random Stat Upgrade Mission Count Up"))
        {
            GameData.Instance.missionReporter.ReportStatUpgrade(GameData.Instance.Stats.ElementAt(Random.Range(0, GameData.Instance.Stats.Count)).Key, 1);

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        if (GUILayout.Button("Game Clear Mission Count Up"))
        {
            GameData.Instance.missionReporter.ReportGameClear(currentChapter, User.Instance._userPet, User.Instance._userBody, 1);

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }


        GUI.Label(new Rect(310, 10, 100, 25), "챕터 번호:");
        chapterInput = GUI.TextField(new Rect(410, 10, 100, 25), chapterInput, 10);

        if (GUI.Button(new Rect(510, 10, 100, 25), "챕터 설정"))
        {
            if (int.TryParse(chapterInput, out int chapter))
                currentChapter = chapter;
            else
                Debug.LogWarning("올바른 숫자를 입력하세요.");

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        GUI.Label(new Rect(310, 35, 100, 25), "장착 펫 교체:");
        petInput = GUI.TextField(new Rect(410, 35, 100, 25), petInput, 1);

        if (GUI.Button(new Rect(510, 35, 100, 25), "펫 설정"))
        {
            if (ulong.TryParse(petInput, out ulong petId) && GameData.Instance.Pets.ContainsKey(petId))
                User.Instance._userPet = petId;
            else
                Debug.LogWarning("0 ~ 4 사이의 값을 입력해주세요.");

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
        GUI.Label(new Rect(310, 60, 100, 25), "캐릭터 교체:");
        bodyInput = GUI.TextField(new Rect(410, 60, 100, 25), bodyInput, 1);

        if (GUI.Button(new Rect(510, 60, 100, 25), "캐릭터 설정"))
        {
            if (int.TryParse(bodyInput, out int bodyId) && GameData.Instance.Bodies.ContainsKey(bodyId))
                User.Instance._userBody = bodyId;
            else
                Debug.LogWarning("0 ~ 3 사이의 값을 입력해주세요.");

            GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
        }
    }
}