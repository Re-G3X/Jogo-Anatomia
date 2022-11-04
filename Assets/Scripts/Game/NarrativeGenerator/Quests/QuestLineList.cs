﻿using System;
using System.Collections.Generic;
using Game.LevelGenerator.LevelSOs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using Game.NarrativeGenerator.NpcRelatedNarrative;
using Game.NPCs;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;

namespace Game.NarrativeGenerator.Quests
{
    [CreateAssetMenu(fileName = "QuestLineList", menuName = "Overlord-Project/QuestLineList", order = 0)]
    [Serializable]
    public class QuestLineList : ScriptableObject, SaveableGeneratedContent
    {
        [field: SerializeField] public List<QuestLine> QuestLines { get; set; }
        [field: SerializeField] public List<EnemySO> EnemySos { get; set; }
        [field: SerializeField] public List<NpcSo> NpcSos { get; set; }
        [field: SerializeField] public List<ItemSo> ItemSos { get; set; }
        [field: SerializeField] public List<DungeonFileSo> DungeonFileSos { get; set; }


        //[field: SerializeField] public QuestNpcsParameters NpcParametersForQuestLines { get; set; }
        [field: SerializeField] public QuestItemsParameters ItemParametersForQuestLines { get; set; }
        [field: SerializeField] public QuestDungeonsParameters DungeonParametersForQuestLines { get; set; }
        [field: SerializeField] public QuestEnemiesParameters EnemyParametersForQuestLines { get; set; }
        [field: SerializeField] public PlayerProfile TargetProfile { get; set; }

        public void Init()
        {
            QuestLines = new List<QuestLine>();
            DungeonFileSos = new List<DungeonFileSo>();
            EnemySos = new List<EnemySO>();
            NpcSos = new List<NpcSo>();
            ItemSos = new List<ItemSo>();
            DungeonParametersForQuestLines = new QuestDungeonsParameters();
            EnemyParametersForQuestLines = new QuestEnemiesParameters();
            ItemParametersForQuestLines = new QuestItemsParameters();
            //NpcParametersForQuestLines = new QuestNpcsParameters();
        }
        public void AddQuestLine(QuestLine questLine)
        {
            QuestLines.Add(questLine);
        }

        public QuestLine GetRandomQuestLine()
        {
            var random = RandomSingleton.GetInstance().Random;
            return QuestLines[random.Next(QuestLines.Count)];
        }
        
        public void SaveAsset(string directory)
        {
#if UNITY_EDITOR
            const string questLineName = "QuestLineList";
            var fileName = directory + questLineName + ".asset";
            if (!AssetDatabase.GUIDFromAssetPath(fileName).Empty()) return;
            CreateAssetsForDungeons(directory);
            CreateAssetsForEnemies(directory);
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            AssetDatabase.CreateAsset(this, uniquePath);
            var newFolder = Constants.SeparatorCharacter + TargetProfile.ToString();
            if (!AssetDatabase.IsValidFolder(directory + newFolder))
            {
                AssetDatabase.CreateFolder(directory, newFolder);
            }
            directory += Constants.SeparatorCharacter + newFolder;
            foreach (var questLine in QuestLines)
            {
                questLine.SaveAsset(directory);
            }
#endif
        }
        
        public void CreateAssetsForDungeons(string directory)
        {
            foreach (var dungeon in DungeonFileSos)
            {
                dungeon.SaveAsset(directory);
            }
        }
        
        public void CreateAssetsForEnemies(string directory)
        {
            foreach (var enemy in EnemySos)
            {
                enemy.SaveAsset(directory);
            }
        }


        public void CalculateDifficultyFromProfile(PlayerProfile playerProfile)
        {
            EnemyParametersForQuestLines.CalculateDifficultyFromProfile(playerProfile);
        }

        public void CalculateMonsterFromQuests()
        {
            EnemyParametersForQuestLines.CalculateMonsterFromQuests(QuestLines);
        }

        /*public void CalculateNpcsFromQuests()
        {
            NpcParametersForQuestLines.CalculateNpcsFromQuests(QuestLines);
        }*/

        public void CalculateItemsFromQuests()
        {
            ItemParametersForQuestLines.CalculateItemsFromQuests(QuestLines);
        }

        public void CalculateDungeonParametersFromQuests(float explorationPreference)
        {
            DungeonParametersForQuestLines.CalculateDungeonParametersFromQuests(QuestLines, explorationPreference);
        }
    }
}