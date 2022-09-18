﻿using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.EnemyManager;
using Game.Events;
using Game.GameManager;
using Game.LevelManager.DungeonLoader;
using Game.NarrativeGenerator.Quests;
using Game.NPCs;
using ScriptableObjects;
using UnityEngine;
using Util;
using Game.Quests;

namespace Game.LevelManager.DungeonManager
{
    public class RoomBhv : MonoBehaviour, ISoundEmitter, IQuestElement
    {

        public static event StartRoomEvent StartRoomEventHandler;

        public DungeonRoom roomData;
        public List<int> northDoor;
        public List<int> southDoor;
        public List<int> eastDoor;
        public List<int> westDoor;

        public bool hasEnemies;
        private bool _hasPlacedInCenter;
        public EnemyByAmountDictionary enemiesDictionary;
        private Vector3 _position;

        public DoorBhv doorNorth;
        public DoorBhv doorSouth;
        public DoorBhv doorEast;
        public DoorBhv doorWest;

        public KeyBhv keyPrefab;
        public TriforceBhv triPrefab;
        public TreasureController treasurePrefab;
        public GameObject[] readableItemPrefabs;
        public GameObject[] npcPrefabs;

        public Collider2D colNorth;
        public Collider2D colSouth;
        public Collider2D colEast;
        public Collider2D colWest;

        public TileBhv tilePrefab;
        public BlockBhv blockPrefab;

        public Sprite northWall, southWall, eastWall, westWall;
        public GameObject NWColumn, NEColumn, SEColumn, SWColumn;

        public GameObject minimapIcon;

        public List<Vector3> spawnPoints;

        private Vector3 _availablePosition;

        private EnemyLoader _enemyLoader;

        private List<GameObject> _instantiatedEnemies;

        private Coordinates _coordinates;

        public static event EnterRoomEvent EnterRoomEventHandler;

        public int QuestId { get; set; }

        private void Awake()
        {
            hasEnemies = false;
            _hasPlacedInCenter = false;
            enemiesDictionary = new EnemyByAmountDictionary();
            _instantiatedEnemies = new List<GameObject>();
        }

        // Use this for initialization
        private void Start()
        {
            _coordinates = roomData.Coordinates;
            _enemyLoader = GetComponent<EnemyLoader>();

            SetLayout();
            _position = transform.position;
            if (RoomHasKey())
            {
                PlaceKeysInRoom();
            }
            if (RoomHasTreasure())
            {
                PlaceTreasuresInRoom();
            }
            if (RoomHasNpc())
            {
                PlaceNpcsInRoom();
            }
            if (roomData.IsStartRoom())
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
                minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
                GetAvailablePosition();
                StartRoomEventHandler?.Invoke(this, new StartRoomEventArgs(_availablePosition));
            }
            else if (roomData.IsFinalRoom())
            {
                PlaceTriforceInRoom();
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            }
            SelectEnemies();

            minimapIcon.transform.localScale = new Vector3(roomData.Dimensions.Width, roomData.Dimensions.Height, 1);
            
        }

        private void DebugRoomData()
        {
            Debug.Log($"The current room: X {roomData.Coordinates.X}, Y {roomData.Coordinates.Y} has the keys with ");
            foreach (var keyID in roomData.KeyIDs)
            {
                Debug.Log($"Key ID: {keyID}");
            }
        }

        private void SetLayout()
        {
            SetKeysToDoors();

            var centerX = roomData.Dimensions.Width / 2.0f - 0.5f;
            var centerY = roomData.Dimensions.Height / 2.0f - 0.5f;
            const float delta = 0.0f; //para que os colisores das portas e das paredes não se sobreponham completamente
            SetDoorsTransform(centerX, centerY, delta);

            SetCollidersOnRoom(centerX, centerY);

            InstantiateTiles(centerX, centerY);

            //Instantiate corner props
            var nwColumnObject = Instantiate(NWColumn, transform, true);
            nwColumnObject.transform.localPosition = new Vector2(-1f - centerX, roomData.Dimensions.Height - centerY);
            var seColumnObject = Instantiate(SEColumn, transform, true);
            seColumnObject.transform.localPosition = new Vector2(roomData.Dimensions.Width - centerX, -1f - centerY);
            var neColumnObject  = Instantiate(NEColumn, transform, true);
            neColumnObject.transform.localPosition = new Vector2(roomData.Dimensions.Width - centerX, roomData.Dimensions.Height - centerY);
            var swColumnObject = Instantiate(SWColumn, transform, true);
            swColumnObject.transform.localPosition = new Vector2(-1f - centerX, -1f - centerY);
        
            SetEnemySpawners(centerX, centerY);
        }

        private void InstantiateTiles(float centerX, float centerY)
        {
            for (var ix = 0; ix < roomData.Dimensions.Width; ix++)
            {
                for (var iy = 0; iy < roomData.Dimensions.Height; iy++)
                {
                    var tileID = roomData.Tiles[ix, iy];
                    TileBhv tileObj;
                    if (tileID == (int) Enums.TileTypes.Block)
                    {
                        tileObj = Instantiate(blockPrefab);
                        /*if (ix == 0)
                        {
                            if (iy == 0)
                            {
                                auxObj = Instantiate(NWColumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            if (iy == (roomData.Dimensions.Height - 1))
                            {
                                auxObj = Instantiate(SWColumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            tileObj.GetComponent<SpriteRenderer>().sprite = westWall;
                        }
                        else if (iy == 0)
                        {
                            tileObj.GetComponent<SpriteRenderer>().sprite = northWall;
                        }
                        else if (ix == (roomData.Dimensions.Width - 1))
                        {
                            if (iy == (roomData.Dimensions.Height - 1))
                            {
                                auxObj = Instantiate(SEColumn, transform, true);
                                auxObj.transform.localPosition =
                                    new Vector2(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY);
                            }

                            tileObj.GetComponent<SpriteRenderer>().sprite = eastWall;
                        }
                        else if (iy == (roomData.Dimensions.Height - 1))
                        {
                            tileObj.GetComponent<SpriteRenderer>().sprite = southWall;
                        }*/
                    }
                    else
                    {
                        tileObj = Instantiate(tilePrefab);
                    }

                    tileObj.SetPosition(ix - centerX, roomData.Dimensions.Height - 1 - iy - centerY, transform);
                }
            }
        }

        private void SetEnemySpawners(float centerX, float centerY)
        {
            var roomPosition = transform.position;
            var xOffset = roomPosition.x;
            var yOffset = roomPosition.y;

            var lowerHalfVer = (roomData.Dimensions.Height / Constants.NSpawnPointsHor);
            var upperHalfVer = (3 * roomData.Dimensions.Height / Constants.NSpawnPointsHor);
            var lowerHalfHor = (roomData.Dimensions.Width / Constants.NSpawnPointsVer);
            var upperHalfHor = (3 * roomData.Dimensions.Width / Constants.NSpawnPointsVer);
            var topHor = (Constants.DistFromBorder +
                          (roomData.Dimensions.Width * (Constants.NSpawnPointsVer - 1) / Constants.NSpawnPointsVer));
            var topVer = (Constants.DistFromBorder +
                          (roomData.Dimensions.Height * (Constants.NSpawnPointsHor - 1) / Constants.NSpawnPointsHor));

            //Create spawn points avoiding the points close to doors.
            for (var ix = Constants.DistFromBorder;
                 ix < (roomData.Dimensions.Width - Constants.DistFromBorder);
                 ix += (roomData.Dimensions.Width / Constants.NSpawnPointsVer))
            {
                for (var iy = Constants.DistFromBorder;
                     iy < (roomData.Dimensions.Height - Constants.DistFromBorder);
                     iy += (roomData.Dimensions.Height / Constants.NSpawnPointsHor))
                {
                    if (roomData.Tiles[ix, iy] == (int) Enums.TileTypes.Block) continue;
                    // Calculate the spawn point 2D position (spx, spy)
                    var spx = ix - centerX + xOffset;
                    var rh = roomData.Dimensions.Height - 1;
                    var spy = rh - iy - centerY + yOffset;
                    var point = new Vector3(spx, spy, 0);

                    // Add the calculated point to spawn point list
                    if (ix <= Constants.DistFromBorder || ix >= topHor)
                    {
                        if (iy >= lowerHalfVer && iy <= upperHalfVer) continue;
                        spawnPoints.Add(point);
                    }
                    else if (iy <= Constants.DistFromBorder || iy >= topVer)
                    {
                        if (ix >= lowerHalfHor && ix <= upperHalfHor) continue;
                        spawnPoints.Add(point);
                    }
                    else
                    {
                        spawnPoints.Add(point);
                    }
                }
            }
        }

        private void SetCollidersOnRoom(float centerX, float centerY)
        {
            //Posiciona os colisores das paredes da sala
            colNorth.transform.localPosition = new Vector2(0.0f, centerY + 1);
            colSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1);
            colEast.transform.localPosition = new Vector2(centerX + 1, 0.0f);
            colWest.transform.localPosition = new Vector2(-centerX - 1, 0.0f);
            colNorth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colSouth.GetComponent<BoxCollider2D>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colEast.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);
            colWest.GetComponent<BoxCollider2D>().size = new Vector2(1, roomData.Dimensions.Height + 2);

            //Ajusta sprites das paredes
            colNorth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colSouth.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(roomData.Dimensions.Width + 2, 1);
            colEast.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
            colWest.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(1, roomData.Dimensions.Height + 2);
        }

        private void SetDoorsTransform(float centerX, float centerY, float delta)
        {
            doorNorth.transform.localPosition = new Vector2(0.0f, centerY + 1 - delta);
            doorSouth.transform.localPosition = new Vector2(0.0f, -centerY - 1 + delta);
            doorEast.transform.localPosition = new Vector2(centerX + 1 - delta, 0.0f);
            doorWest.transform.localPosition = new Vector2(-centerX - 1 + delta, 0.0f);
        }

        private void OnDrawGizmos()
        {
            foreach (Vector3 spawnPoint in spawnPoints)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(spawnPoint, 1);
            }
        }

        private void SelectEnemies()
        {
            if (roomData.EnemiesByType == null) return;
            if (roomData.EnemiesByType.EnemiesByTypeDictionary.Count == 0) return;
            hasEnemies = true;
            enemiesDictionary = roomData.EnemiesByType.GetEnemiesForRoom();
        }

        public void SpawnEnemies()
        {
            var selectedSpawnPoints = new List<int>();
            _instantiatedEnemies.Clear();
            foreach (var enemiesFromType in enemiesDictionary)
            {
                foreach (var questId in enemiesFromType.Value.QuestIds)
                {
                    int actualSpawn;
                    if (selectedSpawnPoints.Count >= spawnPoints.Count)
                    {
                        selectedSpawnPoints.Clear();
                    }
                    do
                    {
                        actualSpawn = RandomSingleton.GetInstance().Next(0, spawnPoints.Count);
                    } while (selectedSpawnPoints.Contains(actualSpawn));
                    var enemy = _enemyLoader.InstantiateEnemyFromScriptableObject(
                        new Vector3(spawnPoints[actualSpawn].x, spawnPoints[actualSpawn].y, 0f), 
                        transform.rotation, enemiesFromType.Key, questId);
                    _instantiatedEnemies.Add(enemy);
                    enemy.GetComponent<EnemyController>().EnemyKilledHandler += RemoveFromDictionary;
                    selectedSpawnPoints.Add(actualSpawn);
                }
            }
        }

        public void OnRoomEnter()
        {
            if (hasEnemies)
            {
                ((ISoundEmitter)this).OnSoundEmitted(this, new EmitPitchedSfxEventArgs(AudioManager.SfxTracks.DoorClose, 1));
                SpawnEnemies();
            }
            minimapIcon.GetComponent<SpriteRenderer>().color = new Color(0.5433761f, 0.2772784f, 0.6320754f, 1.0f);
            EnterRoomEventHandler?.Invoke(this, new EnterRoomEventArgs(roomData.Coordinates, roomData.Dimensions, enemiesDictionary, gameObject.transform.position));
            ((IQuestElement) this).OnQuestTaskResolved(this, new QuestExploreRoomEventArgs( roomData.Coordinates, QuestId ));
        }

        private void SetKeysToDoors()
        {
            doorNorth.keyID = northDoor;
            doorSouth.keyID = southDoor;
            doorEast.keyID = eastDoor;
            doorWest.keyID = westDoor;
        }

        private bool RoomHasKey()
        {
            return roomData.KeyIDs.Count > 0;
        }
        private void PlaceKeysInRoom()
        {
            foreach (var actualKey in roomData.KeyIDs)
            {
                PlaceKeyInRoom(actualKey);
            }
        }

        private void PlaceKeyInRoom(int keyId)
        {
            GetAvailablePosition();
            var key = Instantiate(keyPrefab, _availablePosition, transform.rotation);
            key.transform.position = _availablePosition;
            key.KeyID = keyId;
        }

        private bool RoomHasTreasure()
        {
            return roomData.Items != null;
        }

        private void PlaceTreasuresInRoom()
        {
            foreach (var itemAmountPair in roomData.Items.ItemAmountBySo)
            {
                PlaceTreasureInRoom(itemAmountPair.Key, itemAmountPair.Value);
            }
        }
        private void PlaceTreasureInRoom(ItemSo item, QuestIdList questIds)
        {
            foreach (var questId in questIds.QuestIds)
            {
                if ( item as ReadableItemSo != null )
                    PlaceReadableItem(item, questId);
                else
                    PlaceTreasureItem(item, questId);
            }
        }

        private void PlaceReadableItem(ItemSo item, int questId)
        {
            GetAvailablePosition();
            var readableItem = Instantiate(readableItemPrefabs[Random.Range(0,readableItemPrefabs.Length)], transform).GetComponent<QuestDialogueInteraction>();
            readableItem.DialogueObj = item as ReadableItemSo;
            readableItem.QuestId = questId;
            readableItem.gameObject.transform.position = _availablePosition;
        }

        private void PlaceTreasureItem(ItemSo item, int questId)
        {
            GetAvailablePosition();
            var treasure = Instantiate(treasurePrefab, transform);
            treasure.Treasure = item;
            treasure.transform.position = _availablePosition;
            treasure.QuestId = questId;
        }

        private void PlaceTriforceInRoom()
        {
            GetAvailablePosition();
            var tri = Instantiate(triPrefab, transform);
            tri.transform.position = _availablePosition;
        
        }

        private void GetAvailablePosition()
        {
            if (!_hasPlacedInCenter)
            {
                _availablePosition = roomData.GetCenterMostFreeTilePosition() + _position;
                _hasPlacedInCenter = true;
            }
            else
            {
                _availablePosition = roomData.GetNextAvailablePosition(_availablePosition - _position) + _position;
            }
        }

        private bool RoomHasNpc(){
            return roomData.Npcs != null;
        }
    
        private void PlaceNpcsInRoom()
        {
            foreach (var npc in roomData.Npcs)
            {
                PlaceNpcInRoom(npc);
            }
        }

        private void PlaceNpcInRoom(NpcSo npc)
        {
            GameObject prefab = null;
            foreach (var npcPrefab in npcPrefabs)
            {
                if (npcPrefab.GetComponent<NpcController>().Npc == npc)
                    prefab = npcPrefab;
            }
            var npcController = Instantiate(prefab, transform);
            GetAvailablePosition();
            npcController.transform.position = _availablePosition;
        }

        public void KillEnemies()
        {
            foreach (var enemy in _instantiatedEnemies.Where(enemy => enemy != null))
            {
                Destroy(enemy.gameObject);
            }
        }
        
        public void RemoveFromDictionary(object sender, EnemySO killedEnemyWeapon)
        {
            enemiesDictionary. Remove(killedEnemyWeapon);
        }

    }
}
