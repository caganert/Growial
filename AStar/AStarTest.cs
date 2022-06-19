﻿
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    //private AStar aStar;

    [SerializeField] private NPCPath npcPath = null;
    [SerializeField] private bool moveNPC = false;
    [SerializeField] private SceneName sceneName = SceneName.Scene1_Farm;
    [SerializeField] private Vector2Int finishPosition;
    [SerializeField] private AnimationClip idleDownAnimationClip = null;
    [SerializeField] private AnimationClip eventAnimationClip = null;
    private NPCMovement npcMovement;


    private void Start()
    {

        npcMovement = npcPath.GetComponent<NPCMovement>();
        npcMovement.npcFacingDirectionAtDestination = Direction.down;
        npcMovement.npcTargetAnimationClip = idleDownAnimationClip;

    }

    private void Update()
    {
        if (moveNPC)
        {
            moveNPC = false;

            NPCScheduleEvent npcScheduleEvent = new NPCScheduleEvent(0, 0, 0, 0, Weather.none, Season.none, sceneName, new GridCoordinate(finishPosition.x, finishPosition.y), eventAnimationClip);

            npcPath.BuildPath(npcScheduleEvent);

        }


    }
}
