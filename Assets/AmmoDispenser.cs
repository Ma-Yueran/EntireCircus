using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDispenser : MonoBehaviour
{
    public List<AmmoData> ammoData = new List<AmmoData>();
    public List<int> ammoAppearanceOdds = new List<int>();

    public AmmoData nullAmmoData;

    public AmmoData GetAmmoData()
    {
        return ammoData[Random.Range(0, ammoData.Count)];
    }
}
