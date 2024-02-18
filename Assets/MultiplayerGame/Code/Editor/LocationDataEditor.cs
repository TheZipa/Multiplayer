using MultiplayerGame.Code.Data.StaticData;
using UnityEditor;
using UnityEngine;

namespace MultiplayerGame.Code.Editor
{
    [CustomEditor(typeof(LocationData))]
    public class LocationDataEditor : UnityEditor.Editor
    {
        private const string PlayerSpawnTag = "Player Spawn";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Collect") == false) return;

            LocationData locationData = (LocationData)target;

            locationData.PlayersSpawnLocation = GameObject.FindGameObjectWithTag(PlayerSpawnTag).transform.position;

            EditorUtility.SetDirty(locationData);
            serializedObject.ApplyModifiedProperties();
        }
    }
}