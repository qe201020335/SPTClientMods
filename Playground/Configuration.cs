using BepInEx.Configuration;

namespace Playground;

// Should make a generator for this 

internal class Configuration
{
    internal static Configuration Instance { get; private set; } = null!;
    
    internal static void Create(ConfigFile configFile)
    {
        Instance = new Configuration(configFile);
    }
    
    internal ConfigFile ConfigFile { get; }

    private readonly ConfigEntry<bool> _enableDebugViews;
    internal bool EnableDebugViews
    {
        get => _enableDebugViews.Value;
        set => _enableDebugViews.Value = value;
    }
    
    private readonly ConfigEntry<bool> _enableStaminaDebug;
    internal bool EnableStaminaDebug
    {
        get => _enableStaminaDebug.Value;
        set => _enableStaminaDebug.Value = value;
    }
    
    private readonly ConfigEntry<bool> _enableMalfunctionDebug;
    internal bool EnableMalfunctionDebug
    {
        get => _enableMalfunctionDebug.Value;
        set => _enableMalfunctionDebug.Value = value;
    }
    
    private readonly ConfigEntry<bool> _enableTraderServicesDebug;
    internal bool EnableTraderServicesDebug
    {
        get => _enableTraderServicesDebug.Value;
        set => _enableTraderServicesDebug.Value = value;
    }
    
    //
    // private readonly ConfigEntry<bool> _enableVaultingDebugTool;
    // internal bool EnableVaultingDebugTool
    // {
    //     get => _enableVaultingDebugTool.Value;
    //     set => _enableVaultingDebugTool.Value = value;
    // }
    
    private readonly ConfigEntry<bool> _enableWeaponAnimEventsQueueDebug;
    internal bool EnableWeaponAnimEventsQueueDebug
    {
        get => _enableWeaponAnimEventsQueueDebug.Value;
        set => _enableWeaponAnimEventsQueueDebug.Value = value;
    }
    
    private readonly ConfigEntry<bool> _enableWeaponDurabilityDebug;
    internal bool EnableWeaponDurabilityDebug
    {
        get => _enableWeaponDurabilityDebug.Value;
        set => _enableWeaponDurabilityDebug.Value = value;
    }
    
    private readonly ConfigEntry<bool> _enableWeaponOverheatDebug;
    internal bool EnableWeaponOverheatDebug
    {
        get => _enableWeaponOverheatDebug.Value;
        set => _enableWeaponOverheatDebug.Value = value;
    }
    
    private Configuration(ConfigFile configFile)
    {
        ConfigFile = configFile;
        
        _enableDebugViews = configFile.Bind("Debug", "EnableDebugViews", false, "Enable debug views.");
        _enableStaminaDebug = configFile.Bind("Debug", "EnableStaminaDebug", false, "Enable stamina debug.");
        _enableMalfunctionDebug = configFile.Bind("Debug", "EnableMalfunctionDebug", false, "Enable malfunction debug.");
        _enableTraderServicesDebug = configFile.Bind("Debug", "EnableTraderServicesDebug", false, "Enable trader services debug.");
        // _enableVaultingDebugTool = configFile.Bind("Debug", "EnableVaultingDebugTool", false, "Enable vaulting debug tool.");
        _enableWeaponAnimEventsQueueDebug = configFile.Bind("Debug", "EnableWeaponAnimEventsQueueDebug", false, "Enable weapon anim events queue debug.");
        _enableWeaponDurabilityDebug = configFile.Bind("Debug", "EnableWeaponDurabilityDebug", false, "Enable weapon durability debug.");
        _enableWeaponOverheatDebug = configFile.Bind("Debug", "EnableWeaponOverheatDebug", false, "Enable weapon overheat debug.");
    }
    
    
    
}