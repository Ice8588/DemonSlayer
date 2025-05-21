using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    None,
    WeakPoint,
    AttackPoint
}

public enum DirectionIndex
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7
}

// public interface IDirectionProvider
// {
//     // Get single direction type
//     public DirectionType GetDirectionType(int directionIndex);
    
//     // Get all direction types at once
//     public DirectionType[] GetAllDirectionTypes();

//     // Interact with a specific direction
//     // Returns true if the interaction was successful
//     public bool InteractWithDirection(int directionIndex);
// }
