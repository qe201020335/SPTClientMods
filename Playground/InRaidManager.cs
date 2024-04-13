using System;
using BepInEx.Logging;
using EFT;
using EFT.Vaulting.Debug;
using UnityEngine;

namespace Playground;

public class InRaidManager: MonoBehaviour
{
    private readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("InRaidManager");
    private Configuration _config => Configuration.Instance;
    
    private GameWorld _gameWorld;

    private GameObject _debugObject;
    private StaminaDebug _staminaDebug;
    private MalfunctionDebug _malfunctionDebug;
    private TraderServicesDebug _traderServicesDebug;
    // private VaultingDebugTool _vaultingDebugTool;
    private WeaponAnimEventsQueueDebug _weaponAnimEventsQueueDebug;
    private WeaponDurabilityDebug _weaponDurabilityDebug;
    private WeaponOverheatDebug _weaponOverheatDebug;
    

    private void OnEnable()
    {
        Configuration.Instance.ConfigFile.SettingChanged += OnSettingsChanged;
    }
    
    private void OnDisable()
    {
        Configuration.Instance.ConfigFile.SettingChanged -= OnSettingsChanged;
    }

    internal void Init(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
        
        if (_debugObject != null)
        {
            Destroy(_debugObject);
        }
        _debugObject = new GameObject("InRaidManagerDbg");

        UpdateDebugMenuState();
    }
    
    private void OnSettingsChanged(object sender, EventArgs e)
    {
        UpdateDebugMenuState();
    }

    private void UpdateDebugMenuState()
    {
        if (_gameWorld == null)
        {
            _logger.LogWarning("Not initialized yet");
            return;
        }
        
        _debugObject.SetActive(_config.EnableDebugViews);

        if (_staminaDebug == null)
        {
            _staminaDebug = _debugObject.AddComponent<StaminaDebug>();
            _staminaDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _staminaDebug.enabled = _config.EnableStaminaDebug;
        
        if (_malfunctionDebug == null)
        {
            _malfunctionDebug = _debugObject.AddComponent<MalfunctionDebug>();
            _malfunctionDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _malfunctionDebug.enabled = _config.EnableMalfunctionDebug;
        
        if (_traderServicesDebug == null)
        {
            _traderServicesDebug = _debugObject.AddComponent<TraderServicesDebug>();
            _traderServicesDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _traderServicesDebug.enabled = _config.EnableTraderServicesDebug;
        
        if (_weaponAnimEventsQueueDebug == null)
        {
            _weaponAnimEventsQueueDebug = _debugObject.AddComponent<WeaponAnimEventsQueueDebug>();
            _weaponAnimEventsQueueDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _weaponAnimEventsQueueDebug.enabled = _config.EnableWeaponAnimEventsQueueDebug;
        
        if (_weaponDurabilityDebug == null)
        {
            _weaponDurabilityDebug = _debugObject.AddComponent<WeaponDurabilityDebug>();
            _weaponDurabilityDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _weaponDurabilityDebug.enabled = _config.EnableWeaponDurabilityDebug;
        
        if (_weaponOverheatDebug == null)
        {
            _weaponOverheatDebug = _debugObject.AddComponent<WeaponOverheatDebug>();
            _weaponOverheatDebug.SetDebugObject(_gameWorld.MainPlayer);
        }
        
        _weaponOverheatDebug.enabled = _config.EnableWeaponOverheatDebug;
        
        
        // _vaultingDebugTool = _debugObject.AddComponent<VaultingDebugTool>();
        // _vaultingDebugTool.Initialize(gameWorld.MainPlayer);
        // _vaultingDebugTool.enabled = _config.EnableVaultingDebugTool;
    }
}