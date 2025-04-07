using Defective.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class GameData
{
    public static readonly int FreeMissionRefreshCounts = 1;

    public static readonly int PaidMissionRefreshCounts = 2;

    public static readonly int[] MissionRefreshRequireGems = new int[]
    {
        0, 5, 10, 50
    };

    public static readonly Dictionary<User.ClassType, int> DicCharacterRequireLevel = new Dictionary<User.ClassType, int>()
    {
        { User.ClassType.Charater1,0},
        { User.ClassType.Charater2,8},
        { User.ClassType.Charater3,60},
        { User.ClassType.Charater4,230},
    };

    public static readonly Dictionary<int, string> MonsterType = new Dictionary<int, string>()
    {
        { 1, "노말" },
        { 2, "엘리트" },
        { 3, "네임드" },
        { 4, "보스" }
    };

    public AreaInfo lastUnlockedArea = TempDatas.TempArea[1];

    public static Dictionary<ulong, double> MonsterKillCountsByMonsterId = new Dictionary<ulong, double>();
    public static Dictionary<TempDatas.WeaponTypes, double> MonsterKillCountsByWeaponType = new Dictionary<TempDatas.WeaponTypes, double>();

    public System.DateTime currentTime;

    #region instance
    static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameData();
            }
            return _instance;
        }
    }
    #endregion

    GameData()
    {
        missionReporter = new MissionReporter(AddMissionCount, SetMissionCount);
        randomMissionGenerator = new RandomMissionGenerator();

        LoadJson();
    }
    void LoadJson()
    {
        LoadMissionJson();
        LoadMonsterData();
        LoadBodyInfoData();
        LoadPetData();
        LoadStatData();
    }

    private void LoadStatData()
    {
        Stats.Add(0, "Stat1");
        Stats.Add(1, "Stat2");
        Stats.Add(2, "Stat3");
        Stats.Add(3, "Stat4");
    }

    private void LoadMonsterData()
    {
        Monsters.Add(200110001, "NormalMonster1");
        Monsters.Add(200110002, "NormalMonster2");
        Monsters.Add(200210001, "EliteMonster1");
        Monsters.Add(200310001, "NamedMonster1");
        Monsters.Add(200410001, "BossMonster1");
    }

    private void LoadPetData()
    {
        Pets.Add(0, "Pet1");
        Pets.Add(1, "Pet2");
        Pets.Add(2, "Pet3");
        Pets.Add(3, "Pet4");
        Pets.Add(4, "Pet5");
    }

    private void LoadBodyInfoData()
    {
        Bodies.Add(0, "Character1");
        Bodies.Add(1, "Character2");
        Bodies.Add(2, "Character3");
        Bodies.Add(3, "Character4");
    }

    #region Mission
    public Dictionary<ulong, IMission> Missions = new Dictionary<ulong, IMission>();
    public List<ulong> CompleteMissions = new List<ulong>();

    public MissionReporter missionReporter;
    public RandomMissionGenerator randomMissionGenerator;

    public IMission GetMission(ulong missionId)
    {
        IMission mission = null;
        Missions.TryGetValue(missionId, out mission);
        return mission;
    }

    public static bool IsRandomMission(ulong id)
    {
        return (id / 1000000) % 100 == 01;
    }

    public void RefreshAllRandomMissions()
    {
        Dictionary<ulong, IMission> tempMission = new Dictionary<ulong, IMission>(Missions);
        var missionEnum = tempMission.GetEnumerator();

        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.MissionExplainElements != null)
            {
                User.Instance.ResetUserMission(missionEnum.Current.Key);
                RefreshRandomMission(Missions[missionEnum.Current.Key], false);
            }
        }
    }

    public void RefreshRandomMission(IMission mission, bool isUseRefreshCount)
    {
        if (isUseRefreshCount)
        {
            int userUsedFreeRefreshCount = User.Instance.UsedFreeRefreshCount;
            int userUsedPaidRefreshCount = User.Instance.UsedPaidRefreshCount;

            Debug.Log(userUsedFreeRefreshCount);
            Debug.Log(userUsedPaidRefreshCount);

            if (FreeMissionRefreshCounts <= userUsedFreeRefreshCount)
                User.Instance.SetMissionRefreshCount(userUsedFreeRefreshCount, ++userUsedPaidRefreshCount);
            else
                User.Instance.SetMissionRefreshCount(++userUsedFreeRefreshCount, 0);

            Debug.Log(userUsedFreeRefreshCount);
            Debug.Log(userUsedPaidRefreshCount);
        }

        GenerateRandomMission(mission);
    }

    public void ResetMissions(LifeCycleType lifeCycleType)
    {
        Dictionary<ulong, IMission> tempMission = new Dictionary<ulong, IMission>(Missions);
        var missionEnum = tempMission.GetEnumerator();

        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.LifeCycle == lifeCycleType)
            {
                User.Instance.ResetUserMission(missionEnum.Current.Value.Id);
            }
        }
    }

    public void GenerateAllRandomMissions()
    {
        Dictionary<ulong, IMission> tempMissions = new Dictionary<ulong, IMission>(Missions);

        foreach (var mission in tempMissions)
        {
            if (IsRandomMission(mission.Key))
            {
                GenerateRandomMission(GetMission(mission.Key));
            }
        }
    }

    public void GenerateRandomMission(IMission mission)
    {
        UserMission userMission = User.Instance.GetUserMission(mission.Id);

        MissionObjective[] missionObjectives = null;

        ulong id = mission.Id;

        if (userMission.objectives == null)
        {
            missionObjectives = randomMissionGenerator.GetRandomMissionElements(mission);
        }
        else
        {
            if (userMission.objectives.Length == 0 || userMission.objectives.Any(x => string.IsNullOrEmpty(x.condition1)))
            {
                Debug.LogError($"{mission.Id}: Empty objective or category");
                missionObjectives = randomMissionGenerator.GetRandomMissionElements(mission);
            }
            else
            {
                missionObjectives = userMission.objectives;
            }
        }

        randomMissionGenerator.SetMissionCompleteCount(mission, missionObjectives);
        randomMissionGenerator.SetExplainElementsByMissionObjective(mission, missionObjectives);
        randomMissionGenerator.SetMissionExplainByMissionObjective(mission, missionObjectives);

        User.Instance.SetMissionObjectives(id, missionObjectives);

        User.Instance.Save();

        int lifeCycleType = (int)mission.LifeCycle;
        int rewardType = (int)mission.RewardType;
        ulong rewardAmount = mission.RewardAmount;
        string iconId = mission.IconId;
        string nameId = mission.NameId;
        string explainId = mission.ExplainId;
        double completionCount = mission.CompleteCount;
        List<string> missionExplainElement = mission.MissionExplainElements;

        if (Missions.ContainsKey(id))
            Missions[id] = CreateMission(id, iconId, lifeCycleType, nameId, explainId, completionCount, rewardType, rewardAmount, missionExplainElement, missionObjectives);
        else
            Missions.Add(id, CreateMission(id, iconId, lifeCycleType, nameId, explainId, completionCount, rewardType, rewardAmount, missionExplainElement, missionObjectives));
    }

    bool CompareMissionObjective(string missionTypeCategory, MissionObjective[] userMissionObjective, MissionObjective[] missionObjective)
    {
        if (userMissionObjective == null || userMissionObjective.Length == 0)
            Debug.LogError("UserMissionObjective is Null or Empty");
        if (missionObjective == null)
            Debug.LogError("MissionObjective is Null or Empty");

        if (!userMissionObjective.Any(x => x.condition1 == missionTypeCategory))
            return false;

        var groups = userMissionObjective.GroupBy(x => x.groupId);
        var objsGroups = missionObjective.GroupBy(x => x.groupId);

        bool success = true;

        foreach (var group in groups)
        {
            foreach (var objsGroup in objsGroups)
            {
                success = true;

                foreach (var userMissionObj in group)
                {
                    if (objsGroup.Where(item => userMissionObj.condition1 == item.condition1)
                    .All(item => userMissionObj.condition2 != item.condition2))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                    return true;
            }
        }

        return success;
    }

    public void AddMissionCount(string missionTypeCategory, MissionObjective[] objs, double num)
    {
        foreach (var mission in Missions)
        {
            if (mission.Value.IsComplete || mission.Value.IsReward)
                continue;

            if (mission.Value.MissionObjectives == null)
                continue;

            bool success = CompareMissionObjective(missionTypeCategory, mission.Value.MissionObjectives, objs);

            if (success)
            {
                mission.Value.AddCount(num);

                if (mission.Value.IsComplete)
                    missionReporter.ReportMissionComplete(mission.Value.Id);
            }
        }
    }

    public void SetMissionCount(string missionTypeCategory, MissionObjective[] objs, double num)
    {
        foreach (var mission in Missions)
        {
            if (mission.Value.IsComplete || mission.Value.IsReward)
                continue;

            if (mission.Value.MissionObjectives == null)
                continue;

            bool success = CompareMissionObjective(missionTypeCategory, mission.Value.MissionObjectives, objs);

            if (success)
            {
                mission.Value.SetCount(num);

                if (mission.Value.IsComplete)
                    missionReporter.ReportMissionComplete(mission.Value.Id);
            }
        }
    }

    public static IMission CreateMission(ulong id, string iconId, int lifeCycleType, string nameId, string explainId, double completeCount, int rewardType, ulong rewardAmount, List<string> missionExplainElement, MissionObjective[] missionObjectives)
    {
        ulong type = (id / 10000) % 100;

        switch (type)
        {
            case 01:
                return new DailyMission(id, iconId, lifeCycleType, nameId, explainId, completeCount, rewardType, rewardAmount, missionExplainElement, missionObjectives);
            case 02:
                return new WeeklyMission(id, iconId, lifeCycleType, nameId, explainId, completeCount, rewardType, rewardAmount, missionExplainElement, missionObjectives);
            case 03:
                return new MonthlyMission(id, iconId, lifeCycleType, nameId, explainId, completeCount, rewardType, rewardAmount, missionExplainElement, missionObjectives);
            default:
                Debug.LogError("mission type not found. [" + id + "]");
                return null;
        }
    }

    void LoadMissionJson()
    {
        Missions = new Dictionary<ulong, IMission>();
        CompleteMissions = new List<ulong>();

        TextAsset jsonText = (TextAsset)Resources.Load("JsonData/Mission", typeof(TextAsset));
        JArray jArray = JArray.Parse(jsonText.text);
        for (int i = 0; i < jArray.Count; i++)
        {
            ulong id = jArray[i]["Id"].Value<ulong>();
            int lifeCycleType = jArray[i]["LifeCycle"].Value<int>();
            int rewardType = jArray[i]["RewardType"].Value<int>();
            ulong rewardAmount = jArray[i]["RewardAmount"].Value<ulong>();
            string iconId = jArray[i]["IconId"].Value<string>();
            string nameId = jArray[i]["Name"].Value<string>();
            string explainId = jArray[i]["Explain"].Value<string>();
            double completionCount = jArray[i]["CompleteCount"].Value<double>();

            string[] condition1 = jArray[i]["MissionObjective_Conditions1"].Value<string>().Split(',').Select(x => x.Trim()).ToArray();
            string[] condition2 = jArray[i]["MissionObjective_Conditions2"].Value<string>().Split(',').Select(x => x.Trim()).ToArray();
            string[] objectiveCount = jArray[i]["MissionObjective_ObjectiveCounts"].Value<string>().Split(',').Select(x => x.Trim()).ToArray();
            string[] groupId = jArray[i]["MissionObjective_GroupIds"].Value<string>().Split(',').Select(x => x.Trim()).ToArray();

            // 오른쪽에서 7,8번째 자리 
            // 0000000000
            //   ^^ 
            MissionObjective[] missionObj;

            if (IsRandomMission(id))
                missionObj = User.Instance.GetUserMission(id).objectives;
            else
            {
                missionObj = new MissionObjective[condition1.Length];

                for (int j = 0; j < missionObj.Length; j++)
                {
                    missionObj[j].condition1 = string.IsNullOrEmpty(condition1[j]) ? MissionConditionKeys.Null : condition1[j];
                    missionObj[j].condition2 = string.IsNullOrEmpty(condition2[j]) ? MissionConditionKeys.Null : condition2[j];
                    missionObj[j].objectiveCount = double.Parse(objectiveCount[j]);
                    missionObj[j].groupId = int.Parse(groupId[j]);
                }

                User.Instance.SetMissionObjectives(id, missionObj);
            }

            Missions.Add(id, CreateMission(id, iconId, lifeCycleType, nameId, explainId, completionCount, rewardType, rewardAmount, null, missionObj));

            if (Missions[id] == null)
            {
                Debug.LogError("Cannot Find Mission ::" + id);
            }
        }
    }

    #endregion

    #region Stat
    public readonly Dictionary<ulong, string> Stats = new Dictionary<ulong, string>();
    #endregion

    #region Monster
    public Dictionary<ulong, string> Monsters = new Dictionary<ulong, string>();
    #endregion

    #region Pet
    public Dictionary<ulong, string> Pets = new Dictionary<ulong, string>();
    #endregion

    #region BodyInfo
    public Dictionary<int, string> Bodies = new Dictionary<int, string>();
    #endregion
}
