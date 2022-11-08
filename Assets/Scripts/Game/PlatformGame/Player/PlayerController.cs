using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Events;
using Game.GameManager;
using Game.LevelManager.DungeonLoader;
using Game.LevelManager.DungeonManager;
using PlatformGame.Weapons;

namespace PlatformGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        
        private Camera _camera;
        
        public void Awake()
        {
            _camera = Camera.main;
        }
        public void OnEnable()
        {
            RoomBhv.EnterRoomEventHandler += AdjustCamera;
            
        }

        public void OnDisable()
        {
           
            RoomBhv.EnterRoomEventHandler -= AdjustCamera;
            
        }
        
        private void AdjustCamera(object sender, EnterRoomEventArgs eventArgs)
        {
            var cameraXPosition = eventArgs.PositionInScene.x;
            var cameraYPosition = eventArgs.PositionInScene.y;
            const float cameraZPosition = -5f;
            _camera.transform.position = new Vector3(cameraXPosition, cameraYPosition, cameraZPosition);
        }

    }
}
