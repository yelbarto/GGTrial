using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Block")]
public class Block : ScriptableObject
{
    public int modular;
    public int[] level1;
    public int[] level2;
    public int[] level3;
    public int levels;
    public Vector3 originalScale;
    public int cubeAmount;
    public int place;
}
