﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop_ter{
    
    public void choose(Manager m){
        Quest quest = new Quest();
        quest.tipo = 5;
        quest.n = Random.Range(0, 10);
        quest.c1 = -1;
        quest.c2 = -1;
        quest.parent = -1;

        m.graph.Add(quest);

        //m.chain.Add(6);
    }
}
