using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
//using Facebook.Unity.Settings;
using System.Linq;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using GameAnalyticsSDK;
using Unity.VisualScripting;
using Voodoo.Tiny.Sauce.Internal;
using Voodoo.Tiny.Sauce.Internal.Editor;
using GameAnalyticsSDK.Setup;
public enum Company { None, Gamina}
public class ProjectSetUpWindow : OdinEditorWindow
{
    [BoxGroup("Setup")]
    [OnValueChanged("SetUpKickOffList")]
    public Company CompanyName = Company.None;
    [OnValueChanged("SetUpKickOffList")]
    [BoxGroup("Setup")]
    public string GameName = string.Empty;
    [BoxGroup("Setup")]
    [OnValueChanged("SetUpKickOffList")]
    public string bundleName = string.Empty;
    [BoxGroup("Setup")]
    [OnValueChanged("SetUpKickOffList")]
    [Sirenix.OdinInspector.ReadOnly]
    public string bundleID = "com.companyname";
    
    [BoxGroup("Setup")]
    [OnValueChanged("SetUpKickOffList")]
    public string FacebookAppID;
    [BoxGroup("Setup")]
    [OnValueChanged("SetUpKickOffList")]
    public string FacebookClientToken;

    [BoxGroup("Setup")] 
    [InlineEditor(InlineEditorModes.GUIOnly)]
    public TinySauceSettings TinySauceSettings;
    
    [BoxGroup("Setup")] 
    [OnValueChanged("SetUpKickOffList")] 
    [Sirenix.OdinInspector.ReadOnly]
    public string VoodooAnalyticsId;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        saveChangesMessage = "There are empty fields. Please fill them";
        TinySauceSettings = (TinySauceSettings)AssetDatabase.LoadAssetAtPath("Assets/Resources/TinySauce/Settings.asset", typeof(TinySauceSettings));
        LoadInfo();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        SaveInfo();
    }
    private void SetUpKickOffList()
    {
        bundleID = "com." + CompanyName.ToString().ToLower() + "." + bundleName.ToLower();
        bundleID = bundleID.Replace(" ", "");
        PlayerSettings.companyName = CompanyName.ToString();
        PlayerSettings.productName = GameName;
      
       
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleID);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleID);
        switch (CompanyName)
        {
            case Company.None:
                break;
            case Company.Gamina:
                PlayerSettings.iOS.appleDeveloperTeamID = "---";
                break;
            default:
                break;
        }
    }
    private bool CanClose()
    {
        if (CompanyName == Company.None)
            return true;
        if (string.IsNullOrEmpty(GameName))
            return true;
        if (string.IsNullOrEmpty(FacebookAppID))
            return true;
        if (string.IsNullOrEmpty(VoodooAnalyticsId))
            return true;
        return false;
    }
    private void SaveInfo()
    {
        EditorPrefs.SetInt(PlayerSettings.productName + "compName", (int)CompanyName);
        EditorPrefs.SetString(PlayerSettings.productName + "gameName", GameName);
        EditorPrefs.SetString(PlayerSettings.productName + "bundleId", bundleID);
        EditorPrefs.SetString(PlayerSettings.productName + "facebookId", FacebookAppID);
        EditorPrefs.SetString(PlayerSettings.productName + "vdGameId", TinySauceSettings.EditorIdfa);
        
        AssetDatabase.SaveAssets();
        FacebookSettings.AppIds = new List<string>{FacebookAppID,};
        FacebookSettings.ClientTokens = new List<string>{FacebookClientToken,};
        ManifestMod.GenerateManifest();
      
    }
    private void LoadInfo()
    {
        CompanyName = (Company)EditorPrefs.GetInt(PlayerSettings.productName + "compName", 0);
        GameName = EditorPrefs.GetString(PlayerSettings.productName + "gameName", "ExampleGame");
        bundleID = EditorPrefs.GetString(PlayerSettings.productName + "bundleId", "com." + CompanyName.ToString().ToLower() + "." + GameName.ToLower());
        FacebookAppID = EditorPrefs.GetString(PlayerSettings.productName + "facebookId", FacebookAppID);
        VoodooAnalyticsId = EditorPrefs.GetString(PlayerSettings.productName + "vdGameId", TinySauceSettings.EditorIdfa);
        
        FacebookSettings.AppIds = new List<string>{FacebookAppID,};
        FacebookSettings.ClientTokens = new List<string>{FacebookClientToken,};
        Debug.Log(EditorPrefs.GetString(PlayerSettings.productName + "facebookId", FacebookAppID));
    }
    public override void SaveChanges()
    {
        base.SaveChanges();
        SaveInfo();
    }
    [MenuItem("GaminaBase/ProjectSetup")]
    static void DebugShow()
    {
        ProjectSetUpWindow window = (ProjectSetUpWindow)GetWindow(typeof(ProjectSetUpWindow));
        window.Show();
    }
}