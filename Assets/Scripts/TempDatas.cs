using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDatas
{
    public static Dictionary<string, string> MissionNameDict = new Dictionary<string, string>()
    {
        {"0100050008", "일일일반미션" },
        {"0100050014", "주간일반미션" },
        {"0100050022", "월간일반미션" },
        {"0100050127", "일일랜덤미션 {0}" },
        {"0100050128", "주간랜덤미션 {0}" },
        {"0100050129", "월간랜덤미션 {0}" },
    };

    public static Dictionary<string, string> MissionExplainDic = new Dictionary<string, string>()
    {
        {"0100051008", "일반미션설명" },
        {"0100051014", "일반미션설명" },
        {"0100051022", "일반미션설명" },

        {"0100052001", "{0} domain {1} Area에서 {2} 등급 {3}을(를) {4}마리 처치" },
        {"0100052002", "{0} Type 용병 {1}과(와) 함께 {2} domain {3} Area을(를) {4}회 클리어" },
        {"0100052003", "{0} domain {1} Area에서 시설물 {2}회 파괴" },
        {"0100052004", "{0} domain {1} Area에서 드왈키를 {2}회 처치" },
        {"0100052005", "{0} domain {1} Area에서 공격받는 강아지/고양이를 {2}회 구출" },
        {"0100052006", "{0} domain {1} Area에서 공격받는 여성/박사를 {2}회 구출" },
        {"0100052007", "{0} domain {1} Area에서 낙하물을 {2}회 파괴" },
        {"0100052008", "{0} domain {1} Area에서 미사일을 {2}회 파괴" },
        {"0100052009", "{0} 총기를 사용해 {1} domain {2} Area에서 몬스터 {3}마리 처치" }
    };

    public static Dictionary<int, AreaInfo> TempArea = new Dictionary<int, AreaInfo>()
    {
        {1, new AreaInfo(1, new string[] { "0001010001", "0001010002", "0001020001", "0001020002", "0001030001", "0001040001" }) },
        {2, new AreaInfo(2, new string[] { "0001010002", "0001010003", "0001020002", "0001020003", "0001030002", "0001040002" }) },
        {3, new AreaInfo(3, new string[] { "0001010003", "0001010004", "0001020003", "0001020004", "0001030001", "0001040001" }) },
        {4, new AreaInfo(4, new string[] { "0001010004", "0001010005", "0001020004", "0001020001", "0001030002", "0001040002" }) },
        {5, new AreaInfo(5, new string[] { "0001010001", "0001010005", "0001020004", "0001020002", "0001030001", "0001040001" }) },

        {101, new AreaInfo(101, new string[] { "0001010001", "0001010002", "0001020001", "0001020002", "0001030001", "0001040001" }) },
        {102, new AreaInfo(102, new string[] { "0001010002", "0001010003", "0001020002", "0001020003", "0001030002", "0001040002" }) },
        {103, new AreaInfo(103, new string[] { "0001010003", "0001010004", "0001020003", "0001020004", "0001030001", "0001040001" }) },
        {104, new AreaInfo(104, new string[] { "0001010004", "0001010005", "0001020004", "0001020001", "0001030002", "0001040002" }) },
        {105, new AreaInfo(105, new string[] { "0001010001", "0001010005", "0001020004", "0001020002", "0001030001", "0001040001" }) },

        {201, new AreaInfo(201, new string[] { "0001010001", "0001010002", "0001020001", "0001020002", "0001030001", "0001040001" }) },
        {202, new AreaInfo(202, new string[] { "0001010002", "0001010003", "0001020002", "0001020003", "0001030002", "0001040002" }) },
        {203, new AreaInfo(203, new string[] { "0001010003", "0001010004", "0001020003", "0001020004", "0001030001", "0001040001" }) },
        {204, new AreaInfo(204, new string[] { "0001010004", "0001010005", "0001020004", "0001020001", "0001030002", "0001040002" }) },
        {205, new AreaInfo(205, new string[] { "0001010001", "0001010005", "0001020004", "0001020002", "0001030001", "0001040001" }) },
    };

    public static Dictionary<ulong, MercenaryInfo> TempMercenary = new Dictionary<ulong, MercenaryInfo>()
    {
        {1900000000,  new MercenaryInfo(1900000000, "용병1", MercenaryInfo.PositionType.D)},
        {1900000001,  new MercenaryInfo(1900000001, "용병2", MercenaryInfo.PositionType.A)},
        {1900000002,  new MercenaryInfo(1900000002, "용병3", MercenaryInfo.PositionType.A)},
        {1900000003,  new MercenaryInfo(1900000003, "용병4", MercenaryInfo.PositionType.B)},
        {1900000004,  new MercenaryInfo(1900000004, "용병5", MercenaryInfo.PositionType.B)},
        {1900000005,  new MercenaryInfo(1900000005, "용병6", MercenaryInfo.PositionType.C)},
        {1900000006,  new MercenaryInfo(1900000006, "용병7", MercenaryInfo.PositionType.C)},
        {1900000007,  new MercenaryInfo(1900000007, "용병8", MercenaryInfo.PositionType.D)},
        {1900000008,  new MercenaryInfo(1900000008, "용병9", MercenaryInfo.PositionType.D)},
        {1900000009,  new MercenaryInfo(1900000009, "용병10", MercenaryInfo.PositionType.A)},
    };

    public static Dictionary<ulong, MonsterInfo> TempMonsterInfos = new Dictionary<ulong, MonsterInfo>()
    {
        {0001010001, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "일반1") },
        {0001010002, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "일반2") },
        {0001010003, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "일반3") },
        {0001010004, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "일반4") },
        {0001010005, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "일반5") },

        {0001020001, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "엘리트1") },
        {0001020002, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "엘리트2") },
        {0001020003, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "엘리트3") },
        {0001020004, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "엘리트4") },

        {0001030001, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "네임드1") },
        {0001030002, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "네임드2") },
        {0001030003, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "네임드3") },

        {0001040001, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "보스1") },
        {0001040002, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "보스2") },
        {0001040003, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "보스3") },
    };

    public enum WeaponTypes { Weapon1, Weapon2, Weapon3 }

    public static Dictionary<MonsterInfo.MonsterType, string> MonsterTypeLocale = new Dictionary<MonsterInfo.MonsterType, string>()
    {
        { MonsterInfo.MonsterType.Common, "일반" },
        { MonsterInfo.MonsterType.Elite, "엘리트" },
        { MonsterInfo.MonsterType.Named, "네임드" },
        { MonsterInfo.MonsterType.Boss, "보스" }
    };

    public static Dictionary<string, string> LocaleTexts = new Dictionary<string, string>()
    {
        {"0101000009", "Level Up" },
        {"0101000010", "Buy" },
        {"0105000126", "바로가기" },
        {"0105000127", "보상받기" },
        {"0105000128", "미션완료" },
    };
}
