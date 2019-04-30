using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Transform> ExitPoints;
    public List<Transform> spawnPoints;
    public Transform EndPortalSpawn;

    private Dictionary<Transform, bool> UsedExitPoints;

    public Transform getRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    }

    public Transform GetExitPoint()
    {
        // Instance dictionary if not available.
        if(UsedExitPoints == null)
        {
            UsedExitPoints = new Dictionary<Transform, bool>();
            foreach (Transform exitPoint in ExitPoints)
            {
                UsedExitPoints[exitPoint] = false;
            }
        }

        // Find the first value that has no exitPoint.
        foreach (KeyValuePair<Transform,bool> exitPoint in UsedExitPoints)
        {
            // Does it already have an exit point?
            // If so, just continue.
            if (UsedExitPoints[exitPoint.Key])
            {                
                continue;
            }
            // Else, flag that it already has an exit, and return it.
            else
            {
                UsedExitPoints[exitPoint.Key] = true;
                return exitPoint.Key;
            }
        }
        return null;
    }
    
    public List<Room> finishRoom(GameObject EndPiece)
    {
        bool hasFinished = false;
        List<Room> addedRooms = new List<Room>();
        while(!hasFinished)
        {
            Transform exitPosition = GetEmptyExits();

            // Out of exit points?
            if(exitPosition == null)
            {
                // We're done.
                hasFinished = true;
            }
            else
            {
                Room r = Instantiate(EndPiece, exitPosition.position, exitPosition.rotation).GetComponent<Room>();
                addedRooms.Add(r);
                SetExitPoint(exitPosition, true);
            }
        }
        return (addedRooms);
    }

    public Transform GetEmptyExits()
    {
        foreach (KeyValuePair<Transform,bool> exitPoint in UsedExitPoints)
        {
            if (!UsedExitPoints[exitPoint.Key])
                return exitPoint.Key;
        }
        return null;
    }

    public Transform GetEndPortal()
    {
        return EndPortalSpawn;
    }

    public void SetExitPoint(Transform point, bool value)
    {
        // Instance dictionary if not available.
        if (UsedExitPoints == null)
        {
            UsedExitPoints = new Dictionary<Transform, bool>();
            foreach (Transform exitPoint in ExitPoints)
            {
                UsedExitPoints[exitPoint] = false;
            }
        }

        // Sanity check...
        if(UsedExitPoints.ContainsKey(point))
        {
            UsedExitPoints[point] = value;
        }
        else
        {
            Debug.LogError("This room doesn't have this point!");
        }

    }

    public bool CollidesWithOthers()
    {        
        Collider[] collisions = Physics.OverlapBox(this.transform.position, new Vector3(4,4,4));
        foreach (Collider col in collisions)
        {            
            if (!col.transform.IsChildOf(this.transform))
            {
                //Debug.Log("Found collider:" + col.name + "\nIt's parent is:" + col.transform.parent);
                return true;
            }
                
        }
        return false;
    }

    public bool CollidesWithOthers(Transform ignoredRoom)
    {
        Collider[] collisions = Physics.OverlapBox(this.transform.position, new Vector3(3, 3, 3));
        foreach (Collider col in collisions)
        {
            if (!col.transform.IsChildOf(this.transform) && !col.transform.IsChildOf(ignoredRoom))
            {
                //Debug.Log("Found collider:" + col.name + "\nIt's parent is:" + col.transform.parent);
                return true;
            }

        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireCube(this.transform.position, new Vector3(4,4,4));                

        if(ExitPoints.Count != 0 )
        {
            Gizmos.color = Color.yellow;
            foreach (Transform exitPoint in ExitPoints)
            {
                Gizmos.DrawSphere(exitPoint.position, 0.25f);
            }
        }
        
        if(spawnPoints.Count != 0)
        {
            Gizmos.color = Color.green;
            foreach (Transform spawnPoint in spawnPoints)
            {                
                Gizmos.DrawSphere(spawnPoint.position, 0.25f);
            }
        }
        if(EndPortalSpawn != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(EndPortalSpawn.position, 0.25f);
        }
    }
}
