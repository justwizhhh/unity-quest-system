using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BountyBoard))]
public class BountyBoardEditor : Editor
{
    BountyBoard _board;

    string _addedBountyName
    {
        get => EditorPrefs.GetString("BountyBoard_AddedBountyName", "");
        set => EditorPrefs.SetString("BountyBoard_AddedBountyName", value);
    }

    string _removedBountyName
    {
        get => EditorPrefs.GetString("BountyBoard_RemovedBountyName", "");
        set => EditorPrefs.SetString("BountyBoard_RemovedBountyName", value);
    }

    public override void OnInspectorGUI()
    {
        _board = (BountyBoard)target;

        EditorGUILayout.HelpBox(
            "This object stores all of the bounties (quests) that the player " +
            "can accept/reject, and assigns it to the player accordingly." +
            "" +
            "Use the 'Add New Bounty' button to add a new blank bounty to the" +
            "bounty board, where from there, you can assign clear rewards and " +
            "conditions by replacing the 'BountyInfo' scriptable object!",
            MessageType.None, true);

        EditorGUILayout.Space();
        GUILayout.Label("New Bounty Name (Scriptable Object File Name)");
        _addedBountyName = EditorGUILayout.TextField(_addedBountyName);
        if (GUILayout.Button("Add New Bounty", GUILayout.Height(35)))
        {
            _board.EditorAddBounty(_addedBountyName);
        }
        GUILayout.Label("Removed Bounty Name (Scriptable Object File Name)");
        _removedBountyName = EditorGUILayout.TextField(_removedBountyName);
        if (GUILayout.Button("Remove Bounty", GUILayout.Height(35)))
        {
            _board.EditorRemoveBounty(_removedBountyName);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Clear All Bounties", GUILayout.Height(35)))
        {
            _board.EditorClearAllBounties();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Force Open Menu"))
        {
            _board.PlayerTrigger();
        }
        if (GUILayout.Button("Force Close Menu"))
        {
            _board.PlayerLeave();
        }

        EditorGUILayout.Space();
        DrawDefaultInspector();
    }
}
