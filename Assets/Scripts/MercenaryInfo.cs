using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryInfo
{
    public ulong mercenaryId;
    public string mercenaryName;
    public enum PositionType
    {
        A, B, C, D, E
    }

    public PositionType Position;

    public MercenaryInfo(ulong mercenaryId, string mercenaryName, PositionType position)
    {
        this.mercenaryId = mercenaryId;
        this.mercenaryName = mercenaryName;
        Position = position;
    }
}
