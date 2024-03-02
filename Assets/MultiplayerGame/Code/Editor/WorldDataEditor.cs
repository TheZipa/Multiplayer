using System.Linq;
using MultiplayerGame.Code.Data.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerGame.Code.Editor
{
    [CustomEditor(typeof(WorldData))]
    public class WorldDataEditor : UnityEditor.Editor
    {
        private const string PlayerSpawnTag = "Player Spawn";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //if (GUILayout.Button("Screenshot")) ScreenCapture.CaptureScreenshot($"{SceneManager.GetActiveScene().name}.png");
            if (GUILayout.Button("Collect Player Spawn point") == false) return;

            WorldData worldData = (WorldData)target;
            SetPlayerSpawnLocation(worldData);
            EditorUtility.SetDirty(worldData);
            serializedObject.ApplyModifiedProperties();
        }

        private static void SetPlayerSpawnLocation(WorldData worldData) =>
            worldData.Maps.First(m => m.SceneName == SceneManager.GetActiveScene().name)
                .PlayersSpawnLocation = GameObject.FindGameObjectWithTag(PlayerSpawnTag).transform.position;
    }
}