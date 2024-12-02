using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    public enum MonsterType : int { Common = 1, Elite, Named, Boss, };
    public MonsterType type;
    public ulong monsterId;
    public string monsterName;

    public MonsterInfo(MonsterType type, ulong monsterId, string monsterName)
    {
        this.type = type;
        this.monsterId = monsterId;
        this.monsterName = monsterName;
    }
}
