using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class RandomMissionGenerator
{
    List<MissionObjective> missionObjectives;

    List<string> randomMissionType = new List<string>
    {
        MissionConditionKeys.GameClear, MissionConditionKeys.MonsterKill,
        MissionConditionKeys.StatUpgrade, MissionConditionKeys.Warp
    };

    string selectedMissionType;

    public MissionObjective[] GetRandomMissionElements(IMission mission)
    {
        missionObjectives = new List<MissionObjective>();

        CallFuncToNeedMissionElement(mission, GetNeedElementListByMissionType(mission));

        return missionObjectives.ToArray();
    }

    List<string> GetNeedElementListByMissionType(IMission mission)
    {
        List<string> needElementList = new List<string>();

        selectedMissionType = randomMissionType[UnityEngine.Random.Range(0, randomMissionType.Count)];

        switch (selectedMissionType)
        {
            case MissionConditionKeys.MonsterKill:
                needElementList.Add(MissionConditionKeys.Chapter);
                needElementList.Add(MissionConditionKeys.MonsterType);
                needElementList.Add(MissionConditionKeys.MonsterId);
                break;

            case MissionConditionKeys.GameClear:
                List<string> randomGameClearMissionTypes = new List<string>();

                randomGameClearMissionTypes.Add(MissionConditionKeys.BodyId);
                randomGameClearMissionTypes.Add(MissionConditionKeys.PetId);

                string randomType = randomGameClearMissionTypes[UnityEngine.Random.Range(0, randomGameClearMissionTypes.Count)];

                if (randomType == MissionConditionKeys.PetId)
                {
                    needElementList.Add(MissionConditionKeys.PetId);
                    needElementList.Add(MissionConditionKeys.Chapter);
                }
                else if (randomType == MissionConditionKeys.BodyId)
                {
                    needElementList.Add(MissionConditionKeys.BodyId);
                    needElementList.Add(MissionConditionKeys.Chapter);
                }

                missionObjectives.Add(new MissionObjective(MissionConditionKeys.GameClear, MissionConditionKeys.Null, MathF.Pow(3, (int)mission.LifeCycle), 0));
                break;

            case MissionConditionKeys.StatUpgrade:
                needElementList.Add(MissionConditionKeys.StatId);
                missionObjectives.Add(new MissionObjective(MissionConditionKeys.StatUpgrade, MissionConditionKeys.Null, Mathf.Clamp(Mathf.Pow(5, (int)mission.LifeCycle - 1), 1, 10), 0));
                break;

            case MissionConditionKeys.Warp:
                needElementList.Add(MissionConditionKeys.Chapter);
                missionObjectives.Add(new MissionObjective(MissionConditionKeys.Warp, MissionConditionKeys.Null, MathF.Pow(3, (int)mission.LifeCycle), 0));
                break;

            default:
                break;
        }

        return needElementList;
    }

    void CallFuncToNeedMissionElement(IMission mission, List<string> elements)
    {
        int randomChapter = -1;

        for (int i = 0; i < elements.Count; i++)
        {
            switch (elements[i])
            {
                case MissionConditionKeys.Chapter:
                    if (randomChapter == -1)
                        randomChapter = GetRandomArea();
                    break;

                case MissionConditionKeys.MonsterId:
                    if (randomChapter == -1)
                        randomChapter = GetRandomArea();
                    GetRandomMonster(mission, randomChapter);
                    break;

                case MissionConditionKeys.PetId:
                    GetRandomPet();
                    break;

                case MissionConditionKeys.StatId:
                    GetRandomStat();
                    break;

                case MissionConditionKeys.BodyId:
                    GetRandomBodyId();
                    break;
            }
        }
    }

    public void SetMissionCompleteCount(IMission mission, MissionObjective[] missionObjs)
    {
        for (int i = 0; i < randomMissionType.Count; ++i)
        {
            if (missionObjs.Any(x => x.condition1 == randomMissionType[i]))
            {
                mission.CompleteCount = missionObjs.Where(x => x.condition1 == randomMissionType[i]).Select(x => x.objectiveCount).First();
                break;
            }
        }
    }

    public void SetExplainElementsByMissionObjective(IMission mission, MissionObjective[] missionObjs)
    {
        mission.MissionExplainElements = new List<string>();

        for (int i = 0; i < missionObjs.Count(); i++)
        {
            switch (missionObjs[i].condition1)
            {
                case MissionConditionKeys.Chapter:
                    int areaId = int.Parse(missionObjs[i].condition2);
                    mission.MissionExplainElements.Add(areaId.ToString());
                    break;

                case MissionConditionKeys.StatId:
                    string statName = GameData.Instance.Stats[ulong.Parse(missionObjs[i].condition2)];
                    mission.MissionExplainElements.Add(statName);
                    break;

                case MissionConditionKeys.MonsterId:
                    string monsterName = GameData.Instance.Monsters[ulong.Parse(missionObjs[i].condition2)];
                    mission.MissionExplainElements.Add(monsterName);
                    break;

                case MissionConditionKeys.MonsterType:
                    mission.MissionExplainElements.Add(GameData.MonsterType[int.Parse(missionObjs[i].condition2)]);
                    break;

                case MissionConditionKeys.PetId:
                    mission.MissionExplainElements.Add(GameData.Instance.Pets[ulong.Parse(missionObjs[i].condition2)]);
                    break;

                case MissionConditionKeys.BodyId:
                    mission.MissionExplainElements.Add(GameData.Instance.Bodies[int.Parse(missionObjs[i].condition2)]);
                    break;
            }
        }

        mission.MissionExplainElements.Add(mission.CompleteCount.ToString());
    }

    public void SetMissionExplainByMissionObjective(IMission mission, MissionObjective[] missionObjs)
    {
        for (int i = 0; i < missionObjs.Count(); i++)
        {
            switch (missionObjs[i].condition1)
            {
                case MissionConditionKeys.GameClear:
                    if (missionObjs.Any(x => x.condition1 == MissionConditionKeys.BodyId))
                        mission.ExplainId = "캐릭터 {0}과(와) {1} 챕터 {2}회 클리어";
                    else if (missionObjs.Any(x => x.condition1 == MissionConditionKeys.PetId))
                        mission.ExplainId = "펫 {0}과(와) 함께 {1} 챕터 {2}회 클리어";
                    break;

                case MissionConditionKeys.MonsterKill:
                    mission.ExplainId = "{0} 챕터에서 {1} 등급 {2}을(를) {3}마리 처치";
                    break;

                case MissionConditionKeys.StatUpgrade:
                    mission.ExplainId = "능력치 '{0}' 레벨업 {1}회 수행";
                    break;

                case MissionConditionKeys.Warp:
                    mission.ExplainId = "{0} 챕터에서 워프 이벤트 {1}회 완료";
                    break;
            }
        }
    }

    int GetRandomArea()
    {
        var randomChapter = UnityEngine.Random.Range(1, User.Instance.Chapter + 1);

        missionObjectives.Add(new MissionObjective(MissionConditionKeys.Chapter, randomChapter.ToString(), 0, 0));

        return randomChapter;
    }

    void GetRandomMonster(IMission mission, int randomMissionChater)
    {
        ulong randomMonsterID = GameData.Instance.Monsters.ElementAt(UnityEngine.Random.Range(0, GameData.Instance.Monsters.Count)).Key;

        int monsterType = ((int)randomMonsterID / 100000) % 10;
        missionObjectives.Add(new MissionObjective(MissionConditionKeys.MonsterType, monsterType.ToString(), 0, 0));
        missionObjectives.Add(new MissionObjective(MissionConditionKeys.MonsterId, randomMonsterID.ToString(), 0, 0));

        double monsterKillCount = Mathf.Clamp((int)MathF.Pow(10, 4 - monsterType), 3, 300) * (int)mission.LifeCycle;

        missionObjectives.Add(new MissionObjective(MissionConditionKeys.MonsterKill, MissionConditionKeys.Null, monsterKillCount, 0));
    }

    void GetRandomPet()
    {
        List<ulong> PetIds = new List<ulong>();

        foreach (var pet in GameData.Instance.Pets)
        {
            ulong id = pet.Key;
            PetIds.Add(id);
        }

        ulong randomPetId = PetIds[UnityEngine.Random.Range(0, PetIds.Count)];

        missionObjectives.Add(new MissionObjective(MissionConditionKeys.PetId, randomPetId.ToString("D10"), 0, 0));
    }

    void GetRandomStat()
    {
        Dictionary<ulong, float> weightsByStatType = new Dictionary<ulong, float>();

        foreach (var stat in GameData.Instance.Stats)
            weightsByStatType.Add(stat.Key, Mathf.Clamp(User.Instance.Chapter / 10, 0, 10));

        float totalWeight = 0;

        foreach (var weight in weightsByStatType)
            totalWeight += weight.Value;

        float random = UnityEngine.Random.Range(1, totalWeight);

        float tempWeight = 0;
        ulong randomStatId = 0;

        foreach (var weight in weightsByStatType)
        {
            tempWeight += weight.Value;

            if (tempWeight >= random)
            {
                randomStatId = weight.Key;
                break;
            }
        }

        missionObjectives.Add(new MissionObjective(MissionConditionKeys.StatId, randomStatId.ToString("D10"), 0, 0));
    }

    void GetRandomBodyId()
    {
        List<int> bodies = new List<int>();

        for (int i = 0; i < Enum.GetValues(typeof(User.ClassType)).Length; ++i)
        {
            if (User.Instance.Level >= GameData.DicCharacterRequireLevel[(User.ClassType)i])
                bodies.Add(i);
        }

        int randomBody = bodies[UnityEngine.Random.Range(0, bodies.Count)];

        missionObjectives.Add(new MissionObjective(MissionConditionKeys.BodyId, randomBody.ToString(), 0, 0));
    }
}