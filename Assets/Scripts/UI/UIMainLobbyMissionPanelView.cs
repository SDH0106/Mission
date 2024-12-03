using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIMainLobbyMissionPanelView : MonoBehaviour
{
    public enum MissionPanelType { Daily, Weekly, Monthly, Rank, Tutorial, Occupation, Pvp, Raid }
    MissionPanelType _activatedPanel = MissionPanelType.Daily;

    [SerializeField] UIMainLobbyMissionCompletePanel completePanel;

    bool _isComponentsLoaded;

    public UIMainLobbyMissionCompletePanel MissionCompletePanel { get { return completePanel; } }
    public List<IMission> AllRewardMissions;
    public RectTransform ToggleGroupRect;

    ScrollRect _scrollRect;
    //ScrollSnap _scrollSnap;

    RectTransform _dailyContentPanel;
    List<UIMainLobbyGrowthElement> _dailyElementList;
    RectTransform _weeklyContentPanel;
    List<UIMainLobbyGrowthElement> _weeklyElementList;
    RectTransform _monthlyContentPanel;
    List<UIMainLobbyGrowthElement> _monthlyElementList;

    RectTransform _missionRefreshCountPanel;
    TextMeshProUGUI _missionRefreshText;
    TextMeshProUGUI _missionRefreshCount;

    TextMeshProUGUI _goodsText;

    void Awake()
    {
        LoadComponents();
        OnUpdatePanel();
    }

    public void LoadComponents()
    {
        _isComponentsLoaded = true;
        OnLoadComponents();
    }

    public void UpdatePanel()
    {
        Debug.Log("update");

        if (_isComponentsLoaded == false)
        {
            LoadComponents();
        }

        if (gameObject.activeInHierarchy == false)
        {
            return;
        }

        OnUpdatePanel();
    }

    protected void OnLoadComponents()
    {
        _dailyContentPanel = transform.Find("Window/UIContent/Scroll View/Viewport/DailyContentPanel").GetComponent<RectTransform>();
        _dailyElementList = new List<UIMainLobbyGrowthElement>();
        _weeklyContentPanel = transform.Find("Window/UIContent/Scroll View/Viewport/WeeklyContentPanel").GetComponent<RectTransform>();
        _weeklyElementList = new List<UIMainLobbyGrowthElement>();
        _monthlyContentPanel = transform.Find("Window/UIContent/Scroll View/Viewport/MonthlyContentPanel").GetComponent<RectTransform>();
        _monthlyElementList = new List<UIMainLobbyGrowthElement>();
        _scrollRect = transform.Find("Window/UIContent/Scroll View").GetComponent<ScrollRect>();

        _missionRefreshCountPanel = transform.Find("MissionRefreshPanel").GetComponent<RectTransform>();
        _missionRefreshText = transform.Find("MissionRefreshPanel/RefreshTextPanel/RefreshText").GetComponent<TextMeshProUGUI>();
        _missionRefreshCount = transform.Find("MissionRefreshPanel/RefreshCount/CountText").GetComponent<TextMeshProUGUI>();

        _goodsText = transform.Find("Goods").GetComponent<TextMeshProUGUI>();
    }

    protected void OnUpdatePanel()
    {
        // 보스미션의 경우 리스트의 변동이 있다.
        ElementListClear(_dailyElementList);
        ElementListClear(_weeklyElementList);
        ElementListClear(_monthlyElementList);

        // 미션 초기화
        Dictionary<ulong, IMission> missions = GameData.Instance.Mission;
        var missionEnum = missions.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value is DailyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_dailyContentPanel);

                UpdateDailyElement(element, (DailyMission)missionEnum.Current.Value);
                _dailyElementList.Add(element);
            }
            else if (missionEnum.Current.Value is WeeklyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_weeklyContentPanel);

                UpdateWeeklyElement(element, (WeeklyMission)missionEnum.Current.Value);
                _weeklyElementList.Add(element);
            }
            else if (missionEnum.Current.Value is MonthlyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_monthlyContentPanel);

                UpdateMonthlyElement(element, (MonthlyMission)missionEnum.Current.Value);
                _monthlyElementList.Add(element);
            }
        }

        _goodsText.text = $"Relec: {User.Instance.Relic}\nGem: {User.Instance.Gem}\nCoin: {User.Instance.Coin}";

        UpdateRefreshCount(_activatedPanel);
        SortMissionListAll();
    }

    public void TempMissionToggleChange()
    {
        Toggle[] toggles = transform.Find("Window/ToggleGroupGrowthTab").GetComponentsInChildren<Toggle>();

        Toggle toggle = null;

        foreach (Toggle t in toggles) 
        {
            if (t.isOn)
            {
                toggle = t;
            }
        }

        if (toggle.isOn)
        {
            switch (toggle.gameObject.name)
            {
                case "ToggleDaily":
                    ChangeMissionPanel(MissionPanelType.Daily);
                    break;
                case "ToggleWeekly":
                    ChangeMissionPanel(MissionPanelType.Weekly);
                    break;
                case "ToggleMonthly":
                    ChangeMissionPanel(MissionPanelType.Monthly);
                    break;
            }
        }
    }

    void OnEnable()
    {
        //app.model.ExitFunctionStack.Push(() => { Notify(MainLobbyEventKey.MissionExitClick); });
        Initialize();
    }

    /*private void OnDisable()
    {
        app.model.ExitFunctionStack.Pop();
    }*/

    void InitString()
    {
        transform.Find("Window/ToggleGroupGrowthTab/ToggleDaily/Text").GetComponent<TextMeshProUGUI>().text = "일일";
        transform.Find("Window/ToggleGroupGrowthTab/ToggleWeekly/Text").GetComponent<TextMeshProUGUI>().text = "주간";
        transform.Find("Window/ToggleGroupGrowthTab/ToggleMonthly/Text").GetComponent<TextMeshProUGUI>().text = "월간";
    }

    void Initialize()
    {
        // 보스미션의 경우 리스트의 변동이 있다.
        ElementListClear(_dailyElementList);
        ElementListClear(_weeklyElementList);
        ElementListClear(_monthlyElementList);

        // 미션 초기화
        Dictionary<ulong, IMission> missions = GameData.Instance.Mission;
        var missionEnum = missions.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value is DailyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_dailyContentPanel);

                UpdateDailyElement(element, (DailyMission)missionEnum.Current.Value);
                _dailyElementList.Add(element);
            }
            else if (missionEnum.Current.Value is WeeklyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_weeklyContentPanel);

                UpdateWeeklyElement(element, (WeeklyMission)missionEnum.Current.Value);
                _weeklyElementList.Add(element);
            }
            else if (missionEnum.Current.Value is MonthlyMission)
            {
                UIMainLobbyGrowthElement element = SpawnMissionElement(_monthlyContentPanel);

                UpdateMonthlyElement(element, (MonthlyMission)missionEnum.Current.Value);
                _monthlyElementList.Add(element);
            }
        }

        UpdateRefreshCount(_activatedPanel);
        SortMissionListAll();

        AllRewardMissions = new List<IMission>();
        for (int i = 0; i < _dailyElementList.Count; i++)
        {
            IMission mission = GameData.Instance.GetMission(_dailyElementList[i].ElementId);

            if (mission.IsComplete && !mission.IsReward)
                AllRewardMissions.Add(mission);
        }

        InitString();
    }

    UIMainLobbyGrowthElement SpawnMissionElement(Transform parent)
    {
        GameObject elementPrefab = Resources.Load<GameObject>("UIMainLobbyMissionElement");
        GameObject elementObj = Instantiate(elementPrefab);
        UIMainLobbyGrowthElement element = elementObj.GetComponent<UIMainLobbyGrowthElement>();
        element.transform.SetParent(parent);
        element.transform.localPosition = Vector3.zero;
        element.transform.localEulerAngles = Vector3.zero;
        element.transform.localScale = Vector3.one;

        return element;
    }

    public void ChangeMissionPanel(MissionPanelType panelType)
    {
        IMission mission = null;
        //_rewardButtonImage.color = _rewardButtonColor[0];
        AllRewardMissions = new List<IMission>();

        switch (panelType)
        {
            case MissionPanelType.Daily:
                _activatedPanel = MissionPanelType.Daily;
                _scrollRect.content = _dailyContentPanel;
                for (int i = 0; i < _dailyElementList.Count; i++)
                {
                    mission = GameData.Instance.GetMission(_dailyElementList[i].ElementId);

                    if (mission.IsComplete && !mission.IsReward)
                        AllRewardMissions.Add(mission);
                }
                break;
            case MissionPanelType.Weekly:
                _activatedPanel = MissionPanelType.Weekly;
                _scrollRect.content = _weeklyContentPanel;
                for (int i = 0; i < _weeklyElementList.Count; i++)
                {
                    mission = GameData.Instance.GetMission(_weeklyElementList[i].ElementId);

                    if (mission.IsComplete && !mission.IsReward)
                        AllRewardMissions.Add(mission);
                }
                break;
            case MissionPanelType.Monthly:
                _activatedPanel = MissionPanelType.Monthly;
                _scrollRect.content = _monthlyContentPanel;
                for (int i = 0; i < _monthlyElementList.Count; i++)
                {
                    mission = GameData.Instance.GetMission(_monthlyElementList[i].ElementId);

                    if (mission.IsComplete && !mission.IsReward)
                        AllRewardMissions.Add(mission);
                }
                break;
            default:
                break;
        }

        UpdateRefreshCount(_activatedPanel);
        UpdatePanelVisible();
    }

    void SortMissionListAll()
    {
        SortMissionList(_dailyElementList);
        SortMissionList(_weeklyElementList);
        SortMissionList(_monthlyElementList);
    }

    void SortMissionList(List<UIMainLobbyGrowthElement> list)
    {
        list.Sort((a, b) => a.ElementId.CompareTo(b.ElementId));
        list.Sort((a, b) =>
        {
            var aMission = GameData.Instance.GetMission(a.ElementId);
            var bMission = GameData.Instance.GetMission(b.ElementId);

            int compare = aMission.IsReward.CompareTo(bMission.IsReward);
            if (compare != 0)
                return compare;

            return bMission.IsComplete.CompareTo(aMission.IsComplete);
        });

        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetSiblingIndex(i);
        }
    }

    void UpdateDailyElement(UIMainLobbyGrowthElement element, DailyMission mission)
    {
        bool isEnabled = !mission.IsReward;
        bool[] enabledSlots = new bool[2];
        bool isCompleted= mission.IsComplete;

        UIMainLobbyGrowthElement.EventType rewardEvent;
        switch (mission.RewardType)
        {
            case RewardType.Gem:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyGem;
                break;
            case RewardType.Coin:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyCoin;
                break;
            case RewardType.Relic:
            default:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyRelic;
                break;
        }

        if (isEnabled)
        {
            enabledSlots[0] = true;
            enabledSlots[1] = true;
        }


        element.UpdateElement(UIMainLobbyGrowthElement.ElementType.Mission,
                              mission.Id, isEnabled, mission.Name + (isCompleted ? "(완료)" : ""), mission.Explain,
                           Resources.Load<Sprite>("UI/Icon/" + mission.IconId),
                           new UIMainLobbyGrowthElement.EventType[] { isCompleted ? ( isEnabled ? UIMainLobbyGrowthElement.EventType.Reward : UIMainLobbyGrowthElement.EventType.Null): UIMainLobbyGrowthElement.EventType.Null, isEnabled ? rewardEvent : UIMainLobbyGrowthElement.EventType.Null },
                           new double[] { 0, mission.GetReward() },
                           new UIMainLobbyGrowthElement.RequireType[] { UIMainLobbyGrowthElement.RequireType.Null, UIMainLobbyGrowthElement.RequireType.Null },
                           enabledSlots);

    }

    void UpdateWeeklyElement(UIMainLobbyGrowthElement element, WeeklyMission mission)
    {
        bool isEnabled = !mission.IsReward;
        bool[] enabledSlots = new bool[2];
        bool isCompleted = mission.IsComplete;

        UIMainLobbyGrowthElement.EventType rewardEvent;
        switch (mission.RewardType)
        {
            case RewardType.Gem:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyGem;
                break;
            case RewardType.Coin:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyCoin;
                break;
            default:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyRelic;
                break;
        }

        if (isEnabled)
        {
            enabledSlots[0] = true;
            enabledSlots[1] = true;
        }

        element.UpdateElement(UIMainLobbyGrowthElement.ElementType.Mission,
                              mission.Id, isEnabled, mission.Name + (isCompleted ? "(완료)" : ""), mission.Explain,
                           Resources.Load<Sprite>("UI/Icon/" + mission.IconId),
                           new UIMainLobbyGrowthElement.EventType[] { isCompleted ? (isEnabled ? UIMainLobbyGrowthElement.EventType.Reward : UIMainLobbyGrowthElement.EventType.Null) : ( UIMainLobbyGrowthElement.EventType.Null ), isEnabled ? rewardEvent : UIMainLobbyGrowthElement.EventType.Null },
                           new double[] { 0, mission.GetReward() },
                           new UIMainLobbyGrowthElement.RequireType[] { UIMainLobbyGrowthElement.RequireType.Null, UIMainLobbyGrowthElement.RequireType.Null },
                           enabledSlots);
    }

    void UpdateMonthlyElement(UIMainLobbyGrowthElement element, MonthlyMission mission)
    {
        bool isEnabled = !mission.IsReward;
        bool[] enabledSlots = new bool[2];
        bool isCompleted = mission.IsComplete;

        UIMainLobbyGrowthElement.EventType rewardEvent;
        switch (mission.RewardType)
        {
            case RewardType.Gem:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyGem;
                break;
            case RewardType.Coin:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyCoin;
                break;
            default:
                rewardEvent = UIMainLobbyGrowthElement.EventType.TrophyRelic;
                break;
        }

        if (isEnabled)
        {
            enabledSlots[0] = true;
            enabledSlots[1] = true;
        }

        element.UpdateElement(UIMainLobbyGrowthElement.ElementType.Mission,
                              mission.Id, isEnabled, mission.Name + (isCompleted ? "(완료)" : ""), mission.Explain,
                           Resources.Load<Sprite>("UI/Icon/" + mission.IconId),
                           new UIMainLobbyGrowthElement.EventType[] { isCompleted ? (isEnabled ? UIMainLobbyGrowthElement.EventType.Reward : UIMainLobbyGrowthElement.EventType.Null) : UIMainLobbyGrowthElement.EventType.Null, isEnabled ? rewardEvent : UIMainLobbyGrowthElement.EventType.Null },
                           new double[] { 0, mission.GetReward() },
                           new UIMainLobbyGrowthElement.RequireType[] { UIMainLobbyGrowthElement.RequireType.Null, UIMainLobbyGrowthElement.RequireType.Null },
                           enabledSlots);
    }

    void UpdateRefreshCount(MissionPanelType panelType)
    {
        int lifeCycleType = 0;
        string lifeCycleText = "";

        switch (panelType)
        {
            case MissionPanelType.Daily:
                lifeCycleText = "일일";
                lifeCycleType = (int)LifeCycleType.Daily;
                _missionRefreshCountPanel.gameObject.SetActive(true);
                break;

            case MissionPanelType.Weekly:
                lifeCycleType = (int)LifeCycleType.Weekly;
                lifeCycleText = "주간";
                _missionRefreshCountPanel.gameObject.SetActive(true);
                break;

            case MissionPanelType.Monthly:
                lifeCycleType = (int)LifeCycleType.Monthly;
                lifeCycleText = "월간";
                _missionRefreshCountPanel.gameObject.SetActive(true);
                break;

            default:
                _missionRefreshCountPanel.gameObject.SetActive(false);
                break;
        }

        _missionRefreshText.text = $"{lifeCycleText} {"미션 변경 횟수"}";
        int restFreeCount = (GameData.FreeMissionRefreshCounts[lifeCycleType] - User.Instance.UsedFreeRefreshCount[lifeCycleType]);
        int restPaidCount = (GameData.PaidMissionRefreshCounts[lifeCycleType] - User.Instance.UsedPaidRefreshCount[lifeCycleType]);
        _missionRefreshCount.text = (restFreeCount + restPaidCount).ToString();
    }

    void UpdatePanelVisible()
    {
        _dailyContentPanel.gameObject.SetActive(_activatedPanel == MissionPanelType.Daily);
        _weeklyContentPanel.gameObject.SetActive(_activatedPanel == MissionPanelType.Weekly);
        _monthlyContentPanel.gameObject.SetActive(_activatedPanel == MissionPanelType.Monthly);
    }

    public void ElementListClear(List<UIMainLobbyGrowthElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i].gameObject);
        }
        list.Clear();
    }

    public void OnRewardAllComplete()
    {
        ChangeMissionPanel(_activatedPanel);
    }
}
