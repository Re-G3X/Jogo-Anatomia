﻿using System;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.Quests;
using UnityEngine;

namespace Game.LevelSelection
{
    [Serializable]
    [CreateAssetMenu(fileName = "LevelData", menuName = "Overlord-Project/LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [field:SerializeField] public QuestLineList QuestLines { get; set; }
        [field:SerializeField] public DungeonFileSo Dungeon { get; set; }
        protected bool IsCompleted { get; set; }
        protected bool HasSurrendered { get; set; }

        public void Init(QuestLineList questLines, DungeonFileSo dungeon)
        {
            QuestLines = CreateInstance<QuestLineList>();
            QuestLines.Init(questLines);
            Dungeon = dungeon;
            QuestLines.ConvertDataForCurrentDungeon(Dungeon.Parts);
            IsCompleted = false;
            HasSurrendered = false;
        }

        public void CompleteLevel()
        {
            IsCompleted = true;
        }

        public void GiveUpLevel()
        {
            HasSurrendered = true;
        }

        public bool HasCompleted()
        {
            return IsCompleted || HasSurrendered;
        }
    }
}