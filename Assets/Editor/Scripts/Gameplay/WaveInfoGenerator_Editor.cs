using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lotl.Gameplay.Waves;

[CustomEditor(typeof(WaveInfoGenerator))]
public class WaveInfoGenerator_Editor : Editor
{
    const string WaveIndexLabel = "Subwave Index";
    
    private WaveInfoGenerator waveInfoGenerator;
    private int waveIndex = 1;
    
    private void OnEnable()
    {
        waveInfoGenerator = target as WaveInfoGenerator;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(waveInfoGenerator == null) return;

        EditorGUILayout.Space();
        
        GUILayout.BeginHorizontal();

        waveIndex = EditorGUILayout.IntField(WaveIndexLabel, waveIndex);
        waveIndex = Mathf.Max(1, waveIndex);
        
        if(GUILayout.Button("Print Wave Info"))
        {
            WaveInfo waveInfo = waveInfoGenerator.GenerateWaveInfo(waveIndex, waveInfoGenerator.DefaultMaxWaveCount);

            string message = $"Generated info (Wave Index = {waveIndex}):\n" +
                $"Entry points ({waveInfo.EntryPointIndeces.Count}): " +
                $"{{{string.Join(", ", waveInfo.EntryPointIndeces)}}}.\n" + 
                $"Subwaves ({waveInfo.SubwaveDifficulties.Count}): " +
                $"{{{string.Join(", ", waveInfo.SubwaveDifficulties)}}}";

            Debug.Log(message);
        }

        GUILayout.EndHorizontal();
    }
}
