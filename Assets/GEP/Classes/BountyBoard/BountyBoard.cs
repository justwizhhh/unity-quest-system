using Cinemachine;
using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BountyBoard : MonoBehaviour
{
    // ------------------------------
    //
    // This "quest giver" object that will give the player a list of different bounties to do
    //
    // ------------------------------

    // Public variables
    [Tooltip("These are all of the bounties that the player can select and accept.")]
    public List<BountyObject> Bounties = new List<BountyObject>();

    [Space(10)]
    [Header("Bounty Board Settings")]
    [Tooltip("Is the bounty board being navigated by the player right now?")]
    public bool IsOpened;
    public GameObject BountyPrefab;
    public GameObject UIPrefab;

    [HideInInspector] public bool _isPlayerNearby;

    [Space(10)]
    [Header("Misc. Toggles")]
    [Tooltip("How far away should the selection UI be from the bounty board itself.")]
    public Vector3 SelectionUIOffset;
    [Tooltip("How quickly it should take to switch between cameras.")]
    public float CameraSpeed;

    // Component references
    private SphereCollider _playerCheckCol;
    private CinemachineVirtualCamera _lockCam;
    [HideInInspector] public BountyBoardUI _ui;

    private ThirdPersonController _player;
    private CinemachineVirtualCamera _playerCam;
    private PlayerUI _playerUI;
    private CinemachineBrain _cameraBrain;

    private void Start()
    {
        Bounties = GetComponentsInChildren<BountyObject>().ToList();
        
        _playerCheckCol = GetComponentInChildren<SphereCollider>();
        _lockCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _ui = Instantiate(UIPrefab, FindFirstObjectByType<Canvas>().transform).GetComponent<BountyBoardUI>();
        _ui._bountyBoard = this;

        _player = FindFirstObjectByType<ThirdPersonController>();
        _playerCam = _player.GetComponentInChildren<CinemachineVirtualCamera>();
        _playerUI = FindFirstObjectByType<PlayerUI>();
        _cameraBrain = FindFirstObjectByType<CinemachineBrain>();
        _cameraBrain.m_DefaultBlend.m_Time = CameraSpeed;
    }

    public void PlayerTrigger()
    {
        // Player has opened up the bounty menu
        IsOpened = true;
        _player.IsMoving = false;

        _playerCam.Priority = 0;
        _lockCam.Priority = 1;

        _ui._selectionUI.SetActive(false);
        _ui._menuUI.SetActive(true);
        _ui.Open();

        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayerLeave()
    {
        // Player has chosen to close the bounty menu
        IsOpened = false;
        _player.IsMoving = true;

        _playerCam.Priority = 1;
        _lockCam.Priority = 0;

        _ui._selectionUI.SetActive(true);
        _ui._menuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AssignBounty(BountyObject newBounty)
    {
        if (_player.GetComponentsInChildren<BountyObject>().Length < 
            _player.GetComponent<PlayerInteraction>().MaxBountyCount)
        {
            newBounty.transform.parent = _player.transform;
            Bounties.Find(x => x == newBounty).ActivateBounty();

            _playerUI.UpdateBountyList(false);
        }
    }

    public void ResetBounty(BountyObject newBounty)
    {
        newBounty.transform.parent = transform;
        newBounty.CurrentProgress = 0;
        Bounties.Find(x => x == newBounty).DeactivateBounty();

        _playerUI.UpdateBountyList(false);
    }

    public void RemoveBounty(BountyObject bounty)
    {
        Bounties.Remove(bounty);
        Destroy(bounty.gameObject);

        _playerUI.UpdateBountyList(true);
    }

    public void EditorAddBounty(string infoName)
    {
        // Give the bounty specific info if the user specified any
        // Else, this bounty will use 'TestBounty1' info by default
        Bounty newBountyInfo = AssetDatabase.LoadAssetAtPath<Bounty>(
            "Assets/GEP/Bounties/" + infoName + ".asset");

        if (newBountyInfo == null)
        {
            Debug.LogError("No scriptable object can be found of name: " + infoName + "!");
        }
        else
        {
            BountyObject newBounty = Instantiate(BountyPrefab, this.transform).GetComponent<BountyObject>();
            newBounty.BountyInfo = newBountyInfo;
            // Create new selectable button if in play mode
            if (Application.isPlaying)
            {
                Bounties.Add(newBounty);
                _ui.GenerateBountyButton(newBounty);
            }
        }
    }

    public void EditorRemoveBounty(string infoName)
    {
        BountyObject[] bounties = GetComponentsInChildren<BountyObject>();

        Bounty removedBountyInfo = AssetDatabase.LoadAssetAtPath<Bounty>(
            "Assets/GEP/Bounties/" + infoName + ".asset");

        if (removedBountyInfo == null)
        {
            Debug.LogError("No scriptable object can be found of name: " + infoName + "!");
        }
        else
        {
            if (bounties.Length > 0)
            {
                foreach (BountyObject bounty in GetComponentsInChildren<BountyObject>())
                {
                    if (bounty.BountyInfo == removedBountyInfo)
                    {
                        if (Application.isPlaying)
                        {
                            Destroy(bounty._button.gameObject);
                            Bounties.Remove(bounty);
                        }
                        DestroyImmediate(bounty.gameObject);
                    }
                }
            }
        }
    }

    public void EditorClearAllBounties()
    {
        BountyObject[] bounties = GetComponentsInChildren<BountyObject>();

        foreach (BountyObject bounty in bounties)
        {
            if (Application.isPlaying)
            {
                Destroy(bounty._button.gameObject);
                Bounties.Remove(bounty);
            }
            DestroyImmediate(bounty.gameObject);
        }
    }
}
