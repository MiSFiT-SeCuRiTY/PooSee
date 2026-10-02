namespace PooSee.Services;

public interface ISystemResourceService
{
    int GetRecommendedSessionCount();
    ulong GetTotalPhysicalMemoryBytes();
    int GetLogicalProcessorCount();
}