[System.Serializable]
public struct UserMission
{
    public ulong id;
    public double count;
    public bool isComplete;
    public bool isReward;
    public MissionObjective[] objectives;

    public UserMission(ulong id, double count, bool isComplete, bool isReward, MissionObjective[] objectives)
    {
        this.id = id;
        this.count = count;
        this.isComplete = isComplete;
        this.isReward = isReward;
        this.objectives = objectives;
    }
}