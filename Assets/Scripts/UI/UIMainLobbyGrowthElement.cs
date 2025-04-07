using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIMainLobbyGrowthElement : MonoBehaviour
{
    static readonly Color SlotDisabledColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    static readonly Color DisabledColor = new Color(0, 0, 0, 0.5f);

    public enum ElementType { Quest, Weapon, Pet, Stat, TimeSlip, Mission }
    public ElementType Type { get { return _elementType; } }

    [SerializeField]
    ElementType _elementType = ElementType.Quest;

    public enum RequireType { Money, Gem, Exp, Relic, Null };
    public enum EventType { LevelUp, Buy, Link, Reward, Complete, TrophyRelic, TrophyGem, TrophyCoin, Null }

    static Dictionary<RequireType, string> requirementSlotIconsByRequireType = new Dictionary<RequireType, string>()
    {
        {RequireType.Money, "icon_money" },
        {RequireType.Gem, "icon_gem" },
        {RequireType.Exp, "icon_exp" },
        {RequireType.Relic, "icon_relic" },
    };
    static Dictionary<EventType, string> requirementSlotIconsByEventType = new Dictionary<EventType, string>()
    {
        {EventType.Link, "icon_link" },
        {EventType.Reward, "icon_money" },
        {EventType.Complete, "icon_cat" },
        {EventType.TrophyRelic, "icon_relic" },
        {EventType.TrophyGem, "icon_gem" },
        {EventType.TrophyCoin, "1600000005" },
    };
    static Dictionary<EventType, string> requirementSlotDescriptionTexts = new Dictionary<EventType, string>()
    {
        {EventType.LevelUp, "0101000009" },
        {EventType.Buy, "0101000010" },
        {EventType.Link, "0105000126" },
        {EventType.Reward, "0105000127" },
        {EventType.Complete, "0105000128" },
        {EventType.Null, null }
    };

    struct RequirementSlot
    {
        public Transform slotTransform;
        public RequireType requireType;
        public EventType eventType;
        public TextMeshProUGUI descriptionUI;
        public TextMeshProUGUI valueUI;
        public Image iconUI;
        public Image slotFlashUI;
        public Transform slotMaxTransform;

        public bool isEnabled;

        public RequirementSlot(Transform slotTransform)
        {
            this.slotTransform = slotTransform;
            requireType = RequireType.Null;
            eventType = EventType.Null;
            descriptionUI = slotTransform.Find("TextDescription").GetComponent<TextMeshProUGUI>();
            valueUI = slotTransform.Find("TextValue").GetComponent<TextMeshProUGUI>();
            iconUI = slotTransform.Find("ImageIcon").GetComponent<Image>();
            slotFlashUI = slotTransform.Find("SlotFlash").GetComponent<Image>();
            slotMaxTransform = slotTransform.Find("MaxCover");

            isEnabled = false;
        }
    }
    RequirementSlot[] _requirementSlots;

    struct UpdateElementInfo
    {
        public ElementType elementType;
        public ulong id;
        public bool isEnabled;
        public string title;
        public string content;
        public Sprite icon;
        public EventType[] eventTypes;
        public double[] requireValues;
        public RequireType[] requireTypes;
        public bool[] isEnabledSlots;

        public UpdateElementInfo(ElementType elementType,
        ulong id, bool isEnabled, string title, string content, Sprite icon,
        EventType[] eventTypes, double[] requireValues, RequireType[] requireTypes, bool[] isEnabledSlots)
        {
            this.elementType = elementType;
            this.id = id;
            this.isEnabled = isEnabled;
            this.title = title;
            this.content = content;
            this.icon = icon;
            this.eventTypes = eventTypes;
            this.requireValues = requireValues;
            this.requireTypes = requireTypes;
            this.isEnabledSlots = isEnabledSlots;
        }
    }
    UpdateElementInfo _stagedInfo;

    public ulong ElementId { get; set; }

    float _fQuestProgressFillate = 0;
    float _fQuestProgressRemainTime = 0;

    Image _iconUI;
    public Image IconUI { get { return _iconUI; } }
    TextMeshProUGUI _titleUI;
    TextMeshProUGUI _contentUI;
    Transform _progressBackground;
    Image _progressUI;
    TextMeshProUGUI _progressText;
    Transform _completeSprite;

    Image _elementCoverUI;

    Transform _refreshButton;
    TextMeshProUGUI _paidRefreshCount;
    Transform _paidRefreshUI;

    bool _isCull;

    void Awake()
    {
        _iconUI = transform.Find("ImageIcon").GetComponent<Image>();
        if (_iconUI)
            _iconUI.preserveAspect = true;

        _titleUI = transform.Find("MainTextAndProgress/TextElementTitle").GetComponent<TextMeshProUGUI>();
        _contentUI = transform.Find("MainTextAndProgress/TextElementContent").GetComponent<TextMeshProUGUI>();
        _progressBackground = transform.Find("MainTextAndProgress/ProgressBackground");
        _completeSprite = transform.Find("CompleteSprite");
        _elementCoverUI = transform.Find("ElementCover").GetComponent<Image>();
        _elementCoverUI.onCullStateChanged.AddListener(OnCullStateChanged);

        _refreshButton = transform.Find("RequirementSlots/RefreshButton");
        _paidRefreshCount = transform.Find("RequirementSlots/RefreshButton/PaidRefresh/RefreshCount").GetComponent<TextMeshProUGUI>();
        _paidRefreshUI = transform.Find("RequirementSlots/RefreshButton/PaidRefresh");

        if (_progressBackground != null)
        {
            Transform progressTransform = _progressBackground.Find("Progress");
            if (progressTransform != null)
            {
                _progressUI = progressTransform.GetComponent<Image>();
                _progressUI.fillAmount = 0;
            }
            Transform textTransform = _progressBackground.Find("Count");
            if (textTransform != null)
            {
                _progressText = textTransform.GetComponent<TextMeshProUGUI>();
                _progressText.text = "";
            }
        }

        Transform slotTransform = transform.Find("RequirementSlots");
        if (slotTransform != null)
        {
            int slotCount = 0;
            for (int i = 0; i < slotTransform.childCount; i++)
            {
                if (slotTransform.GetChild(i).name.Contains("Requirement"))
                    slotCount++;
            }
            _requirementSlots = new RequirementSlot[slotCount];


            for (int i = 0; i < slotCount; i++)
            {
                _requirementSlots[i] = new RequirementSlot(transform.Find("RequirementSlots/Requirement" + i));
            }
        }
    }
    
    void OnCullStateChanged(bool cull)
    {
        if (_isCull == true && cull == false)
        {
            UpdateElement(_stagedInfo);
        }
        _isCull = cull;
    }

    public void SetIcon(Sprite iconSprite)
    {
        if (_iconUI != null && iconSprite != null)
            _iconUI.sprite = iconSprite;
    }

    public void SetQuestProgress(float progress)
    {
        if (_progressUI == null)
            return;

        if (_isCull)
            return;

        _fQuestProgressFillate = progress;
        UpdateQuestProgress();
    }

    public void SetQuestRemainTime(float second)
    {
        if (_isCull)
            return;

        _fQuestProgressRemainTime = second;
    }

    void UpdateQuestProgress()
    {
        if (_progressUI == null)
            return;

        if (_isCull)
            return;

        if (_progressUI.gameObject.activeInHierarchy)
            _progressUI.fillAmount = _fQuestProgressFillate;

        if (_progressText.gameObject.activeInHierarchy)
        {
            var time = TimeSpan.FromSeconds(_fQuestProgressRemainTime);
            byte flag = 0;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            flag |= (byte)(time.Hours.Equals(0) ? 0 : 4);
            flag |= (byte)(time.Minutes.Equals(0) ? 0 : 2);
            flag |= (byte)(time.Seconds.Equals(0) ? 0 : 1);
            flag |= (byte)(time.Milliseconds.Equals(0) ? 0 : 1);

            if ((flag & 4) == 4)
                builder.AppendFormat("{0}h ", time.Hours);
            if ((flag & 2) == 2)
                builder.AppendFormat("{0}m ", time.Minutes);
            if ((flag & 1) == 1)
            {
                if (time.Milliseconds.Equals(0) == false)
                    builder.AppendFormat("{0}.{1}s", time.Seconds, time.Milliseconds);
                else
                    builder.AppendFormat("{0}s", time.Seconds);
            }

            _progressText.text = builder.ToString();
        }
    }

    public void SetMissionProcess(bool isEnabled)
    {
        if (_progressUI == null)
            return;

        IMission mission = null;
        mission = GameData.Instance.GetMission(ElementId);
        float completionCount = (float)mission.CompleteCount;
        float count = (float)mission.Count;

        float value = Mathf.InverseLerp(0, completionCount, count);
        _progressUI.fillAmount = value;

        if (_progressText == null)
            return;

        _progressText.text = "( " + count.ToString("0") + " / " + completionCount.ToString("0") + " )";

        if (_completeSprite != null)
        {
            if (isEnabled)
                _completeSprite.gameObject.SetActive(false);
            else
                _completeSprite.gameObject.SetActive(true);
        }

    }

    public void UpdateSlotMax(int slotidx, bool isMax)
    {
        if (_requirementSlots[slotidx].slotMaxTransform == null)
            return;

        _requirementSlots[slotidx].slotMaxTransform.gameObject.SetActive(isMax);
        _requirementSlots[slotidx].valueUI.gameObject.SetActive(!isMax);
        _requirementSlots[slotidx].iconUI.gameObject.SetActive(!isMax);
        _requirementSlots[slotidx].descriptionUI.gameObject.SetActive(!isMax);
    }    

    void UpdateElement(UpdateElementInfo info)
    {
        UpdateElement(info.elementType, info.id, info.isEnabled, info.title, info.content, info.icon
                , info.eventTypes, info.requireValues, info.requireTypes, info.isEnabledSlots);

    }

    public void UpdateElement(ElementType elementType,
        ulong id, bool isEnabled, string title, string content, Sprite icon,
        EventType[] eventTypes, double[] requireValues, RequireType[] requireTypes, bool[] isEnabledSlots)
    {
        _stagedInfo = new UpdateElementInfo(elementType, id, isEnabled, title, content, icon
                , eventTypes, requireValues, requireTypes, isEnabledSlots);

        if (_isCull == true || (gameObject.activeInHierarchy == false && elementType != ElementType.Mission))
        {
            return;
        }

        _elementType = elementType;
        ElementId = id;

        _titleUI.text = title;
        _contentUI.text = content;

        if (User.Instance.GetUserMission(ElementId).isComplete)
            _refreshButton.gameObject.SetActive(false);
        else
        {
            int lifeCycleType = (int)(GameData.Instance.GetMission(ElementId).LifeCycle);

            bool isActive = User.Instance.UsedPaidRefreshCount < GameData.PaidMissionRefreshCounts;
            _refreshButton.gameObject.SetActive(GameData.IsRandomMission(id) && isActive);

            if (_refreshButton.gameObject.activeSelf)
            {
                bool isFree = User.Instance.UsedFreeRefreshCount < GameData.FreeMissionRefreshCounts;

                _paidRefreshUI.gameObject.SetActive(!isFree);

                if(!isFree)
                    _paidRefreshCount.text = GameData.MissionRefreshRequireGems[lifeCycleType].ToString();
            }
        }


        if (_progressBackground != null)
        {
            switch (_elementType)
            {
                case ElementType.Quest:
                    _progressBackground.gameObject.SetActive(true);
                    UpdateQuestProgress();
                    break;
                case ElementType.Mission:
                    _progressBackground.gameObject.SetActive(true);
                    SetMissionProcess(isEnabled);
                    break;
                default:
                    _progressBackground.gameObject.SetActive(false);
                    break;

            }
        }        

        if (_iconUI != null && icon != null)
            _iconUI.sprite = icon;
        _elementCoverUI.color = isEnabled ? Color.clear : DisabledColor;

        if (_requirementSlots != null)
        {
            for (int i = 0; i < _requirementSlots.Length; i++)
            {
                _requirementSlots[i].eventType = eventTypes[i];
                _requirementSlots[i].isEnabled = isEnabledSlots[i];

                bool isSlotMarked = false;

                if (requirementSlotDescriptionTexts.ContainsKey(eventTypes[i]))
                {
                    if (requirementSlotDescriptionTexts[eventTypes[i]] != null)
                        _requirementSlots[i].descriptionUI.text = TempDatas.LocaleTexts[requirementSlotDescriptionTexts[eventTypes[i]]];
                }
                else
                {
                    _requirementSlots[i].descriptionUI.text = "보상 시";
                }

                _requirementSlots[i].requireType = requireTypes[i];

                Color enableColor = isEnabledSlots[i] ? Color.white : SlotDisabledColor;
                Color enableAccentColor = isEnabledSlots[i] ? Color.white : SlotDisabledColor;

                if (elementType == ElementType.Quest || elementType == ElementType.Weapon)
                {
                    enableAccentColor = isEnabled ? new Color(1f, 0.25f, 0.25f) : SlotDisabledColor;
                    if (elementType == ElementType.Quest)
                        _iconUI.color = enableAccentColor;
                }

                if (isEnabledSlots[i] && elementType == ElementType.Mission)
                {
                    enableAccentColor = isSlotMarked ? new Color(1f, 0.25f, 0.25f) : Color.white;
                    enableColor = isSlotMarked ? new Color(1f, 0.25f, 0.25f) : Color.white;
                }

                if (requireTypes[i] == RequireType.Gem)
                    _requirementSlots[i].valueUI.text = requireValues[i].ToString("0");

                else
                    _requirementSlots[i].valueUI.text = requireValues[i].ToString();


                if (requirementSlotIconsByRequireType.ContainsKey(requireTypes[i]))
                {
                    _requirementSlots[i].iconUI.sprite = Resources.Load<Sprite>("UI/Icon/" + requirementSlotIconsByRequireType[requireTypes[i]]);
                }
                else
                {
                    if(requireTypes[i] == RequireType.Null)
                    {
                        if(requirementSlotIconsByEventType.ContainsKey(eventTypes[i]))
                            _requirementSlots[i].iconUI.sprite = Resources.Load<Sprite>("UI/Icon/" + requirementSlotIconsByEventType[eventTypes[i]]);

                        if (eventTypes[i] == EventType.Null)
                        {
                            _requirementSlots[i].iconUI.sprite = null;
                            enableColor = new Color(0f, 0f, 0f, 0f);
                            isEnabledSlots[i] = false;
                        }

                        if (requireValues[i] == 0)
                            _requirementSlots[i].valueUI.text = null;
                    }
                }

                _requirementSlots[i].iconUI.color = _requirementSlots[i].iconUI.sprite != null ? enableAccentColor : Color.clear;
                _requirementSlots[i].descriptionUI.color = enableColor;
                _requirementSlots[i].valueUI.color = enableColor;      
            }
        }        
    }

    /* public void ClickRefreshButton()
     {
         UserMission mission = User.Instance.GetUserMission(ElementId);
         int lifeCycleType = (int)GameData.Instance.GetMission(ElementId).LifeCycleType;

         if (!GameData.Instance.RandomMissions.ContainsKey(ElementId) || mission.isComplete)
             return;

         if (User.Instance.UsedFreeRefreshCount[lifeCycleType] < GameData.FreeMissionRefreshCounts[lifeCycleType])
         {
             string lifeCycleText = "";

             switch (lifeCycleType)
             {
                 case (int)LifeCycleType.Daily:
                     lifeCycleText = "0101000013";
                     break;

                 case (int)LifeCycleType.Weekly:
                     lifeCycleText = "0101000014";
                     break;

                 case (int)LifeCycleType.Monthly:
                     lifeCycleText = "0101000015";
                     break;
             }

             OMTUtility.ShowAlertWindow("무료 랜덤 미션 변경", Locale.GetTextFormat("0100010207", "main", Locale.GetText(lifeCycleText, "main"), GameData.Instance.GetMission(ElementId).Name)
                 + $"\n({Locale.GetTextFormat("0100010222","main", Locale.GetText(lifeCycleText, "main"))}: {GameData.FreeMissionRefreshCounts[(int)lifeCycleType] - User.Instance.UsedFreeRefreshCount[(int)lifeCycleType]})",
                 () => RefreshMission(lifeCycleType, true), null);
             return;
         }

         else if (User.Instance.UsedFreeRefreshCount[lifeCycleType] >= GameData.FreeMissionRefreshCounts[lifeCycleType])
         {
             if (User.Instance.Gem < GameData.MissionRefreshRequireGems[lifeCycleType])
             {
                 OMTUtility.ShowAlertWindow("보석 부족 알림", Locale.GetText("0100010209", "main"), null);
                 return;
             }

             else if (User.Instance.UsedPaidRefreshCount[lifeCycleType] >= GameData.PaidMissionRefreshCounts[lifeCycleType])
             {
                 OMTUtility.ShowAlertWindow("횟수 부족 알림", Locale.GetText("0100010210", "main"), null);
                 return;
             }
         }

         OMTUtility.ShowAlertWindow("유료 랜덤 미션 변경", Locale.GetTextFormat("0100010208", "main", GameData.MissionRefreshRequireGems[lifeCycleType], GameData.Instance.GetMission(ElementId).Name), () => RefreshMission(lifeCycleType, false), null);
     }*/

    public void ClickRefreshButton()
    {
        UserMission mission = User.Instance.GetUserMission(ElementId);
        int lifeCycleType = (int)GameData.Instance.GetMission(ElementId).LifeCycle;

        if (!GameData.IsRandomMission(ElementId) || mission.isComplete)
            return;

        if (User.Instance.UsedFreeRefreshCount < GameData.FreeMissionRefreshCounts)
        {
            string lifeCycleText = "";

            switch (lifeCycleType)
            {
                case (int)LifeCycleType.Daily:
                    lifeCycleText = "일일";
                    break;

                case (int)LifeCycleType.Weekly:
                    lifeCycleText = "주간";
                    break;

                case (int)LifeCycleType.Monthly:
                    lifeCycleText = "월간";
                    break;
            }

            Utility.ShowAlertWindow("무료 랜덤 미션 변경", $"무료 {lifeCycleText} 미션 변경 횟수를 소모하여\n'{GameData.Instance.GetMission(ElementId).Name}'을 변경하시겠습니까?"
                + $"\n(남은 {lifeCycleText} 무료 변경 횟수: {GameData.FreeMissionRefreshCounts - User.Instance.UsedFreeRefreshCount})",
                () => RefreshMission(lifeCycleType, true), null);
            return;
        }

        else if (User.Instance.UsedFreeRefreshCount >= GameData.FreeMissionRefreshCounts)
        {
            if (User.Instance.Gem < GameData.MissionRefreshRequireGems[lifeCycleType])
            {
                Utility.ShowAlertWindow("보석 부족 알림", "보석이 부족합니다.", null);
                return;
            }

            else if (User.Instance.UsedPaidRefreshCount >= GameData.PaidMissionRefreshCounts)
            {
                Utility.ShowAlertWindow("횟수 부족 알림", "미션 변경 횟수를 모두 소모하셨습니다.", null);
                return;
            }
        }

        Utility.ShowAlertWindow("유료 랜덤 미션 변경", $"보석을 {GameData.MissionRefreshRequireGems[lifeCycleType]}개 소모하여\n'{GameData.Instance.GetMission(ElementId).Name}'을(를) 변경하시겠습니까?", () => RefreshMission(lifeCycleType, false), null);
    }

    public void RefreshMission(int lifeCycleType, bool isFree)
    {
        User.Instance.ResetUserMission(ElementId);
        GameData.Instance.RefreshRandomMission(GameData.Instance.GetMission(ElementId), isFree);

        if (!isFree)
            User.Instance.Gem -= GameData.MissionRefreshRequireGems[lifeCycleType];

        GameObject.Find("UIMissionPanel").GetComponent<UIMainLobbyMissionPanelView>().UpdatePanel();
    }
}