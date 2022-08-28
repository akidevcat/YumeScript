using YumeScript.Runtime;

namespace YumeScript.Extensions;

public static class RuntimeConfigurationExtensions
{
    public static RuntimeInstance CreateRuntime(this RuntimeConfiguration configuration)
    {
        // ToDo pre-checks
        return new RuntimeInstance(configuration);
    }
}