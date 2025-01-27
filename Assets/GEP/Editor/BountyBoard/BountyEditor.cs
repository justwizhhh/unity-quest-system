using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BountyObject))]
public class BountyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BountyObject bounty = (BountyObject)target;

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "This object is the bounty (quest) itself. Here, we track the " +
            "progress the player has made, and also distribute all of the " +
            "rewards once the player completes the bounty.",
            MessageType.None, true);

        EditorGUILayout.Space();
        GUILayout.Label("Bounty Info (Scriptable Object Data)");
        bounty.BountyInfo = (Bounty)EditorGUILayout.ObjectField(bounty.BountyInfo, typeof(Bounty), false);
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Complete Bounty", GUILayout.Height(35)))
        {
            bounty.EditorCompleteBounty();
        }
        if (GUILayout.Button("Erase Bounty Progress", GUILayout.Height(35)))
        {
            bounty.EditorEraseBountyProgress();
        }

        EditorGUILayout.Space();
        bounty.IsInProgress = EditorGUILayout.Toggle("Is In Progress", bounty.IsInProgress);
        bounty.CurrentProgress = EditorGUILayout.IntSlider(
            "Current Progress",
            bounty.CurrentProgress,
            0,
            bounty._maxProgress);
        EditorGUILayout.Space();
        bounty.IsCompleted = EditorGUILayout.Toggle("Is Complete", bounty.IsCompleted);
    }
}
