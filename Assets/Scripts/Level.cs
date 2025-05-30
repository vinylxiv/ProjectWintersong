// Vinyl 2025.

using UdonSharp;
using UnityEngine;

public class Level : UdonSharpBehaviour
{
    public Transform[] GetSpawns()
    {
        Transform SpawnParentTransform = transform.Find("Spawns");
        Transform[] Result = new Transform[SpawnParentTransform.childCount];
        
        for (int Idx = 0; Idx < SpawnParentTransform.childCount; ++Idx)
        {
            Result[Idx] = SpawnParentTransform.GetChild(Idx);
        }
        
        return Result;
    }
}
