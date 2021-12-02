using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class InventoryItem 
{
   [SerializeField] public float quantity;
   [SerializeField] public Chest.CrystallType crystallType;

    public int Quantity { get; set; }

    public Chest.CrystallType CrystallType { get;  set; }
}
