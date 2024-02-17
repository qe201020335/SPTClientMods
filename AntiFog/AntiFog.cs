using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BSG.CameraEffects;
using HarmonyLib;
using EFT;

namespace AntiFog;

public class AntiFog : MonoBehaviour
{
    public static LocalPlayer localPlayer;
    private static GameObject FPSCamera;
    private static Camera FPSCameraCamera;

    private static object mBOIT_Scattering;
    private static CustomGlobalFog FPSCameraCustomGlobalFog;
    private static Behaviour FPSCameraGlobalFog;
    public static NightVision FPSCameraNightVision;
    public static float defaultNightVisionNoiseIntensity;
    public static string scene;
    private static float defaultMBOITZeroLevel;

    private static bool defaultFPSCameraCustomGlobalFog;
    private static bool defaultFPSCameraGlobalFog;


    private static Dictionary<string, string> sceneLevelSettings = new Dictionary<string, string>();

    public static bool NVG = false;

    public bool GraphicsMode = false;

    public void Start()
    {
        sceneLevelSettings.Add("City_Scripts", "---City_ levelsettings ---");
        sceneLevelSettings.Add("Laboratory_Scripts", "---Laboratory_levelsettings---");
        sceneLevelSettings.Add("custom_Light", "---Custom_levelsettings---");
        sceneLevelSettings.Add("Factory_Day", "---FactoryDay_levelsettings---");
        sceneLevelSettings.Add("Factory_Night", "---FactoryNight_levelsettings---");
        sceneLevelSettings.Add("Lighthouse_Abadonned_pier", "---Lighthouse_levelsettings---");
        sceneLevelSettings.Add("Shopping_Mall_Terrain", "---Interchange_levelsettings---");
        sceneLevelSettings.Add("woods_combined", "---Woods_levelsettings---");
        sceneLevelSettings.Add("Reserve_Base_DesignStuff", "---Reserve_levelsettings---");
        sceneLevelSettings.Add("shoreline_scripts", "---ShoreLine_levelsettings---");
        sceneLevelSettings.Add("default", "!settings");

        Plugin.NVGCustomGlobalFogIntensity.SettingChanged += SettingsUpdated;
        Plugin.CustomGlobalFogIntensity.SettingChanged += SettingsUpdated;


        Plugin.StreetsFogLevel.SettingChanged += SettingsUpdated;
        Plugin.CustomsFogLevel.SettingChanged += SettingsUpdated;
        Plugin.LighthouseFogLevel.SettingChanged += SettingsUpdated;
        Plugin.InterchangeFogLevel.SettingChanged += SettingsUpdated;
        Plugin.WoodsFogLevel.SettingChanged += SettingsUpdated;
        Plugin.ReserveFogLevel.SettingChanged += SettingsUpdated;
        Plugin.ShorelineFogLevel.SettingChanged += SettingsUpdated;
    }

    public void Update()
    {
        if (Input.GetKeyDown(Plugin.GraphicsToggle.Value.MainKey) && (!Input.GetKey(KeyCode.LeftShift)) && FPSCamera != null)
        {
            if (GraphicsMode)
            {
                GraphicsMode = false;
                ResetGraphics();
            }
            else
            {
                GraphicsMode = true;
                UpdateAmandsGraphics();
            }
        }
    }

    public void ActivateAmandsGraphics(GameObject fpscamera, PrismEffects prismeffects)
    {
        if (FPSCamera != null) return;

        FPSCamera = fpscamera;

        if (fpscamera == null) return;

        if (FPSCameraCamera == null)
        {
            FPSCameraCamera = FPSCamera.GetComponent<Camera>();
        }

        scene = SceneManager.GetActiveScene().name;
        if (!sceneLevelSettings.ContainsKey(scene)) scene = "default";

        FPSCameraCustomGlobalFog = FPSCamera.GetComponent<CustomGlobalFog>();
        if (FPSCameraCustomGlobalFog != null)
        {
            defaultFPSCameraCustomGlobalFog = FPSCameraCustomGlobalFog.enabled;
        }

        foreach (Component component in FPSCamera.GetComponents<Component>())
        {
            if (component.ToString() == "FPS Camera (UnityStandardAssets.ImageEffects.GlobalFog)")
            {
                FPSCameraGlobalFog = component as Behaviour;
                defaultFPSCameraGlobalFog = FPSCameraGlobalFog.enabled;
                break;
            }

            if (component.ToString() == "FPS Camera (MBOIT_Scattering)")
            {
                mBOIT_Scattering = component;
                if (mBOIT_Scattering != null)
                {
                    defaultMBOITZeroLevel = Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").GetValue<float>();
                }
            }
        }

        FPSCameraNightVision = FPSCamera.GetComponent<NightVision>();
        if (FPSCameraNightVision != null)
        {
            NVG = FPSCameraNightVision.On;
        }

        GraphicsMode = true;
        UpdateAmandsGraphics();
    }


    public void UpdateAmandsGraphics()
    {
        if (mBOIT_Scattering != null)
        {
            switch (scene)
            {
                case "City_Scripts":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.StreetsFogLevel.Value);
                    break;
                case "Laboratory_Scripts":
                    break;
                case "custom_Light":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.CustomsFogLevel.Value);
                    break;
                case "Lighthouse_Abadonned_pier":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.LighthouseFogLevel.Value);
                    break;
                case "Shopping_Mall_Terrain":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.InterchangeFogLevel.Value);
                    break;
                case "woods_combined":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.WoodsFogLevel.Value);
                    break;
                case "Reserve_Base_DesignStuff":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.ReserveFogLevel.Value);
                    break;
                case "shoreline_scripts":
                    Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel + Plugin.ShorelineFogLevel.Value);
                    break;
            }
        }

        if (FPSCameraCustomGlobalFog != null)
        {
            FPSCameraCustomGlobalFog.enabled = (scene == "Factory_Day" || scene == "Factory_Night" || scene == "default")
                ? false
                : defaultFPSCameraCustomGlobalFog;
            FPSCameraCustomGlobalFog.FuncStart = NVG ? Plugin.NVGCustomGlobalFogIntensity.Value : Plugin.CustomGlobalFogIntensity.Value;
            FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Normal;
        }

        if (FPSCameraGlobalFog != null)
        {
            FPSCameraGlobalFog.enabled = false;
        }
    }

    private void ResetGraphics()
    {
        if (mBOIT_Scattering != null)
        {
            Traverse.Create(mBOIT_Scattering).Field("ZeroLevel").SetValue(defaultMBOITZeroLevel);
        }

        if (FPSCameraNightVision)
        {
            FPSCameraNightVision.NoiseIntensity = defaultNightVisionNoiseIntensity;
            FPSCameraNightVision.ApplySettings();
        }

        if (FPSCameraCustomGlobalFog != null)
        {
            FPSCameraCustomGlobalFog.enabled = defaultFPSCameraCustomGlobalFog;
            FPSCameraCustomGlobalFog.FuncStart = 1f;
            FPSCameraCustomGlobalFog.BlendMode = CustomGlobalFog.BlendModes.Lighten;
        }

        if (FPSCameraGlobalFog != null)
        {
            FPSCameraGlobalFog.enabled = defaultFPSCameraGlobalFog;
        }
    }

    private void SettingsUpdated(object sender, EventArgs e)
    {
        if (GraphicsMode)
        {
            UpdateAmandsGraphics();
        }
    }
}