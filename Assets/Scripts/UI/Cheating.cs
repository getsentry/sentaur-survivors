using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Cheating : MonoBehaviour
{
    public void CheatLittle()
    {
        throw new NullReferenceException("Cheat not found!");
    }

    public void CheatLot()
    {
        crash_in_c();
    }
    
    // CPlugin.c
    [DllImport("__Internal")]
    private static extern void crash_in_c();
}