using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


public class DungeonGenerator : MonoBehaviour
{
    [Header("References")]
    public MainManager manager;
    public GameObject EndPiece;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject EndPortal;
    public GameObject LoadingScreen;

    [Header("Properties")]
    public int NumberOfRooms;
    public int MaxNumberOfEnemies;

    [Space]
    public List<Room> roomList;    

    private List<Room> instantiatedRooms;
    private List<GameObject> instantiatedObjects;
    private Room startingRoom;
    private Room endingRoom;
    private GameObject playerEntity;
    
    public void GenerateRooms()
    {

        // Are we doing this for the first time?        
        if(instantiatedRooms == null)
        {
            // If so, define list
            instantiatedRooms = new List<Room>();
        }
        else
        {
            // If not, delete all rooms, then redefine it.            
            foreach (Room room in instantiatedRooms)
            {
                if(room != null)
                    DestroyImmediate(room.gameObject);
            }
            instantiatedRooms = new List<Room>();
        }

        int i = 0;
        int continues = 0;

        while (instantiatedRooms.Count < NumberOfRooms && continues < 500) 
        {
            i++;
            if (instantiatedRooms.Count == 0)
            {
                Room newRoom = Instantiate(roomList[UnityEngine.Random.Range(0, roomList.Count)], transform).GetComponent<Room>();
                startingRoom = newRoom;
                instantiatedRooms.Add(newRoom);
            }
            else
            {
                // Get a random position from an instantiated room.
                int random = UnityEngine.Random.Range(0, instantiatedRooms.Count);
                Room chosenRoom = instantiatedRooms[random];
                Transform randomPosition = chosenRoom.GetExitPoint();

                // Doesn't have any more exit points to get?
                if(randomPosition == null)
                {
                    //continue
                    continues++;
                    continue;
                }

                // We got a random position, spawn the room at it's location.
                Vector3 spawnPosition = randomPosition.position;
                int randomRoom = UnityEngine.Random.Range(0, roomList.Count);
                Room newRoom = Instantiate(roomList[randomRoom], spawnPosition ,Quaternion.identity,transform).GetComponent<Room>();

                // Get this room's exit point
                Transform newRoomExitPoint = newRoom.GetExitPoint();
                if(newRoomExitPoint != null)
                {
                    // Make sure they're facing each other.
                    newRoom.transform.RotateAround(newRoomExitPoint.position, Vector3.up, randomPosition.rotation.eulerAngles.y);
                    // Join them
                    newRoom.transform.position += randomPosition.position - newRoomExitPoint.position;                    
                }

                if(newRoom.CollidesWithOthers())
                {
                    chosenRoom.SetExitPoint(randomPosition, false);
                    DestroyImmediate(newRoom.gameObject);
                    continues++;
                    continue;
                }
                
                // Add the room to the list.
                instantiatedRooms.Add(newRoom);
                // we don't know if this is the last room, so we keep assigning it until it breaks out of the loop.
                endingRoom = newRoom;
            }
            continues = 0;
        }
        Debug.Log("Number of iterations: " + i);

        List<Room> addedEnds = new List<Room>();
        foreach (Room spawnedRoom in instantiatedRooms)
        {
            addedEnds.AddRange(spawnedRoom.finishRoom(EndPiece));
        }
        instantiatedRooms.AddRange(addedEnds);        
    }

    void Start()
    {
        instantiatedObjects = new List<GameObject>();
        StartCoroutine("CreateDungeon");        
    }    
      

    IEnumerator CreateDungeon()
    {
        bool pathAllowed = false;
        int passes = 0;        
        while (!pathAllowed || passes > 10)
        {                        
            GenerateRooms();            

            // Build nav mesh
            NavMeshSurface surface = startingRoom.GetComponentInChildren<NavMeshSurface>();
            surface.BuildNavMesh();

            // Spawn Player
            Transform spawnPoint = startingRoom.getRandomSpawnPoint();

            NavMeshAgent testAgent;
            NavMeshPath path = new NavMeshPath(); 
            if (playerEntity == null)
            {
                playerEntity = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                playerEntity.transform.position = spawnPoint.position;
                playerEntity.transform.rotation = spawnPoint.rotation;
            }
            testAgent = playerEntity.GetComponent<NavMeshAgent>();            
            
            

            testAgent.CalculatePath(endingRoom.transform.position, path);
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                // This is invalid pathing, log it
                Debug.LogError("Invalid pathing");
                //DestroyImmediate(player);
                passes++;
            }
            else
            {
                Debug.Log("Pathing is allowed");
                pathAllowed = true;
            }
            yield return null;
        }
        Debug.Log("Finished generating dungeon");

        SpawnAgents();        
    }

    private void SpawnAgents()
    {
        if (playerEntity != null)
        {
            // The agent was created, so we can keep going            
            
            // Avoid overloading
            if(MaxNumberOfEnemies > (instantiatedRooms.Count - 2))
            {
                MaxNumberOfEnemies = (instantiatedRooms.Count - 2);
            }

            // Spawn enemies
            for (int i = 0; i < MaxNumberOfEnemies; i++)
            {
                Transform spawnpoint = null;
                int f = 0;
                while(spawnpoint == null && f < 20)
                {
                    spawnpoint = instantiatedRooms[UnityEngine.Random.Range(1, instantiatedRooms.Count - 2)].getRandomSpawnPoint();
                    f++;
                }       
                
                if(spawnpoint != null)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, spawnpoint.rotation);
                    instantiatedObjects.Add(enemy);
                }
            }

            // Spawn end portal
            Transform endSpawn = endingRoom.GetEndPortal();
            EndPortal portal = Instantiate(EndPortal, endSpawn.position, endSpawn.rotation, EndPortal.transform).GetComponent<EndPortal>();
            portal.SetPlayer(playerEntity.transform);
            instantiatedObjects.Add(portal.gameObject);

            LoadingScreen.SetActive(false);

            StartGame(playerEntity);
        }
        else
        {
            Debug.LogError("No player in scene, caused probably by a generation error");
        }
    }    

    private void StartGame(GameObject player)
    {
        manager.AssignPlayer(player);
        manager.StartGame();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Layout"))
        {
            DungeonGenerator generator = (DungeonGenerator)target;
            generator.GenerateRooms();  
        }
    }
}
#endif