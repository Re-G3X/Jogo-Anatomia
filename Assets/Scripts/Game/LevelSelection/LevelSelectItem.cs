﻿using System;
using Game.Events;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.LevelSelection
{
    public class LevelSelectItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public string LevelId { get; set; }

        [field: SerializeField] public LevelData levelData { get; set; }

        [SerializeField] private LevelDescription levelDescription;

        private bool isSelected;
        
        public static event LevelLoadEvent LoadLevelEventHandler;

        private void Start()
        {
            isSelected = false;
        }

        public void ConfirmStageSelection(InputAction.CallbackContext context)
        {
            if(!isSelected) return;
            Debug.Log("Confirmed Level!");
            LoadLevelEventHandler?.Invoke(this, new LevelLoadEventArgs(levelData.Dungeon, levelData.Quests, false));
            SceneManager.LoadScene("LevelWithEnemies");
        }

        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log("Selected Level!");
            isSelected = true;
            levelDescription.CreateDescriptions(levelData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log("Deselected Level!");
            isSelected = false;
        }
    }
}