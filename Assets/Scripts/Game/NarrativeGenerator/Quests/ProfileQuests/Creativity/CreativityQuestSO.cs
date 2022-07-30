using ScriptableObjects;
using Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.NPCs;
using Game.NarrativeGenerator.EnemyRelatedNarrative;
using Game.NarrativeGenerator.ItemRelatedNarrative;
using ScriptableObjects;

namespace Game.NarrativeGenerator.Quests.QuestGrammarTerminals
{
    public class CreativityQuestSO : QuestSO
    {
        public override string symbolType {
            get { return Constants.CREATIVITY_QUEST; }
        }
        public override Dictionary<string, Func<int,int>> NextSymbolChances
        {
            get {

                if ( nextSymbolChances != null )
                    return nextSymbolChances;

                Dictionary<string, Func<int, int>> creativityQuestWeights = new Dictionary<string, Func<int, int>>();
                creativityQuestWeights.Add( Constants.EXPLORE_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.GOTO_QUEST, Constants.TwoOptionQuestLineWeight );
                creativityQuestWeights.Add( Constants.EMPTY_QUEST, Constants.OneOptionQuestEmptyWeight );
                return creativityQuestWeights;
                
            } 
        }

        public override void DefineQuestSO ( MarkovChain chain, List<QuestSO> questSos, List<NpcSo> possibleNpcSos, TreasureRuntimeSetSO possibleItems, WeaponTypeRuntimeSetSO enemyTypes)
        {
            switch ( chain.GetLastSymbol().symbolType )
            {
                case Constants.EXPLORE_QUEST:
                    CreateAndSaveExploreQuestSo(questSos);
                break;
                case Constants.GOTO_QUEST:
                    CreateAndSaveGotoQuestSo(questSos);
                break;
                default:
                    Debug.LogError("help something went wrong!");
                break;
            }
        }

        public static void CreateAndSaveExploreQuestSo( List<QuestSO> questSos)
        {
            var secretRoomQuest = ScriptableObject.CreateInstance<ExploreQuestSO>();
            secretRoomQuest.Init("Explore Room", false, questSos.Count > 0 ? questSos[^1] : null);
            //TODO initiate data for secretRoomQuest
            if (questSos.Count > 0)
            {
                questSos[^1].Next = secretRoomQuest;
            }

            questSos.Add(secretRoomQuest);
        }

        public static void CreateAndSaveGotoQuestSo( List<QuestSO> questSos)
        {
            var secretRoomQuest = ScriptableObject.CreateInstance<GotoQuestSO>();
            secretRoomQuest.Init("Explore Room", false, questSos.Count > 0 ? questSos[^1] : null);
            //TODO initiate data for secretRoomQuest
            if (questSos.Count > 0)
            {
                questSos[^1].Next = secretRoomQuest;
            }

            questSos.Add(secretRoomQuest);
        }
    }
}