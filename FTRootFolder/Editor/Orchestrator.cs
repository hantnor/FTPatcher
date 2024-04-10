/* 
 * By https://value.gay for hantnor
 *
 * Copyright 2024 ValueFactory https://shader.gay
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the â€œSoftwareâ€), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED â€œAS ISâ€, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
 * NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Set-up:
 *   1. Create patch using hdiffz.exe: https://github.com/sisong/HDiffPatch
 *    Run the following line in a command line
 *      hdiffz.exe "HANOriginal.FBX" "HANEdited.FBX" patch.bin
 *
 *    The both FBX files should be in the same directory as the hdiffz.exe executable.
 *    The 'Original.FBX' file is the original FBX that was edited to have the FT blendshapes.
 *    The 'Edited.FBX' file the edited FBX that has the FT blendshapes. This is the FBX
 *    that will be produced when the original FBX is patched.
 *
 *    'patch.bin' is the file path to the patch binary.
 *
 *   2. Set up the package folder for the model you're making the patch for.
 *
 *   3. Configure the orchestrator script to work with the FBX.
 *    You can search for '@Config' in this file to find all the places where things need to be
 *    adjusted for the model.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

// NOTE(valuef):
// @Config
// When configuring this to patch other models, rename the namespace to avoid any clashes during
// compile time! I'd recommend changing out the avatar name here for the avatar that you'll be
// adapting the patcher for.
namespace hantnor.HANNamespace.FT {

public class Orchestrator : EditorWindow {

  // NOTE(valuef):
  // @Config
  // The GUID of the original FBX of the model that'll be patched. The orchestrator will try to find
  // the FBX using this GUID so that the user doesn't have to.
  // Look at the .meta file of the FBX to find the GUID of it.
  public const string ORIGINAL_FBX_GUID = "HANGUID"; 

  // NOTE(valuef):
  // @Config
  // The path pointing to the original FBX of the model that'll be patched. The orchestrator will
  // use this to try to find the FBX so that the user doesn't have to.
  public const string DEFAULT_FBX_PATH = "HANOriginalPath/HANOriginal.fbx";

  // NOTE(valuef):
  // @Config
  // The path at which the original FBX will be backed up to before patching.
  public const string BACKUP_FBX_PATH = "HANOriginalPath/HANOriginal BACKUP.fbx"; 

  // NOTE(valuef):
  // @Config
  // The path pointing to the patch file that will patch the original FBX into the edited FT FBX.
  public const string PATCH_FILE_PATH = "Assets/Han's Creations/HANNamespace 1.0 FT/patch.bin";

  // NOTE(valuef):
  // @Config
  // The path pointing to the hpatchz.exe executable which will patch the FBX with the patch.bin
  // file to create the final FT patched FBX.
  public const string HPATCHZ_FILE_PATH = "Assets/Han's Creations/HANNamespace 1.0 FT/hpatchz.exe";

  // NOTE(valuef):
  // @Config
  // The MenuItem string here defines the path in the top bar of the Unity editor that'll the button
  // to open the patched will be placed in.
  [MenuItem("Tools/Han's Creations/HANNamespace - FT Patcher")]
  public static void open() {

    var should_set_pos = !EditorWindow.HasOpenInstances<Orchestrator>();

    try_to_find_HANNamespace_fbx_if_needed();

    var window = EditorWindow.GetWindow<Orchestrator>(false, "HANNamespace FT DLC");

    if(should_set_pos) {
      window.position = new Rect(Screen.width * .5f, Screen.height * .35f, 600, 320);
    }

    window.minSize = new Vector2(600, 320);
    window.Show();
  }

  public static GameObject HANNamespace_fbx;
  public static bool first_open = true;

  void OnGUI() {
    if(first_open) {
      try_to_find_HANNamespace_fbx_if_needed();
      first_open = false;
    }
  
    if(wrap_label == null) {
      wrap_label = new GUIStyle(EditorStyles.label);
      wrap_label.wordWrap = true;
    }
    if(bold_label == null) {
      bold_label = new GUIStyle(EditorStyles.boldLabel);
      bold_label.wordWrap = true;
    }
    if(header_label == null) {
      header_label = new GUIStyle(EditorStyles.boldLabel);
      header_label.wordWrap = true;
      header_label.fontSize = (int)(2.0f * header_label.fontSize);
    }
    if(hantnor_logo == null) {
      hantnor_logo = Resources.Load<Texture2D>("hantnor");
    }
    if(bold_warn_label == null) {
      bold_warn_label = new GUIStyle(EditorStyles.boldLabel);
      bold_warn_label.wordWrap = true;
      bold_warn_label.normal.textColor = Color.yellow;
    }
    if(link_color == null) {
      link_color= new GUIStyle(EditorStyles.label);
      link_color.wordWrap = true;
      link_color.normal.textColor = new Color(66f/255f, 135/255f, 245/255f,1);
    }

    EditorGUI.indentLevel++;

    EditorGUILayout.Space();

    GUILayout.BeginHorizontal();
    GUILayout.Box(hantnor_logo, GUILayout.Height(64), GUILayout.Width(64));
    // NOTE(valuef):
    // @Config
    EditorGUILayout.LabelField("Han's Creations - HANNamespace FT Patcher", header_label, GUILayout.Height(64));
    GUILayout.EndHorizontal();

    EditorGUILayout.Space();
    EditorGUILayout.Space();

    EditorGUILayout.LabelField("Welcome to the patcher!", bold_label);
    EditorGUILayout.LabelField("By default, you can hit the 'Patch' button on its own.", wrap_label);
    EditorGUILayout.LabelField("Optionally, a 'HANNamespace FBX' slot is available if you have a specific prefab youâ€™d like to apply the patch onto.", wrap_label);

    EditorGUILayout.LabelField("Have fun!", bold_label);

    EditorGUILayout.Space();
    EditorGUILayout.Space();

    HANNamespace_fbx = (GameObject)EditorGUILayout.ObjectField("HANNamespace FBX", HANNamespace_fbx, typeof(GameObject), false);

    EditorGUILayout.Space();

    if(HANNamespace_fbx == null) {
      EditorGUILayout.LabelField("Couldn't automatically find the HANNamespace fbx!", bold_warn_label);
      EditorGUILayout.LabelField("In this case, you'll have to manually drag in the fbx from your Project view into the slot above.", wrap_label);
    }

    GUI.enabled = HANNamespace_fbx != null;
    if(indented_button("Patch")) {

      var project_root = Application.dataPath;
      {
        var idx = project_root.LastIndexOf("Assets");
        if(idx > 0) {
          project_root = project_root.Remove(idx);
        }
        else {
          EditorUtility.DisplayDialog("Patcher",
            $"Internal error: failed to get the project root directory (original directory is '{project_root}', failed to find the 'Assets' index.\nPatching will not continue. Please contact a programmer.",
            "OK"
          );
          return;
        }
      }

      var fbx_relative_path = AssetDatabase.GetAssetPath(HANNamespace_fbx);
      var fbx_path = Path.Combine(project_root, fbx_relative_path);
      var patch_path = Path.Combine(project_root, PATCH_FILE_PATH);
      var hpatchz_path = Path.Combine(project_root, HPATCHZ_FILE_PATH);
      var backup_path = Path.Combine(project_root, BACKUP_FBX_PATH);
      var temp_path = Path.Combine(Path.GetDirectoryName(fbx_path), "PATHCER TEMP.fbx");

      Debug.Log($"Patcher info:");
      Debug.Log($"  FBX path: '{fbx_path}'");
      Debug.Log($"  Patch path: '{patch_path}'");
      Debug.Log($"  Hpatchz path: '{hpatchz_path}'");
      Debug.Log($"  Backup FBX path: '{backup_path}'");
      Debug.Log($"  Temp FBX path: '{temp_path}'");

      // NOTE(valuef): Patch the FBX and store the result in the temp_path
      // 2024-01-31
      try {
        using(var p = new System.Diagnostics.Process()) {

          // NOTE(valuef): Must be absolute paths!
          // 2024-01-31
          p.StartInfo.FileName = hpatchz_path;
          p.StartInfo.Arguments = $@"""{fbx_path}"" ""{patch_path}"" ""{temp_path}""";
          p.StartInfo.UseShellExecute = false;
          p.StartInfo.RedirectStandardOutput = true;
          p.StartInfo.RedirectStandardError = true;
          p.Start();
          
          p.WaitForExit();

          var stdout = p.StandardOutput.ReadToEnd();
          var stderr = p.StandardError.ReadToEnd();
          var exit_code = p.ExitCode;

          Debug.Log($"Stdout: {stdout}");
          Debug.Log($"Stderr: {stderr}");
          Debug.Log($"Exit code: {exit_code}");

          if(exit_code != 0) {
            EditorUtility.DisplayDialog("Patcher",
              $"The patcher failed to patch the FBX:\n{stdout}\n{stderr}\n\nMake sure the FBX you're patching is the original, unedited FBX and is not a newer or older version than is supported!",
              "OK"
            );

            File.Delete(temp_path);
            return;
          }
        }
      }
      catch(System.Exception ex) {
        EditorUtility.DisplayDialog("Patcher",
          $"An internal error occurred while starting the patcher process.\nMore information has been logged to the console.\nPatching will not continue.",
          "OK"
        );
        Debug.LogException(ex);

        File.Delete(temp_path);
        return;
      }

      // NOTE(valuef): Safety in case a file already exists in the backup path.
      // 2024-01-31
      if(File.Exists(backup_path)) {
        var should_delete = EditorUtility.DisplayDialog("Patcher",
          $"The patcher will create a backup of the original FBX file at '{backup_path}' BUT there already exists a FBX there.\n\nYou can chose to delete the existing file, but beware that THIS CANNOT BE UNDONE.",
          "DELETE EXISTING PATCHED FILE", "Cancel"
        );

        if(should_delete) {
          try {
            File.Delete(backup_path);
          }
          catch(System.Exception ex) {
            File.Delete(temp_path);

            EditorUtility.DisplayDialog("Patcher",
              $"An internal error occurred while trying to delete the existing backup FBX file at '{backup_path}'. More information has been logged into the console. Patching will not continue.",
              "OK"
            );
            Debug.LogException(ex);
            return;
          }
        }
        else {
          File.Delete(temp_path);
          return;
        }
      }

      // NOTE(valuef): Back up the original FBX.
      // 2024-01-31
      {
        var success = AssetDatabase.CopyAsset(fbx_relative_path, BACKUP_FBX_PATH);
        if(!success) {
          File.Delete(temp_path);

          EditorUtility.DisplayDialog("Patcher",
            $"Failed to back up the original FBX at '{fbx_relative_path}' to '{BACKUP_FBX_PATH}'.\nThe patching will not continue.",
            "OK"
          );
          return;
        }
      }

      // NOTE(valuef): Replace the original fbx with the patched fbx while maintaining the metadata
      // of the original fbx.
      // 2024-01-31
      try {
        File.Replace(temp_path, fbx_path, null);
      }
      catch(System.Exception ex) {
        EditorUtility.DisplayDialog("Patcher",
          $"Failed replace the original FBX at '{fbx_path}' with the patched FBX at '{temp_path}'. More information has been logged into the console.",
          "OK"
        );
        Debug.LogException(ex);
        File.Delete(temp_path);
        return;
      }

      // NOTE(valuef): "Reload" the newly patched fbx so Unity has a chance to reprocess it.
      // 2024-01-31
      AssetDatabase.ImportAsset(DEFAULT_FBX_PATH);

      EditorUtility.DisplayDialog("Patcher", $"Patch successful!", "OK");
    }
    GUI.enabled = true;

    GUILayout.FlexibleSpace();
    {
      GUILayout.Label("by ValueFactory (value.gay)", link_color);
      var rect = GUILayoutUtility.GetLastRect();

      var ev = Event.current;
      if(rect.Contains(ev.mousePosition) && ev.type == EventType.MouseDown) {
        EditorUtility.OpenWithDefaultApp("https://value.gay");
        ev.Use();
      }

      EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
    }

    EditorGUI.indentLevel--;
  }

  public static 
  bool 
  indented_button(
    string content,
    params GUILayoutOption[] layout
  ) {
    EditorGUILayout.BeginHorizontal();
    // NOTE(valuef): 15.0f is from EditorGUI.indent which is internal to EditorGUI.cs of UnityEditor.dll
    // 2022-05-25
    GUILayout.Space(EditorGUI.indentLevel * 15.0f);
    var ret = GUILayout.Button(content, layout);
    EditorGUILayout.EndHorizontal();
    return ret;
  }

  public static
  void
  try_to_find_HANNamespace_fbx_if_needed() {
    if(HANNamespace_fbx != null) {
      return;
    }

    // NOTE(valuef): Try to find the FBX via the GUID.
    // 2024-01-31
    do {
      var path = AssetDatabase.GUIDToAssetPath(ORIGINAL_FBX_GUID);
      if(string.IsNullOrWhiteSpace(path)) {
        break;
      }

      var go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
      if(go == null) {
        break;
      }

      HANNamespace_fbx = go;
      return;

    } while(false);

    // NOTE(valuef): Try to find the FBX via the default FBX path.
    // 2024-01-31
    do {
      var go = (GameObject)AssetDatabase.LoadAssetAtPath(DEFAULT_FBX_PATH, typeof(GameObject));
      if(go == null) {
        break;
      }

      HANNamespace_fbx = go;
    } while(false);
  }

  public static GUIStyle wrap_label;
  public static GUIStyle bold_label;
  public static GUIStyle header_label;
  public static GUIStyle link_color;
  public static GUIStyle bold_warn_label;
  public static Texture2D hantnor_logo;
}

[InitializeOnLoad]
public class Autoload {
  static Autoload() {

    {
      // NOTE(valuef):
      // @Config
      // The key suffix is a string that's used to determine if the project has shown the
      // orchestrator window once. You can change this out for different models.
      var key_suffix = "hantnor.HANNamespace.FT.1";
      var key = Path.Combine(Application.dataPath, key_suffix);

      if(!EditorPrefs.HasKey(key)) {
        EditorPrefs.SetBool(key, true);
        Orchestrator.open();
      }
    }

    AssetDatabase.importPackageCompleted += (name) => {
      Orchestrator.try_to_find_HANNamespace_fbx_if_needed();
    };
  }
}
}
