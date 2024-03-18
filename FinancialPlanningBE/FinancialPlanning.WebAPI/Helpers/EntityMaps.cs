namespace FinancialPlanning.WebAPI.Helpers;

public static class EntityMaps
{
    public static readonly Dictionary<int, string> StatusMap = new Dictionary<int, string>
    {
        { 0, "New" },
        { 1, "In Progress" },
        { 2, "Waiting For Approval" },
        { 3, "Approved" },
        { 4, "Closed" }
    };
}