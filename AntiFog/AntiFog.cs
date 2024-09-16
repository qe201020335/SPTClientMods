using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BSG.CameraEffects;
using Comfort.Common;
using HarmonyLib;
using EFT;
using UnityEngine.Serialization;

namespace AntiFog;

public class AntiFog : MonoBehaviour
{
    private static GameObject FPSCamera;
    private static Camera FPSCameraCamera;

    // private CustomGlobalFog FPSCameraCustomGlobalFog;
    public static NightVision FPSCameraNightVision;
    private string _scene;
    
    private MBOIT_Scattering _mboitScattering;
    private static float defaultMBOITZeroLevel;
    private static float defaultLevelSettingsZeroLevel;

    private Tuple<bool, float> defaultFPSCameraCustomGlobalFog;

    public bool NVG = false;

    public bool IsActive { get; private set; } = false;

    private LevelSettings _levelSettings;
    private float _zeroLevelOffset;

    private void Awake()
    {
        Plugin.Instance.Config.SettingChanged += SettingsUpdated;
    }

    public void Update()
    {
        if (Input.GetKeyDown(Plugin.GraphicsToggle.Value.MainKey) && (!Input.GetKey(KeyCode.LeftShift)) && FPSCamera != null)
        {
            if (IsActive)
            {
                IsActive = false;
                ResetGraphics();
            }
            else
            {
                IsActive = true;
                UpdateComponentValues();
            }
        }
    }

    public void InitComponents(GameObject fpscamera, PrismEffects prismeffects)
    {
        if (FPSCamera != null) return;

        FPSCamera = fpscamera;

        if (fpscamera == null) return;

        if (FPSCameraCamera == null)
        {
            FPSCameraCamera = FPSCamera.GetComponent<Camera>();
        }

        _scene = SceneManager.GetActiveScene().name;

        // FPSCameraCustomGlobalFog = FPSCamera.GetComponent<CustomGlobalFog>();
        
        _mboitScattering = FPSCamera.GetComponent<MBOIT_Scattering>();
        defaultMBOITZeroLevel = _mboitScattering.ZeroLevel;
        
        // if (FPSCameraCustomGlobalFog != null)
        // {
        //     defaultFPSCameraCustomGlobalFog = new Tuple<bool, float>(FPSCameraCustomGlobalFog.enabled, FPSCameraCustomGlobalFog.FuncStart);
        // }
        

        _levelSettings = Singleton<LevelSettings>.Instance;
        defaultLevelSettingsZeroLevel = _levelSettings.ZeroLevel;
        
        FPSCameraNightVision = FPSCamera.GetComponent<NightVision>();
        if (FPSCameraNightVision != null)
        {
            NVG = FPSCameraNightVision.On;
        }

        UpdateSettings();
        
        IsActive = true;
        enabled = true;
        UpdateComponentValues();
    }

    private void UpdateSettings()
    {
        _zeroLevelOffset = 0;
        switch(_scene)
        {
            case "City_Scripts":
                _zeroLevelOffset = Plugin.StreetsFogLevel.Value;
                break;
            case "Laboratory_Scripts":
                break;
            case "custom_Light":
                _zeroLevelOffset = Plugin.CustomsFogLevel.Value;
                break;
            case "Lighthouse_Abadonned_pier":
                _zeroLevelOffset = Plugin.LighthouseFogLevel.Value;
                break;
            case "Shopping_Mall_Terrain":
                _zeroLevelOffset = Plugin.InterchangeFogLevel.Value;
                break;
            case "woods_combined":
                _zeroLevelOffset = Plugin.WoodsFogLevel.Value;
                break;
            case "Reserve_Base_DesignStuff":
                _zeroLevelOffset = Plugin.ReserveFogLevel.Value;
                break;
            case "shoreline_scripts":
                _zeroLevelOffset = Plugin.ShorelineFogLevel.Value;
                break;
            default:
                Plugin.Log.LogWarning($"Unknown map <{_scene}>, using everywhere else fog level.");
                _zeroLevelOffset = Plugin.EverywhereElseFogLevel.Value;
                break;
        }
    }
    

    public void UpdateComponentValues()
    {
        if (_levelSettings != null) _levelSettings.ZeroLevel = defaultLevelSettingsZeroLevel + _zeroLevelOffset;
        
        if (_mboitScattering != null) _mboitScattering.ZeroLevel = defaultMBOITZeroLevel + _zeroLevelOffset;

        // if (FPSCameraCustomGlobalFog != null)
        // {
        //     FPSCameraCustomGlobalFog.enabled = !(_scene == "Factory_Day" || _scene == "Factory_Night" || _scene == "default") && defaultFPSCameraCustomGlobalFog.Item1;
        //     FPSCameraCustomGlobalFog.FuncStart = NVG ? Plugin.NVGCustomGlobalFogIntensity.Value : Plugin.CustomGlobalFogIntensity.Value;
        //     FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Normal;
        // }
    }

    private void ResetGraphics()
    {
        if (_levelSettings != null)
        {
            _levelSettings.ZeroLevel = defaultLevelSettingsZeroLevel;
        }

        if (_mboitScattering != null)
        {
            _mboitScattering.ZeroLevel = defaultMBOITZeroLevel;
        }

        // if (FPSCameraCustomGlobalFog != null)
        // {
        //     FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog.Item1;
        //     FPSCameraCustomGlobalFog.FuncStart = defaultFPSCameraCustomGlobalFog.Item2;
        // }
    }

    private void SettingsUpdated(object sender, EventArgs e)
    {
        UpdateSettings();
        if (IsActive)
        {
            UpdateComponentValues();
        }
    }
}