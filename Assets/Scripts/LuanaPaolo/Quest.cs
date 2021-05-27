﻿using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest")]
public class Quest : ScriptableObject
{
    public int tipo;
    public int n1, n2, n3;
    public int parent = -1, c1 = -1, c2 = -1;

    public override string ToString()
    {
        return "Type="+tipo+"n1="+n1+"n2="+n2+"n3="+n3;
    }
}
