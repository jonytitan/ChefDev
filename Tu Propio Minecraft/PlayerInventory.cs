using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Tipos de objetos que existen en mi mundo
public enum ItemType
{
    none,
    log,
    rock,
    meat
}

public class PlayerInventory : MonoBehaviour
{
    //Se instancia player inventory para usarlo en otros scripts
    public static PlayerInventory instance;
    public Item[] items;

    private void Awake()
    {
        instance = this;
    }
    //Metodo usado para vaciar un Item[] una vez este vacio
    public void EmptySlot(int numSlot, Image img)
    {
        items[numSlot].isFull = false;
        items[numSlot].amount = 1;
        items[numSlot].type = ItemType.none;
        items[numSlot].name = null;
        img.sprite = null;
        img.enabled = false;
    }
}
//Estructura para la clase Player - debe ser Serializable como esta aqui mismo
[System.Serializable]
public class Item
{
    public bool isFull;
    public int amount;
    public ItemType type;
    public string name;
    public GameObject slotSprite;
}
//Estructura para la clase Contenedor - debe ser Serializable como esta aqui mismo
[System.Serializable]
public class ItemContainer
{
    public bool isFull;
    public int amount;
    public ItemType type;
    public string name;
    public Sprite sprite;
    public GameObject slotSprite;
}