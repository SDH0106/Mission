using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class User
{
    public enum ClassType : int
    {
        Charater1, Charater2, Charater3, Charater4
    }

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
    public ulong _userPet = 0;
    public int _chapter = 10;
    public int Chapter => _chapter;

    public int _level = 25;
    public int Level => _level;

    public int _userBody = 0;

    int _usedMissionFreeRefreshCount;
    int _usedMissionPaidRefreshCount;

    public double Gem = 100;
    public double Relic = 50;
    public double Coin = 10;

    public Action OnValuesChange;

    public int UsedFreeRefreshCount
    {
        get { return _usedMissionFreeRefreshCount; }
    }

    public int UsedPaidRefreshCount
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
            _userMissions[missionId] = new UserMission(missionId, count, _userMissions[missionId].isComplete, _userMissions[missionId].isReward, _userMissions[missionId].objectives);
        else
            _userMissions.Add(missionId, new UserMission(missionId, count, false, false, null));

        NotifyValuesChanged();
    }

    public void SetMissionComplete(ulong missionId, bool isComplete)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, isComplete, _userMissions[missionId].isReward, _userMissions[missionId].objectives);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, isComplete, false, null));

        NotifyValuesChanged();
    }

    public void SetMissionReward(ulong missionId, bool isReward)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, _userMissions[missionId].isComplete, isReward, _userMissions[missionId].objectives);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, isReward, null));

        NotifyValuesChanged();
    }
    public void SetMissionObjectives(ulong missionId, MissionObjective[] missionObejctive)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, _userMissions[missionId].count, _userMissions[missionId].isComplete, _userMissions[missionId].isReward, missionObejctive);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, false, missionObejctive));

        NotifyValuesChanged();
    }

    public void SetMissionRefreshCount(int usedFreeRefreshCount, int usedPaidRefreshCount)
    {
        _usedMissionFreeRefreshCount = usedFreeRefreshCount;
        _usedMissionPaidRefreshCount = usedPaidRefreshCount;

        NotifyValuesChanged();
    }
    public void ResetUserMission(ulong missionId)
    {
        if (_userMissions.ContainsKey(missionId))
            _userMissions[missionId] = new UserMission(missionId, 0, false, false, null);
        else
            _userMissions.Add(missionId, new UserMission(missionId, 0, false, false, null));

        NotifyValuesChanged();
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

    void NotifyValuesChanged()
    {
        if (OnValuesChange != null)
            OnValuesChange();
    }
}
