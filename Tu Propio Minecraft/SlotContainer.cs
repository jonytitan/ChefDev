using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class SlotContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    PlayerInventory inventory;
    [SerializeField] ChestControl chest;
    [SerializeField] public int numSlot;
    [SerializeField] TextMeshProUGUI itemUnits;
    [SerializeField] Image itemImage;
    private void Start()
    {
        inventory = PlayerInventory.instance;
    }

    private void Update()
    {
        if (chest.items[numSlot].isFull)
        {
            itemImage.enabled = true;
            itemImage.sprite = chest.items[numSlot].sprite;
            itemUnits.text = "x" + chest.items[numSlot].amount;
        }
        else
        {
            itemImage.enabled = false;
            itemImage.sprite = null;
            itemUnits.text = "";
        }
    }
    //Metodo que se ejucutara una vez se presione un boton, asegurate de agregarlo en una funcion OnClick de algun botonen en la interface de un objeto
    public void UseItem()
    {
        if (chest.items[numSlot].isFull)
        {
            //Debug.Log("Aqui hay: " + chest.items[numSlot].name);

            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (inventory.items[i].isFull == false)
                {
                    //Debug.Log("item añadido");
                    inventory.items[i].isFull = chest.items[numSlot].isFull;
                    inventory.items[i].amount = chest.items[numSlot].amount;
                    inventory.items[i].type = chest.items[numSlot].type;
                    inventory.items[i].name = chest.items[numSlot].name;
                    inventory.items[i].slotSprite.GetComponent<Slot>().img.sprite = itemImage.sprite;
                    inventory.items[i].slotSprite.GetComponent<Slot>().img.enabled = true;
                    //limpiar espacio una vez pasado al player
                    chest.EmptySlot(numSlot, itemImage);
                    break;
                }
                var sub = inventory.items[i].amount + chest.items[numSlot].amount;
                //usado en caso sea menor al limite
                if (inventory.items[i].isFull == true && inventory.items[i].name == chest.items[numSlot].name && sub <= 64)
                {
                    Debug.Log("item estakeado");
                    inventory.items[i].amount = sub;
                    chest.EmptySlot(numSlot, itemImage);
                    break;
                }
            }
        }
    }
    //Metodo que valida cuando el mouse pasa adentro de un objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (chest.items[numSlot].isFull)
        {
            //Debug.Log("in");
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            itemImage.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
    }
    //Metodo que valida cuando el mouse sale de un objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (chest.items[numSlot].isFull)
        {
            //Debug.Log("in");
            transform.localScale = new Vector3(1f, 1f, 1f);
            itemImage.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
