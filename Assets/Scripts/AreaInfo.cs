using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInfo
{
    public int areaId;
    public string[] appearMonsterIds;

    public AreaInfo(int areaId, string[] appearMonsters)
    {
        this.areaId = areaId;
        this.appearMonsterIds = appearMonsters;
    }
}
