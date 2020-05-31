using UnityEngine;
using UnityEditor;

namespace EasySurvivalScripts
{
    [InitializeOnLoad]
    public class OtherAssetsWindow : EditorWindow
    {
        static OtherAssetsWindow()
        {
            if (PlayerPrefs.GetInt("akjsdakjsd213123kjs") == 0)
            {
                OpenWindow();

                PlayerPrefs.SetInt("akjsdakjsd213123kjs", 1);
            }
        }

        [MenuItem("Survival Scripts/Assets")]
        public static void OpenWindow()
        {
            GetWindow<OtherAssetsWindow>("Assets");

            GetWindow<OtherAssetsWindow>("Assets").maxSize = new Vector2(1100, 600);
            GetWindow<OtherAssetsWindow>("Assets").minSize = new Vector2(1100, 601);
            GetWindow<OtherAssetsWindow>("Assets");
        }

        [MenuItem("Survival Scripts/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();

            Debug.Log("Player Prefs has been deleted!");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUIStyle style = new GUIStyle();
            style.fontSize = 25;
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField("Aljebro Studio - Asset Library", style);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            style.fontSize = 15;
            style.fontStyle = FontStyle.BoldAndItalic;
            EditorGUILayout.LabelField("For painless creation of any type of game mechanic, do have a look at these Assets!", style);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t1 = (Texture)Resources.Load("EditorContent/EasyObjectExamine");

            if (GUILayout.Button(t1, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-object-examine-134061");

            }

            Texture t2 = (Texture)Resources.Load("EditorContent/EasyFuseInteraction");

            if (GUILayout.Button(t2, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-fuse-circuit-interaction-134063");
            }

            Texture t3 = (Texture)Resources.Load("EditorContent/EasyHazmatSuit");

            if (GUILayout.Button(t3, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-hazmat-suit-134065");
            }

            Texture t4 = (Texture)Resources.Load("EditorContent/EasyInputInteraction");

            if (GUILayout.Button(t4, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-input-interaction-134064");
            }

            Texture t5 = (Texture)Resources.Load("EditorContent/EasyInventory");

            if (GUILayout.Button(t5, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/easy-inventory-134070");
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            Texture t6 = (Texture)Resources.Load("EditorContent/EasyLeverInteraction");

            if (GUILayout.Button(t6, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-lever-interaction-134062");
            }

            Texture t7 = (Texture)Resources.Load("EditorContent/EasyMinimap");

            if (GUILayout.Button(t7, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/easy-minimap-134067");
            }

            Texture t8 = (Texture)Resources.Load("EditorContent/FullBodyFPSController");

            if (GUILayout.Button(t8, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/full-body-fps-controller-134060");
            }

            Texture t9 = (Texture)Resources.Load("EditorContent/EasyDialoguesInteraction");

            if (GUILayout.Button(t9, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-dialogues-interaction-134071");
            }

            Texture t10 = (Texture)Resources.Load("EditorContent/EasySurvivalScripts");

            if (GUILayout.Button(t10, GUILayout.Height(124), GUILayout.Width(200)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/integration/easy-survival-scripts-134072");
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            style.fontSize = 25;
            style.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Support [Click below] : ", style);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            style.fontStyle = FontStyle.Italic;
            style.fontSize = 20;
            style.normal.textColor = Color.blue;

            if (GUILayout.Button("YouTube : https://www.youtube.com/channel/UCJMf2I-etK8yvXhSZM4olWg",  style))
            {
                Application.OpenURL("https://www.youtube.com/channel/UCJMf2I-etK8yvXhSZM4olWg");
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.TextField("Email : aljebrostudio@gmail.com", style);

            //if (GUILayout.Button("Email : aljebrostudio@gmail.com", style))
            //{

            //}

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Website", style))
            {
                Application.OpenURL("http://www.aljebros.com/");
            }

        }
    }
}