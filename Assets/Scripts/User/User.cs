using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class User
{
    static User _instance;
    public static User Instance
    {
        get
        {
            if (_instance == null)
                _instance = new User();
            return _instance;
        }
    }

    User()
    {
        Load();
    }

    public const string SavedJsonPrefsKey = "SavedJson";

    Dictionary<ulong, UserMission> _userMissions;

    int[] _usedMissionFreeRefreshCount = new int[4];
    int[] _usedMissionPaidRefreshCount = new int[4];

    public double Gem = 100;
    public double Relic = 50;
    public double Coin = 10;

    public int[] UsedFreeRefreshCount
    {
        get { return _usedMissionFreeRefreshCount; }
    }

    public int[] UsedPaidRefreshCount
    {
        get { return _usedMissionPaidRefreshCount; }
    }

    public UserMission GetUserMission(ulong missionId)
    {
        UserMission mission;
        _userMissions.TryGetValue(missionId, out mission);

        return mission;
    }

    public void SetMissionCount(ulong missionId, double count)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, count, _userMissions[missionId].isComplete, _userMissions[missionId].isReward, _userMissions[missionId].randomMissionElements);
        else
            _userMissions.Add(missionId, new UserMission(missionId, count, false, false, null));
    }

    public void SetMissionComplete(ulong missionId, bool isComplete)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, isComplete, _userMissions[missionId].isReward, _userMissions[missionId].randomMissionElements);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, isComplete, false, null));
    }

    public void SetMissionReward(ulong missionId, bool isReward)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, _userMissions[missionId].isComplete, isReward, _userMissions[missionId].randomMissionElements);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, isReward, null));
    }

    public void SetMissionElements(ulong missionId, Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, _userMissions[missionId].isComplete, _userMissions[missionId].isReward, randomMissionElements);

        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, false, randomMissionElements));
    }

    public void SetMissionRefreshCount(LifeCycleType type, int useFreeRefreshCount, int usePaidRefreshCount)
    {
        _usedMissionFreeRefreshCount[(int)type] = useFreeRefreshCount;
        _usedMissionPaidRefreshCount[(int)type] = usePaidRefreshCount;
    }

    public void ClearMissionExceptRandomElement(ulong missionId, Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, 0, false, false, randomMissionElements);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, false, randomMissionElements));
    }

    public void ClearUserMission(ulong missionId)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, 0, false, false, null);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, false, null));
    }

    public void ClearUserMissionRefreshCount(int lifeCycleType)
    {
        Debug.Log("ResetRefCount");

        _usedMissionFreeRefreshCount[lifeCycleType] = 0;
        _usedMissionPaidRefreshCount[lifeCycleType] = 0;
    }

    public enum UserPropertyCategory
    {
        FirstLogin, UserName, BodyId, Chapter, MaxHealth,
        Exp, Gem, Relic,
        UserQuests, UserWeapons, UserStats, UserMissions, EquippedWeaponId, ScrambleWeaponTotalLevel,
        MonsterKillCountTotal,
        UserCostumes, EquippedCostume,
        UserPets, EquippedPets,
        DefenceMonsterTotalKillCount, DefenceMonsterMaxKillCount, UserBarricades, EquippedBarricadeId,
        ScrambleHighScore,
        DailyClearLog, WeeklyClearLog, MonthlyClearLog,
        VipPoint, VipLevel, IsVipDailyReward, IsVipRateUpRewards,
        IsAutoMode, IsEndlessBoss,
        TutorialClear, GuildId,
        UserItmes, UserShopPackages, EquippedForceMetal, AttackCountForceMetal,
        UserPosts, AttendanceCount, ReceivedAttendanceRewards, TotalExp,
        Act, Level, RaidTotalScore,
        DualWinLoseResult, GuildWinResults, GuildWinResultIds,
        GuildPvpDualCount, GuildRaidBattleCounts, RaidBattleCounts,
        DualPoint, Tier, IsUseForceMetal, Guilds,
        AdsTotalRemainCount, AdsRemainCounts, AdsCountResetTime, TimeOfFirstUnityAdPlayed,
        UserAreaInfo, DefenceWeaponLevel, DefenceBarricadeLevel, TeamSkillCooldown,
        ModeLevel, WeaponType, DefenceWeaponsLevel, TopScore, BarrierCooldown, LastLogoutTime,
        UserMissionRefreshCount,
    }
}
