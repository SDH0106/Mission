using Defective.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    public static readonly int[] FreeMissionRefreshCounts = new int[]
    {
        0, 1, 1, 1
    };

    public static readonly int[] PaidMissionRefreshCounts = new int[]
    {
        0, 3, 2, 1
    };

    public static readonly int[] MissionRefreshRequireGems = new int[]
    {
        0, 5, 10, 50
    };

    public AreaInfo lastUnlockedArea = TempDatas.TempArea[1];

    public static Dictionary<ulong, double> MonsterKillCountsByMonsterId = new Dictionary<ulong, double>();
    public static Dictionary<TempDatas.WeaponTypes, double> MonsterKillCountsByWeaponType = new Dictionary<TempDatas.WeaponTypes, double>();

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
        LoadJson();
    }
    void LoadJson()
    {
        LoadMissionJson();
    }

    #region mission
    public Dictionary<ulong, IMission> Mission;
    public List<ulong> CompleteMissions;
    public List<ulong> BasicMissions;
    public Dictionary<ulong, RandomMission> RandomMissions;

    public IMission GetMission(ulong missionId)
    {
        IMission mission = null;
        Mission.TryGetValue(missionId, out mission);
        return mission;
    }

    public RandomMission GetRandomMission(ulong missionId)
    {
        RandomMission mission = null;
        RandomMissions.TryGetValue(missionId, out mission);
        return mission;
    }

    public void AddMissionCount(MissionType type, double num)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                missionEnum.Current.Value.AddCount(num);
            }
        }
    }

    public void AddMissionCount(MissionType type, double num, int areaId)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                if (!(missionEnum.Current.Value).RandomMissionElements.ContainsKey(RandomMission.MissionElementTypes.Area))
                    continue;

                if (areaId != int.Parse((missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Area]))
                    continue;

                missionEnum.Current.Value.AddCount(num);
            }
        }
    }

    public void AddMissionCount(MissionType type, Dictionary<string, double> specificMonster, int areaId)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                if (!(missionEnum.Current.Value).RandomMissionElements.ContainsKey(RandomMission.MissionElementTypes.Area))
                    continue;

                if (areaId != int.Parse((missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Area]))
                    continue;

                double count = 0;

                string monsterNameId = (missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Monster];
                string monsterType = (missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.MonsterType];

                foreach (var monsterInfo in TempDatas.TempMonsterInfos)
                {
                    if (!specificMonster.ContainsKey(monsterInfo.Key.ToString()))
                        continue;

                    count += specificMonster[monsterInfo.Key.ToString()];
                }

                missionEnum.Current.Value.AddCount(count);
            }
        }

        if (type == MissionType.SpecificMonsterKill)
            MonsterKillCountsByMonsterId.Clear();
    }

    public void AddMissionCount(MissionType type, ulong[] mercenaries, int areaId)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                if (!(missionEnum.Current.Value).RandomMissionElements.ContainsKey(RandomMission.MissionElementTypes.Area))
                    continue;

                if (areaId != int.Parse((missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Area]))
                    continue;

                foreach (var mercenaryId in mercenaries)
                {
                    if (mercenaryId == 0)
                        continue;

                    missionEnum.Current.Value.AddCount(1);
                    return;
                }
            }
        }
    }

    public void AddMissionCount(MissionType type, double num, int areaId, ulong sideEventId)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                if (!(missionEnum.Current.Value).RandomMissionElements.ContainsKey(RandomMission.MissionElementTypes.Area))
                    continue;

                if (areaId != int.Parse((missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Area]))
                    continue;

                string[] ids = missionEnum.Current.Value.RandomMissionElements[RandomMission.MissionElementTypes.SideEvent].Split('/');

                if (ids.Contains(sideEventId.ToString()))
                    missionEnum.Current.Value.AddCount(num);
            }
        }
    }

    public void AddMissionCount(MissionType type, Dictionary<TempDatas.WeaponTypes, double> killCountByWeapon, int areaId)
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.Type == type)
            {
                if (!(missionEnum.Current.Value).RandomMissionElements.ContainsKey(RandomMission.MissionElementTypes.Area))
                    continue;

                if (areaId != int.Parse((missionEnum.Current.Value).RandomMissionElements[RandomMission.MissionElementTypes.Area]))
                    continue;

                TempDatas.WeaponTypes randomWeaponType = (TempDatas.WeaponTypes)int.Parse(missionEnum.Current.Value.RandomMissionElements[RandomMission.MissionElementTypes.Weapon]);

                if (killCountByWeapon.ContainsKey(randomWeaponType))
                    missionEnum.Current.Value.AddCount(killCountByWeapon[randomWeaponType]);
            }
        }

        if (type == MissionType.MonsterKillBySpecificWeapon)
            MonsterKillCountsByWeaponType.Clear();
    }

    public void ClearAllMissionCount()
    {
        var missionEnum = Mission.GetEnumerator();
        while (missionEnum.MoveNext())
        {
            User.Instance.ClearMissionExceptRandomElement(missionEnum.Current.Value.Id, missionEnum.Current.Value.RandomMissionElements);
        }
    }

    public void ClearMission(LifeCycleType lifeCycleType)
    {
        Dictionary<ulong, IMission> tempMission = new Dictionary<ulong, IMission>(Mission);
        var missionEnum = tempMission.GetEnumerator();

        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.LifeCycleType == lifeCycleType)
            {
                if (missionEnum.Current.Value.RandomMissionExplainElements != null)
                    RefreshRandomMission(Mission[missionEnum.Current.Key], true);

                else
                    User.Instance.ClearUserMission(missionEnum.Current.Value.Id);
            }
        }

        User.Instance.ClearUserMissionRefreshCount((int)lifeCycleType);
    }

    void LoadMissionJson()
    {
        Mission = new Dictionary<ulong, IMission>();
        RandomMissions = new Dictionary<ulong, RandomMission>();
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

            // 오른쪽에서 7,8번째 자리 
            // 0000000000
            //   ^^ 
            ulong missionCategory = (id / 1000000) % 100;

            // 일반 미션
            if (missionCategory == 00)
            {
                string explainId = jArray[i]["Explain"].Value<string>();
                int missionType = jArray[i]["Type"].Value<int>();
                double completionCount = jArray[i]["CompleteCount"].Value<double>();

                Mission.Add(id, Utility.CreateMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completionCount, rewardType, rewardAmount));
            }

            // 랜덤 미션
            else if (missionCategory == 01)
            {
                GetRandomMission(id, iconId, nameId, lifeCycleType, rewardType, rewardAmount);
            }

            if (Mission[id] == null)
            {
                Debug.LogError("Cannot Find Mission ::" + id);
            }
        }
    }

    public void RefreshAllRandomMission()
    {
        Dictionary<ulong, IMission> tempMission = new Dictionary<ulong, IMission>(Mission);
        var missionEnum = tempMission.GetEnumerator();

        while (missionEnum.MoveNext())
        {
            if (missionEnum.Current.Value.RandomMissionExplainElements != null)
            {
                RefreshRandomMission(Mission[missionEnum.Current.Key], true);
            }
        }
    }

    public void RefreshRandomMission(IMission mission, bool isResetRefreshCount)
    {
        ulong id = mission.Id;
        string iconId = mission.IconId;
        string nameId = RandomMissions[id].nameId;
        int lifeCycleType = (int)mission.LifeCycleType;
        int rewardType = (int)mission.RewardType;
        ulong rewardAmount = mission.RewardAmount;

        if (!isResetRefreshCount)
        {
            int userUsedfreeRefreshCount = User.Instance.UsedFreeRefreshCount[lifeCycleType];
            int userUsedPaidRefreshCount = User.Instance.UsedPaidRefreshCount[lifeCycleType];

            if (FreeMissionRefreshCounts[lifeCycleType] <= userUsedfreeRefreshCount)
                User.Instance.SetMissionRefreshCount((LifeCycleType)lifeCycleType, userUsedfreeRefreshCount, ++userUsedPaidRefreshCount);
            else
                User.Instance.SetMissionRefreshCount((LifeCycleType)lifeCycleType, ++userUsedfreeRefreshCount, 0);
        }

        User.Instance.ClearUserMission(id);

        GetRandomMission(id, iconId, nameId, lifeCycleType, rewardType, rewardAmount);
    }

    public void GetRandomMission(ulong id, string iconId, string nameId, int lifeCycleType, int rewardType, ulong rewardAmount)
    {
        RandomMission randomMission;

        if (User.Instance.GetUserMission(id).randomMissionElements == null)
        {
            randomMission = new RandomMission(lifeCycleType, iconId, nameId);
        }

        else
        {
            foreach (var element in User.Instance.GetUserMission(id).randomMissionElements)
            {
                if (element.Value == null)
                {
                    Debug.LogError("Have UserMission Data But No Key in Dictionary");
                    break;
                }
            }

            randomMission = new RandomMission(User.Instance.GetUserMission(id), lifeCycleType, iconId, nameId);
        }

        string explainId = randomMission.explainId;
        int missionType = (int)randomMission.Type;
        double completionCount = randomMission.CompletionCount;
        List<string> missionExplainElements = randomMission.MissionExplainElement;
        Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements = randomMission.RandomMissionElements;

        User.Instance.SetMissionElements(id, randomMissionElements);

        if (Mission.ContainsKey(id))
            Mission[id] = Utility.CreateMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completionCount, rewardType, rewardAmount, missionExplainElements, randomMissionElements);
        else
            Mission.Add(id, Utility.CreateMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completionCount, rewardType, rewardAmount, missionExplainElements, randomMissionElements));

        if (RandomMissions.ContainsKey(id))
            RandomMissions[id] = randomMission;
        else
            RandomMissions.Add(id, randomMission);

        Debug.Log(id + ": " + (MissionType)missionType + " " + string.Join(",", missionExplainElements));
    }
    #endregion

    public int CalculateLastUnlockedArea()
    {
        return lastUnlockedArea.areaId;
    }
}
