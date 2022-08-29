using YumeScript.External;

namespace YumeScript.Configuration;

public class RuntimeConfiguration
{
    private IResourceManager? _resourceManager;
    private ICallbackEngine? _callbackEngine;
    
    public RuntimeConfiguration(Action<RuntimeConfiguration> cfg)
    {
        cfg(this);
    }

    public void UseResourceManager(IResourceManager resourceManager) => _resourceManager = resourceManager;
    
    public void UseCallbackEngine(ICallbackEngine callbackEngine)
    {
        _callbackEngine = callbackEngine;
    }
}