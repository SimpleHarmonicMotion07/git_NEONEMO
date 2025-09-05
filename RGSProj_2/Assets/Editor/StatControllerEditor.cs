#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Core;

[CustomEditor(typeof(StatsManager))]
public class StatControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Final Value�� �ǽð����� �ٲ�� �ν����� ���ΰ�ħ
        if (Application.isPlaying)
            Repaint();  // ��Ÿ���� ���� ���ΰ�ħ

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("���� ���� �� (Final Value)", EditorStyles.boldLabel);

        StatsManager controller = (StatsManager)target;

        if (controller.initialStats != null)
        {
            foreach (var stat in controller.initialStats)
            {
                string statName = stat.statName;
                float finalValue = controller.GetFinalValue(statName);
                EditorGUILayout.LabelField($"{statName}", finalValue.ToString("F2"));
            }
        }
    }
}
#endif
