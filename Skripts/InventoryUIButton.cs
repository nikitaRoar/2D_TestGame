using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUIButton : MonoBehaviour
{
   // public InventoryItem itemData;

  //  public Chest.CrystallType crystallType;

    public InventoryUsedCallback callback;

    public float quantity;

    [SerializeField] private Text label;
    [SerializeField] private Text count;
    [SerializeField] private Image m_image;
    [SerializeField] private List<Sprite> sprites;

    public InventoryItem ItemData { get; set; }

    private void Start()
    {
        string spriteNameToSearch = ItemData.CrystallType.ToString();

        m_image.sprite = sprites.Find(x => x.name.Contains(spriteNameToSearch));//ошибка

        label.text = spriteNameToSearch;//название
        count.text = ItemData.Quantity.ToString();//кол-во 
        gameObject.GetComponent<Button>().onClick.AddListener(() => callback(this));
    }


    public void crystalltype()
    {

    }

    public void Quantity() 
    {
       
    }
}

