using Defective.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMission
{
    public string IconId { get; set; }
    public string nameId;
    public string explainId;
    public MissionType Type { get; set; }
    public LifeCycleType LifeCycleType { get; set; }
    public double CompletionCount { get; set; }

    List<string> missionExplainElement = new List<string>();
    public List<string> MissionExplainElement => missionExplainElement;

    Dictionary<MissionElementTypes, string> randomMissionElements;
    public Dictionary<MissionElementTypes, string> RandomMissionElements => randomMissionElements;

    public enum MissionElementTypes { IconId, NameId, ExplainId, MissionType, Domain, Area, Monster, MonsterType, MissionCount, Weapon, Mercenary, SideEvent }

    public enum SideEventType { None, Human, Animal, Missile, Count }

    List<MissionType> randomMissionTypes = new List<MissionType>() { MissionType.SpecificMonsterKill, MissionType.ClearWithSpecificMercenary, MissionType.DestroyBuilding, MissionType.SideEventSuccess, MissionType.MonsterKillBySpecificWeapon };

    public RandomMission(int lifeCycleType, string iconId, string nameId)
    {
        LifeCycleType = (LifeCycleType)lifeCycleType;

        this.Type = randomMissionTypes[UnityEngine.Random.Range(0, randomMissionTypes.Count)];
        this.IconId = iconId;
        this.nameId = nameId;

        randomMissionElements = null;
        GetRandomMissionElements(Type);

        randomMissionElements.Add(MissionElementTypes.IconId, IconId);
        randomMissionElements.Add(MissionElementTypes.NameId, nameId);
        randomMissionElements.Add(MissionElementTypes.ExplainId, explainId);
        randomMissionElements.Add(MissionElementTypes.MissionType, ((int)Type).ToString());
    }

    public RandomMission(UserMission userMission, int lifeCycleType, string iconId, string nameId)
    {
        LifeCycleType = (LifeCycleType)lifeCycleType;

        randomMissionElements = userMission.randomMissionElements;
        IconId = randomMissionElements[MissionElementTypes.IconId];
        nameId = randomMissionElements[MissionElementTypes.NameId];
        IconId = iconId;
        this.nameId = nameId;
        explainId = randomMissionElements[MissionElementTypes.ExplainId];
        Type = (MissionType)(int.Parse(randomMissionElements[MissionElementTypes.MissionType]));
        CompletionCount = int.Parse(randomMissionElements[MissionElementTypes.MissionCount]);

        GetRandomMissionElements(Type);
    }

    public RandomMission() { }

    void GetRandomMissionElements(MissionType missionType)
    {
        List<MissionElementTypes> needElementList = GetNeedElementListByMissionType(missionType);

        if (randomMissionElements == null)
        {
            randomMissionElements = new Dictionary<MissionElementTypes, string>();

            CallFuncToNeedMissionElement(needElementList);
        }

        else
        {
            for (int i = 0; i < needElementList.Count; i++)
            {
                GetExplainElementsByMissionType(needElementList[i]);
            }
        }
    }

    Dictionary<SideEventType, List<ulong>> SideEventActorIdDic = new Dictionary<SideEventType, List<ulong>>();

    List<MissionElementTypes> GetNeedElementListByMissionType(MissionType missionType)
    {
        List<MissionElementTypes> needElementList = new List<MissionElementTypes>();

        switch (missionType)
        {
            case MissionType.SpecificMonsterKill:
                explainId = "0100052001";
                needElementList.Add(MissionElementTypes.Area);
                needElementList.Add(MissionElementTypes.Monster);
                needElementList.Add(MissionElementTypes.MissionCount);
                break;

            case MissionType.ClearWithSpecificMercenary:
                explainId = "0100052002";
                needElementList.Add(MissionElementTypes.Mercenary);
                needElementList.Add(MissionElementTypes.Area);
                needElementList.Add(MissionElementTypes.MissionCount);
                break;

            case MissionType.DestroyBuilding:
                explainId = "0100052003";
                needElementList.Add(MissionElementTypes.Area);
                needElementList.Add(MissionElementTypes.MissionCount);
                break;

            case MissionType.SideEventSuccess:
                // 사이드 이벤트 타입별 explainId => GetRandomSideEvent 함수에 타입별로 구분
                needElementList.Add(MissionElementTypes.Area);
                needElementList.Add(MissionElementTypes.SideEvent);
                needElementList.Add(MissionElementTypes.MissionCount);
                break;

            case MissionType.MonsterKillBySpecificWeapon:
                explainId = "0100052009";
                needElementList.Add(MissionElementTypes.Weapon);
                needElementList.Add(MissionElementTypes.Area);
                needElementList.Add(MissionElementTypes.MissionCount);
                break;

            default:
                break;
        }

        return needElementList;
    }

    void CallFuncToNeedMissionElement(List<MissionElementTypes> elements)
    {
        AreaInfo randomArea = null;

        for (int i = 0; i < elements.Count; i++)
        {
            switch (elements[i])
            {
                case MissionElementTypes.Area:
                    if (randomArea == null)
                        randomArea = GetRandomArea();
                    break;

                case MissionElementTypes.MissionCount:
                    GetRandomMissionClearCount();
                    break;

                case MissionElementTypes.Monster:
                    if (randomArea == null)
                        randomArea = GetRandomArea();
                    GetRandomMonster(randomArea);
                    break;

                case MissionElementTypes.Mercenary:
                    GetRandomMercenary();
                    break;

                case MissionElementTypes.Weapon:
                    GetRandomWeaponType();
                    break;

                case MissionElementTypes.SideEvent:
                    GetRandomSideEvent();
                    break;
            }

            GetExplainElementsByMissionType(elements[i]);
        }
    }
    /*void GetExplainElementsByMissionType(MissionElementTypes elementType, Dictionary<ulong, Mercenary> MerceNaries)
    {
        switch (elementType)
        {
            case MissionElementTypes.Area:
                missionExplainElement.Add(randomMissionElements[MissionElementTypes.Domain]);
                missionExplainElement.Add((randomMissionElements[MissionElementTypes.Area].ToInt() - (randomMissionElements[MissionElementTypes.Domain].ToInt() - 1) * 100).ToString());
                break;

            case MissionElementTypes.MissionCount:
                missionExplainElement.Add(randomMissionElements[MissionElementTypes.MissionCount]);
                break;

            case MissionElementTypes.Monster:
                MonsterInformation.MonsterType monsterType = (MonsterInformation.MonsterType)randomMissionElements[MissionElementTypes.MonsterType].ToInt();
                string typeId = LocaleIdByMonsterType[monsterType];
                missionExplainElement.Add(Locale.GetText(typeId, "main"));
                missionExplainElement.Add(Locale.GetText(randomMissionElements[MissionElementTypes.Monster], "main"));
                break;

            case MissionElementTypes.Mercenary:
                ulong mercenaryId = randomMissionElements[MissionElementTypes.Mercenary].ToUnsignedLong();
                missionExplainElement.Add(MerceNaries[mercenaryId].Position.ToString());
                missionExplainElement.Add(Locale.GetText(MerceNaries[mercenaryId].NameId, "main"));
                break;

            case MissionElementTypes.Weapon:
                DefenceWeapon.WeaponTypes weaponType = (DefenceWeapon.WeaponTypes)randomMissionElements[MissionElementTypes.Weapon].ToInt();
                missionExplainElement.Add(weaponType.ToString());
                break;

            case MissionElementTypes.SideEvent:
                break;

            default: break;
        }
    }*/
    void GetExplainElementsByMissionType(MissionElementTypes elementType)
    {
        switch (elementType)
        {
            case MissionElementTypes.Area:
                missionExplainElement.Add(randomMissionElements[MissionElementTypes.Domain]);
                missionExplainElement.Add((int.Parse(randomMissionElements[MissionElementTypes.Area]) - (int.Parse(randomMissionElements[MissionElementTypes.Domain]) - 1) * 100).ToString());
                break;

            case MissionElementTypes.MissionCount:
                missionExplainElement.Add(randomMissionElements[MissionElementTypes.MissionCount]);
                break;

            case MissionElementTypes.Monster:
                MonsterInfo.MonsterType monsterType = (MonsterInfo.MonsterType)int.Parse(randomMissionElements[MissionElementTypes.MonsterType]);
                string typeId = TempDatas.MonsterTypeLocale[monsterType];
                missionExplainElement.Add(typeId);
                missionExplainElement.Add(TempDatas.TempMonsterInfos[ulong.Parse(randomMissionElements[MissionElementTypes.Monster])].monsterName);
                break;

            case MissionElementTypes.Mercenary:
                ulong mercenaryId = ulong.Parse(randomMissionElements[MissionElementTypes.Mercenary]);
                missionExplainElement.Add(TempDatas.TempMercenary[mercenaryId].Position.ToString());
                missionExplainElement.Add(TempDatas.TempMercenary[mercenaryId].mercenaryName);
                break;

            case MissionElementTypes.Weapon:
                TempDatas.WeaponTypes weaponType = (TempDatas.WeaponTypes)int.Parse(randomMissionElements[MissionElementTypes.Weapon]);
                missionExplainElement.Add(weaponType.ToString());
                break;

            case MissionElementTypes.SideEvent:
                break;

            default: break;
        }
    }

    // 실제 인게임 적용본
    /*OccupationAreaInfo GetRandomArea(Dictionary<int, OccupationAreaInfo> OccupationAreaInfoByAreaId)
    {
        List<OccupationAreaInfo> tempOccupationAreaInfo = new List<OccupationAreaInfo>();

        foreach (var occupationArea in OccupationAreaInfoByAreaId.Values)
        {
            if (occupationArea.AreaId == 1)
            {
                tempOccupationAreaInfo.Add(occupationArea);
                continue;
            }

            if (!User.Instance.UserAreaInfoData.ContainsKey(occupationArea.AreaId))
                continue;

            var userAreaInfo = User.Instance.UserAreaInfoData[occupationArea.AreaId] as UserOccupationAreaInfo;
            if (userAreaInfo.State != UserOccupationAreaInfo.AreaState.Locked)
            {
                if (Type == MissionType.DestroyBuilding)
                {
                    var settingsText = Resources.Load<TextAsset>(string.Format("JsonData/BuildingData"));
                    var settingsJson = new JSONObject(settingsText.text);

                    bool isBuildingDataInJson = false;

                    for (int i = 0; i < settingsJson.Count; ++i)
                    {
                        int buildingId = ((int)(settingsJson[i].GetField("Id").ToString().ToUnsignedLong() / 10000)) % 1000 - 100;

                        if (buildingId == occupationArea.AreaId)
                        {
                            isBuildingDataInJson = true;
                            break;
                        }
                    }

                    if (!isBuildingDataInJson)
                        continue;

                    GameObject building = Resources.Load<GameObject>($"Buildings/{occupationArea.AreaId}");

                    bool childActive = false;

                    for (int i = 0; i < building.transform.childCount; ++i)
                    {
                        if (building.transform.GetChild(i).gameObject.activeSelf)
                        {
                            childActive = true;
                            break;
                        }
                    }

                    if (!childActive)
                        continue;
                }

                tempOccupationAreaInfo.Add(occupationArea);
            }
        }

        var randomMissionArea = tempOccupationAreaInfo[UnityEngine.Random.Range(0, tempOccupationAreaInfo.Count)];

        randomMissionElements.Add(MissionElementTypes.Area, randomMissionArea.AreaId.ToString());
        randomMissionElements.Add(MissionElementTypes.Domain, (randomMissionArea.AreaId / 100 + 1).ToString());

        return randomMissionArea;
    }*/

    // Mission Project용 수정본
    AreaInfo GetRandomArea()
    {
        List<AreaInfo> tempOccupationAreaInfo = new List<AreaInfo>();

        foreach(var areaInfo in TempDatas.TempArea)
        {
            tempOccupationAreaInfo.Add(areaInfo.Value);
        }

        var randomMissionArea = tempOccupationAreaInfo[UnityEngine.Random.Range(0, tempOccupationAreaInfo.Count)];

        randomMissionElements.Add(MissionElementTypes.Area, randomMissionArea.areaId.ToString());
        randomMissionElements.Add(MissionElementTypes.Domain, (randomMissionArea.areaId / 100 + 1).ToString());

        return randomMissionArea;
    }

    /*void GetRandomMonster(OccupationAreaInfo randomMissionArea)
    {
        var settingsText = Resources.Load<TextAsset>(string.Format("JsonData/Settings-DefenceMode"));
        var settingsJson = new JSONObject(settingsText.text);

        var MonsterPresetsJson = settingsJson.GetField("MonsterPresets");
        ulong randomMonsterID = 0;

        int randomMonsterType = UnityEngine.Random.Range((int)MonsterInformation.MonsterType.Common, (int)MonsterInformation.MonsterType.Boss + 1);

        randomMissionElements.Add(MissionElementTypes.MonsterType, randomMonsterType.ToString());

        List<ulong> monsterList = new List<ulong>();

        for (int i = 0; i < MonsterPresetsJson.Count; i++)
        {
            if (MonsterPresetsJson[i].GetField("Area").ToString().ToInt() == randomMissionArea.AreaId &&
                MonsterPresetsJson[i].GetField("MonsterType").ToString().ToInt() == randomMonsterType)
            {
                var valueJson = MonsterPresetsJson[i].GetField("Monsters").list;

                for (int monstersIdx = 0; monstersIdx < valueJson.Count; monstersIdx++)
                {
                    monsterList.Add(ulong.Parse(valueJson[monstersIdx].str));
                }

                randomMonsterID = monsterList[UnityEngine.Random.Range(0, monsterList.Count())];
                break;
            }
        }

        var monsterInfoText = Resources.Load<TextAsset>("JsonData/Monster");
        var monsterInfoJson = new JSONObject(monsterInfoText.text);

        for (int i = 0; i < monsterInfoJson.Count; i++)
        {
            if (monsterInfoJson[i].GetField("Id").ToString().ToUnsignedLong() == randomMonsterID)
            {
                string randomMonsterNameID = monsterInfoJson[i].GetField("NameId").str;

                if (string.IsNullOrEmpty(randomMonsterNameID))
                {
                    Debug.LogWarning($"{randomMonsterID} Monster Name is None in Locale");

                    monsterList.Remove(randomMonsterID);
                    randomMonsterID = monsterList[UnityEngine.Random.Range(0, monsterList.Count())];

                    i = 0;
                }

                else
                {
                    randomMissionElements.Add(MissionElementTypes.Monster, randomMonsterNameID);
                    break;
                }
            }
        }
    }*/
    void GetRandomMonster(AreaInfo randomMissionArea)
    {
        int randomMonsterType = UnityEngine.Random.Range((int)MonsterInfo.MonsterType.Common, (int)MonsterInfo.MonsterType.Boss + 1);

        randomMissionElements.Add(MissionElementTypes.MonsterType, randomMonsterType.ToString());

        List<MonsterInfo> monsterList = new List<MonsterInfo>();

        for (int i = 0; i < randomMissionArea.appearMonsterIds.Length; ++i)
        {
            if ((int)(TempDatas.TempMonsterInfos[ulong.Parse(randomMissionArea.appearMonsterIds[i])].type) == randomMonsterType)
            {
                monsterList.Add(TempDatas.TempMonsterInfos[ulong.Parse(randomMissionArea.appearMonsterIds[i])]);
            }
        }

        ulong randomMonterName = monsterList[UnityEngine.Random.Range(0, monsterList.Count())].monsterId;

        randomMissionElements.Add(MissionElementTypes.Monster, randomMonterName.ToString());
    }

    /*void GetRandomMissionClearCount()
    {
        double missionCount = 0;

        if (Type == MissionType.SpecificMonsterKill)
            missionCount = Mathf.Clamp((int)MathF.Pow(10, ((int)MonsterInformation.MonsterType.Boss - randomMissionElements[MissionElementTypes.MonsterType].ToInt())), 0, 300) * (int)LifeCycleType;

        else if (Type == MissionType.ClearWithSpecificMercenary)
            missionCount = MathF.Pow(3, (int)LifeCycleType);

        else if (Type == MissionType.DestroyBuilding)
            missionCount = MathF.Pow(5, (int)LifeCycleType);

        else if (Type == MissionType.SideEventSuccess)
            missionCount = MathF.Pow(5, (int)LifeCycleType);

        else if (Type == MissionType.MonsterKillBySpecificWeapon)
            missionCount = MathF.Pow(5, (int)LifeCycleType) * 100;

        CompletionCount = missionCount;
        randomMissionElements.Add(MissionElementTypes.MissionCount, missionCount.ToString());
    }*/

    void GetRandomMissionClearCount()
    {
        double missionCount = 0;

        if (Type == MissionType.SpecificMonsterKill)
            missionCount = Mathf.Clamp((int)MathF.Pow(10, ((int)MonsterInfo.MonsterType.Boss - int.Parse(randomMissionElements[MissionElementTypes.MonsterType]))), 0, 300) * (int)LifeCycleType;

        else if (Type == MissionType.ClearWithSpecificMercenary)
            missionCount = MathF.Pow(3, (int)LifeCycleType);

        else if (Type == MissionType.DestroyBuilding)
            missionCount = MathF.Pow(5, (int)LifeCycleType);

        else if (Type == MissionType.SideEventSuccess)
            missionCount = MathF.Pow(5, (int)LifeCycleType);

        else if (Type == MissionType.MonsterKillBySpecificWeapon)
            missionCount = MathF.Pow(5, (int)LifeCycleType) * 100;

        CompletionCount = missionCount;
        randomMissionElements.Add(MissionElementTypes.MissionCount, missionCount.ToString());
    }

    void GetRandomMercenary()
    {
        List<ulong> mercenaryIds = new List<ulong>();

        foreach (var userMercenary in TempDatas.TempMercenary)
        {
            ulong id = userMercenary.Key;
            mercenaryIds.Add(id);
        }

        ulong randomMercenaryId = mercenaryIds[UnityEngine.Random.Range(0, mercenaryIds.Count)];

        randomMissionElements.Add(MissionElementTypes.Mercenary, randomMercenaryId.ToString());
    }

    void GetRandomSideEvent()
    {
        List<ulong> SideEventActorIdList = new List<ulong>();

        SideEventType randomSideEventType = (SideEventType)UnityEngine.Random.Range(1, (int)SideEventType.Count);

        switch (randomSideEventType)
        {
            case SideEventType.Animal:
                explainId = "0100052005";
                SideEventActorIdList.Add(110010);
                SideEventActorIdList.Add(110011);
                break;

            case SideEventType.Human:
                explainId = "0100052006";
                SideEventActorIdList.Add(110012);
                SideEventActorIdList.Add(110021);
                break;

            case SideEventType.Missile:
                explainId = "0100052008";
                SideEventActorIdList.Add(110018);
                SideEventActorIdList.Add(110019);
                SideEventActorIdList.Add(110020);
                break;

            default:
                break;
        }

        // randommission 인자 딕셔너리 저장 시 ','를 이용하므로 이를 제외한 다른 기호 이용
        string idList = string.Join('/', SideEventActorIdList);

        randomMissionElements.Add(MissionElementTypes.SideEvent, idList);
    }

    void GetRandomWeaponType()
    {
        int randomWeaponType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(TempDatas.WeaponTypes)).Length);

        randomMissionElements.Add(MissionElementTypes.Weapon, randomWeaponType.ToString());
    }
}