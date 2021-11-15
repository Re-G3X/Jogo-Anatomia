using System.Collections.Generic;
using System;
using Game.NarrativeGenerator.Quests;
using Game.NarrativeGenerator;
using UnityEngine;
using Util;

public class NonTerminalQuest : Symbol
{
    public Dictionary<string, Func<float,float>> nextSymbolChance {get; set;}
    public bool canDrawNext {get; set;}
    // Symbol symbol = new NonTerminalQuest();
    protected float r;
    protected int lim;
    protected float maxQuestChance;
    protected Dictionary<string, int> questWeightsbyType;
    private static readonly int QUEST_LIMIT = 2;

    public NonTerminalQuest()//(int lim, Dictionary<string, int> questWeightsbyType)
    {
        canDrawNext = true;
        // this.questWeightsbyType = questWeightsbyType;
        // this.lim = lim;
    }

    public void Option(Manager m)
    {
        DrawQuestType();
        DefineNextQuest(m);
    }

    protected virtual void DefineNextQuest(Manager m){}

    private void DrawQuestType()
    {
        r = ((questWeightsbyType[Constants.TALK_QUEST] +
            questWeightsbyType[Constants.GET_QUEST] * 2 +
            questWeightsbyType[Constants.KILL_QUEST] * 3 +
            questWeightsbyType[Constants.EXPLORE_QUEST] * 4) / 16) *
        UnityEngine.Random.Range(0f, 3f);
        if (lim == QUEST_LIMIT)
        {
            r = maxQuestChance;
        }
        lim++;
    }
    
    void Symbol.SetDictionary( Dictionary<string, Func<float,float>> _nextSymbolChances  )
    {
        nextSymbolChance = _nextSymbolChances;
    }

    void Symbol.SetNextSymbol(MarkovChain chain)
    {
        float chance = (float) UnityEngine.Random.Range( 0, 100 ) / 100 ;
        Debug.Log(chance);
        foreach ( Func<float,float> nextSymbol in nextSymbolChance.Keys )
        {
            if ( nextSymbol(chain.symbolNumber) > chance )
            {
                SymbolType _nextSymbol;
                nextSymbolChance.TryGetValue( nextSymbol, out _nextSymbol );
                chain.SetSymbol( _nextSymbol );
            }
        }
    }
}