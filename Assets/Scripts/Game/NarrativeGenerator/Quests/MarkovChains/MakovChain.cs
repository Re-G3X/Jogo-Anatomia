using UnityEngine;
using System.Collections.Generic;
using Game.NarrativeGenerator;
using Game.NarrativeGenerator.Quests.QuestTerminals;

namespace Game.NarrativeGenerator.Quests
{
    public class MarkovChain
    {
        public Symbol symbol;
        public SymbolType symbolType;
        public int symbolNumber = 0;

        public MarkovChain ()
        {
            symbol = new NonTerminalQuest();
            symbolType = SymbolType.Start;
            symbolNumber = 0;
        }

        public void SetSymbol ( SymbolType _symbol )
        {
            symbolType = _symbol;
            symbolNumber++;
            switch ( _symbol )
            {
                case SymbolType.Kill:
                    this.symbol = new Kill();
                break;
                case SymbolType.Talk:
                    this.symbol = new Talk();
                break;
                case SymbolType.Get:
                    this.symbol = new Get();
                break;
                case SymbolType.Explore:
                    this.symbol = new Explore();
                break;
                case SymbolType.kill:
                    this.symbol = ScriptableObject.CreateInstance<KillQuestSO>();
                break;
                case SymbolType.talk:
                    this.symbol = ScriptableObject.CreateInstance<TalkQuestSO>();
                break;
                case SymbolType.empty:
                    this.symbol = ScriptableObject.CreateInstance<EmptyQuestSO>();
                break;
                case SymbolType.get:
                    this.symbol = ScriptableObject.CreateInstance<GetQuestSO>();
                break;
                case SymbolType.drop:
                    this.symbol = ScriptableObject.CreateInstance<DropQuestSO>();
                break;
                case SymbolType.item:
                    this.symbol = ScriptableObject.CreateInstance<ItemQuestSO>();
                break;
                case SymbolType.secret:
                    this.symbol = ScriptableObject.CreateInstance<SecretRoomQuestSO>();
                break;
                default:
                    Debug.LogError("Symbol type not found!");
                break;
            }
        }

        public Symbol GetSymbol ()
        {
            return this.symbol;
        }
    }
}