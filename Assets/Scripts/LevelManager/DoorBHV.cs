﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorBHV : MonoBehaviour
{

    //public GameManager gm;
    public List<int> keyID;
    public bool isOpen;
    public bool isClosedByEnemies;
    public Sprite lockedSprite;
    public Sprite closedSprite;
    public Sprite openedSprite;
    public Transform teleportTransform;
    public Material gradientMaterial;
    //	public int moveX;
    //	public int moveY;
    [SerializeField]
    private DoorBHV destination;
    private RoomBHV parentRoom;
    [SerializeField]
    private AudioClip unlockSnd;

    private AudioSource audioSrc;

    private void Awake()
    {
        keyID = new List<int>();
        isOpen = false;
        parentRoom = transform.parent.GetComponent<RoomBHV>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        //TODO arrumar essa gambiarra e dar um jeito da porta representar as 2
        //Chaves que a abrem
        int firstKeyID = -1;
        if (keyID.Count > 0)
            firstKeyID = keyID[0];
        if (keyID.Count == 0)
        {
            //Debug.Log("No Key");
            Destroy(gameObject);
        }
        else if (firstKeyID > 0)
        {

            //Debug.Log("Positive key id");
            //Render the locked door sprite with the color relative to its ID
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = lockedSprite;
            sr.material = gradientMaterial;
            sr.material.SetColor("gradientColor1", Util.colorId[firstKeyID - 1]);
            if (keyID.Count > 1)
                sr.material.SetColor("gradientColor2", Util.colorId[keyID[1] - 1]);
            else
                sr.material.SetColor("gradientColor2", Util.colorId[firstKeyID - 1]);


            //sr.color = Util.colorId[firstKeyID - 1];
            //text.text = keyID.ToString ();

        }
        if (parentRoom.hasEnemies)
        {
            isClosedByEnemies = true;
            if (firstKeyID == 0 || isOpen)
            {
                //Debug.Log("Negative key id");
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = closedSprite;
            }

        }
        //gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int firstKeyID = -1;
            if (keyID.Count > 0)
                firstKeyID = keyID[0];
            /*foreach (int id in keyID)
                Debug.Log("Lock Key: "+ id);
            foreach (int id in Player.instance.keys)
                Debug.Log("Player Key: " + id);*/
            List<int> commonKeys = keyID.Intersect(Player.instance.keys).ToList();
            if (firstKeyID == 0 || isOpen)
            {
                if (!isClosedByEnemies)
                {
                    audioSrc.PlayOneShot(audioSrc.clip, 0.8f);
                    MovePlayerToNextRoom();
                    GameManager.instance.UpdateRoomGUI(destination.parentRoom.x, destination.parentRoom.y);
                }
            }
            else if (commonKeys.Count() == keyID.Count)
            {
                if (!isClosedByEnemies)
                {
                    audioSrc.PlayOneShot(unlockSnd, 0.7f);
                    //Player.instance.keys.Remove(commonKeys[0]);
                    foreach (int key in commonKeys)
                    {
                        if (!Player.instance.usedKeys.Contains(key))
                            Player.instance.usedKeys.Add(key);
                    }

                    GameManager.instance.UpdateKeyGUI();
                    GameManager.instance.UpdateRoomGUI(destination.parentRoom.x, destination.parentRoom.y);
                    OpenDoor();
                    if (!destination.parentRoom.hasEnemies)
                        destination.OpenDoor();
                    isOpen = true;
                    destination.isOpen = true;
                    OnKeyUsed(commonKeys.First());
                    MovePlayerToNextRoom();
                }
            }
            else
            {
                OnRoomFailExit();
                OnRoomFailEnter();
            }
            /*if(parent.isEnd)
            {
                Debug.Log("The end");
                GameManager.state = GameManager.LevelPlayState.Won;
                //TODO change this to when the sierpinsk-force is taken
                gm.LevelComplete();
                return;
            }*/
        }
    }

    private void MovePlayerToNextRoom()
    {
        OnRoomExit(Player.instance.GetComponent<PlayerController>().GetHealth());
        //Enemy spawning logic here TODO make it better and work with the variable enemies SOs
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log("Lenght>>>" + enemies.Length);
        //Legacy: this was used when the doors were not closed when there are enemies in the room
        /*if (enemies.Length == 0)
        {
            parentRoom.hasEnemies = false;
        }
        else
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }*/
        //The normal room transition
        Player.instance.transform.position = destination.teleportTransform.position;
        RoomBHV parent = destination.parentRoom;
        Player.instance.AdjustCamera(parent.x, parent.y);
        destination.transform.parent.GetComponent<RoomBHV>().OnRoomEnter();

    }

    public void SetDestination(DoorBHV other)
    {
        destination = other;
    }

    //Methods to Player Profile
    private void OnRoomFailEnter()
    {
        PlayerProfile.instance.OnRoomFailEnter(new Vector2Int(destination.parentRoom.x, destination.parentRoom.y));
    }



    private void OnRoomFailExit()
    {
        PlayerProfile.instance.OnRoomFailExit(new Vector2Int(parentRoom.x, parentRoom.y));
    }

    private void OnRoomExit(int playerHealth)
    {
        PlayerProfile.instance.OnRoomExit(new Vector2Int(parentRoom.x, parentRoom.y), playerHealth);
    }

    private void OnKeyUsed(int id)
    {
        PlayerProfile.instance.OnKeyUsed(id);
    }

    public void OpenDoor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = openedSprite;
    }

    public void OpenDoorAfterKilling()
    {
        int firstKeyID = -1;
        if (keyID.Count > 0)
            firstKeyID = keyID[0];
        if (firstKeyID == 0 || isOpen)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = openedSprite;
        }
        isClosedByEnemies = false;
    }
}