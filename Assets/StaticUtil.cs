using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUtil
{
    public static Dictionary<Alignment, int> projectileAlignmentLayerMap = new Dictionary<Alignment, int>() {
        { Alignment.PlayerOne, 8 },
        { Alignment.PlayerTwo, 9 } 
    };
}
