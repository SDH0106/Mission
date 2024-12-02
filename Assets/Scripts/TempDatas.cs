using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDatas
{
    public static Dictionary<string, string> MissionNameDict = new Dictionary<string, string>()
    {
        {"0100050008", "�����Ϲݹ̼�" },
        {"0100050014", "�ְ��Ϲݹ̼�" },
        {"0100050022", "�����Ϲݹ̼�" },
        {"0100050127", "���Ϸ����̼� {0}" },
        {"0100050128", "�ְ������̼� {0}" },
        {"0100050129", "���������̼� {0}" },
    };

    public static Dictionary<string, string> MissionExplainDic = new Dictionary<string, string>()
    {
        {"0100051008", "�Ϲݹ̼Ǽ���" },
        {"0100051014", "�Ϲݹ̼Ǽ���" },
        {"0100051022", "�Ϲݹ̼Ǽ���" },

        {"0100052001", "{0} domain {1} Area���� {2} ��� {3}��(��) {4}���� óġ" },
        {"0100052002", "{0} Type �뺴 {1}��(��) �Բ� {2} domain {3} Area��(��) {4}ȸ Ŭ����" },
        {"0100052003", "{0} domain {1} Area���� �ü��� {2}ȸ �ı�" },
        {"0100052004", "{0} domain {1} Area���� ���Ű�� {2}ȸ óġ" },
        {"0100052005", "{0} domain {1} Area���� ���ݹ޴� ������/����̸� {2}ȸ ����" },
        {"0100052006", "{0} domain {1} Area���� ���ݹ޴� ����/�ڻ縦 {2}ȸ ����" },
        {"0100052007", "{0} domain {1} Area���� ���Ϲ��� {2}ȸ �ı�" },
        {"0100052008", "{0} domain {1} Area���� �̻����� {2}ȸ �ı�" },
        {"0100052009", "{0} �ѱ⸦ ����� {1} domain {2} Area���� ���� {3}���� óġ" }
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
        {1900000000,  new MercenaryInfo(1900000000, "�뺴1", MercenaryInfo.PositionType.D)},
        {1900000001,  new MercenaryInfo(1900000001, "�뺴2", MercenaryInfo.PositionType.A)},
        {1900000002,  new MercenaryInfo(1900000002, "�뺴3", MercenaryInfo.PositionType.A)},
        {1900000003,  new MercenaryInfo(1900000003, "�뺴4", MercenaryInfo.PositionType.B)},
        {1900000004,  new MercenaryInfo(1900000004, "�뺴5", MercenaryInfo.PositionType.B)},
        {1900000005,  new MercenaryInfo(1900000005, "�뺴6", MercenaryInfo.PositionType.C)},
        {1900000006,  new MercenaryInfo(1900000006, "�뺴7", MercenaryInfo.PositionType.C)},
        {1900000007,  new MercenaryInfo(1900000007, "�뺴8", MercenaryInfo.PositionType.D)},
        {1900000008,  new MercenaryInfo(1900000008, "�뺴9", MercenaryInfo.PositionType.D)},
        {1900000009,  new MercenaryInfo(1900000009, "�뺴10", MercenaryInfo.PositionType.A)},
    };

    public static Dictionary<ulong, MonsterInfo> TempMonsterInfos = new Dictionary<ulong, MonsterInfo>()
    {
        {0001010001, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "�Ϲ�1") },
        {0001010002, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "�Ϲ�2") },
        {0001010003, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "�Ϲ�3") },
        {0001010004, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "�Ϲ�4") },
        {0001010005, new MonsterInfo(MonsterInfo.MonsterType.Common, 0001010001, "�Ϲ�5") },

        {0001020001, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "����Ʈ1") },
        {0001020002, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "����Ʈ2") },
        {0001020003, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "����Ʈ3") },
        {0001020004, new MonsterInfo(MonsterInfo.MonsterType.Elite, 0001010001, "����Ʈ4") },

        {0001030001, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "���ӵ�1") },
        {0001030002, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "���ӵ�2") },
        {0001030003, new MonsterInfo(MonsterInfo.MonsterType.Named, 0001010001, "���ӵ�3") },

        {0001040001, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "����1") },
        {0001040002, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "����2") },
        {0001040003, new MonsterInfo(MonsterInfo.MonsterType.Boss, 0001010001, "����3") },
    };

    public enum WeaponTypes { Weapon1, Weapon2, Weapon3 }

    public static Dictionary<MonsterInfo.MonsterType, string> MonsterTypeLocale = new Dictionary<MonsterInfo.MonsterType, string>()
    {
        { MonsterInfo.MonsterType.Common, "�Ϲ�" },
        { MonsterInfo.MonsterType.Elite, "����Ʈ" },
        { MonsterInfo.MonsterType.Named, "���ӵ�" },
        { MonsterInfo.MonsterType.Boss, "����" }
    };

    public static Dictionary<string, string> LocaleTexts = new Dictionary<string, string>()
    {
        {"0101000009", "Level Up" },
        {"0101000010", "Buy" },
        {"0105000126", "�ٷΰ���" },
        {"0105000127", "����ޱ�" },
        {"0105000128", "�̼ǿϷ�" },
    };
}
