using System.Collections.Generic;

[System.Serializable]
public struct UserMission
{
    public ulong id;
    public double count;
    public bool isComplete;
    public bool isReward;
    public Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements;

    public UserMission(ulong id, double count, bool isComplete, bool isReward, Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements)
    {
        this.id = id;
        this.count = count;
        this.isComplete = isComplete;
        this.isReward = isReward;
        this.randomMissionElements = randomMissionElements;
    }
}