[System.Serializable]
public struct MissionObjective
{
    public string condition1;
    public string condition2;
    public double objectiveCount;
    public int groupId;

    public MissionObjective(string category, string categoryValue, double objectiveCount = 0, int groupId = 0)
    {
        this.condition1 = category;
        this.condition2 = categoryValue;
        this.objectiveCount = objectiveCount;
        this.groupId = groupId;
    }
}