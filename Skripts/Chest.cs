using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
   public InventoryItem itemData = new InventoryItem();


    private void OnTriggerEnter2D(Collider2D collider)
    {
        Knight knight = collider.gameObject.GetComponent<Knight>();
        
        if (itemData.CrystallType == CrystallType.Random)
            itemData.CrystallType = (CrystallType)UnityEngine.Random.Range(0, 4);

        if (itemData.Quantity == 0)
            itemData.Quantity = UnityEngine.Random.Range(1, 6);

        GameController.Instance.AddNewInventoryItem(itemData);

        if (knight != null) Destroy(gameObject);
    }

    public enum CrystallType { Random, Red, Green, Blue }
}
