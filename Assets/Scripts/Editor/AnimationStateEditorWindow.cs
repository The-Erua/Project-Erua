using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class AnimationStateEditorWindow : EditorWindow
{
    string animationName = "";
    AnimationType animationType = AnimationType.None;
    List<string> currentAnimationStates = new List<string>();

    [MenuItem("Window/Animation State Editor")]
    public static void ShowWindow()
    {
        GetWindow<AnimationStateEditorWindow>("Animation State Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Add New Animation State", EditorStyles.boldLabel);
        animationName = EditorGUILayout.TextField("Animation Name", animationName);
        animationType = (AnimationType)EditorGUILayout.EnumPopup("Animation Type", animationType);

        if (GUILayout.Button("Add and Save"))
        {
            AddAndSaveAnimationState(animationName, animationType);
        }

        if (GUILayout.Button("Delete and Save"))
        {
            DeleteAndSaveAnimationState(animationName);
        }
    }

    private void AddAndSaveAnimationState(string name, AnimationType type)
    {
        string path = Application.dataPath + "/TextAssets/AnimationStateDefinition.txt";
        File.AppendAllText(path, $"{name}, {(int)type}\n");

        UpdateAnimationHashClass(name, true);
    }

    private void DeleteAndSaveAnimationState(string name)
    {
        string path = Application.dataPath + "/TextAssets/AnimationStateDefinition.txt";
        if (File.Exists(path))
        {
            var lines = File.ReadAllLines(path).Where(line => !line.StartsWith(name + ",")).ToList();
            File.WriteAllLines(path, lines);

            UpdateAnimationHashClass(name, false);
        }
    }

    private void UpdateAnimationHashClass(string name, bool add)
    {
        string className = "AnimationHash";
        string filePath = Application.dataPath + $"/Scripts/{className}.cs";

        List<string> lines = new List<string>();
        if (File.Exists(filePath))
        {
            lines = File.ReadAllLines(filePath).ToList();
        }
        else
        {
            lines.Add("using UnityEngine;");
            lines.Add("");
            lines.Add($"public class {className}");
            lines.Add("{");
            lines.Add("}");
        }

        string hashLine = $"   public static readonly int {name.ToUpper()} = Animator.StringToHash(\"{name}\");";

        if (add)
        {
            if (!lines.Contains(hashLine))
            {
                lines.Insert(lines.Count - 1, hashLine);
            }
        }
        else
        {
            lines.Remove(hashLine);
        }

        File.WriteAllLines(filePath, lines);
        AssetDatabase.Refresh();
    }
}
