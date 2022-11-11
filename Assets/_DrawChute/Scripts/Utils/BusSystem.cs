using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BusSystem
{
    public static Action<Mesh> OnDrawEnd;
    public static void CallDrawEnd(Mesh mesh) { OnDrawEnd?.Invoke(mesh); }
}
