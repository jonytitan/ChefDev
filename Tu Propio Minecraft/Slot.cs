using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//Para usar los metodos IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IDragHandler, IDropHandler debes agregar la libreria EventSystem
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    PlayerInventory inventory;
    PlayerStats stats;
    [SerializeField] public int numSlot;
    [SerializeField] TextMeshProUGUI texUnit;
    [SerializeField] public Image img;

    [Header("Drag and Drop")]
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform rectMove;
    [SerializeField] bool inDrag;
    [SerializeField] bool inUse;

    private Vector2 rectMoveStart;
    public int numSlotCur;

    void Start()
    {
        inventory = PlayerInventory.instance;
        stats = PlayerStats.instance;
        rectMoveStart = rectMove.anchoredPosition;
        numSlotCur = numSlot;
    }

    private void Update()
    {
        UnitItem();
    }

    //Metodo que se ejucutara una vez se presione un boton sobre , asegurate de agregarlo en una funcion OnClick de algun boton en la interface del jugador
    public void UseItem()
    {
        Debug.Log("Aqui hay: " + inventory.items[numSlotCur].name+"; "+ inventory.items[numSlotCur].type);
        if (stats.hungryCurr < 10  && inventory.items[numSlotCur].type == ItemType.meat && inventory.items[numSlotCur].amount == 1)
        {
            stats.AddHungry(3);
            inventory.EmptySlot(numSlotCur, img);
        }
        if (stats.hungryCurr < 10 && inventory.items[numSlotCur].type == ItemType.meat && inventory.items[numSlotCur].amount > 1)
        {
            stats.AddHungry(3);
            inventory.items[numSlotCur].amount -= 1;
        }
    }

    //Verifica constantemente las unidades en el inventario de mi personaje
    private void UnitItem()
    {
        if (inventory.items[numSlot].isFull)
        {
            texUnit.text = "x" + inventory.items[numSlot].amount;
        }
        if (!inventory.items[numSlot].isFull)
        {
            texUnit.text = "";
        }
    }

    //Metodo que valida cuando el mouse pasa adentro de un objeto
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory.items[numSlotCur].isFull)
        {
            //Debug.Log("in");
            transform.localScale = new Vector3(1.15f, 1.15f, 1f);
            img.transform.localScale = new Vector3(1.15f, 1.15f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1.15f, 1.15f, 1f);
        }
    }

    //Metodo que valida cuando el mouse sale de un objeto
    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory.items[numSlotCur].isFull)
        {
            //Debug.Log("out");
            transform.localScale = new Vector3(1f, 1f, 1f);
            img.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    //Metodo que valida cuando se mantiene presionado clic izquierdo sobre un objeto
    public void OnDrag(PointerEventData eventData)
    {
        if (inventory.items[numSlotCur].isFull)
        {
            rectMove.anchoredPosition += eventData.delta/ canvas.scaleFactor;
        }
    }

    //Metodo que valida cuando el mouse deja de presionar clic izquierdo sobre un objeto en particular
    public void OnDrop(PointerEventData eventData)
    {
        int origen = eventData.pointerDrag.GetComponent<Slot>().numSlot;
        //Debug.Log("origen: " + origen);
        //Debug.Log("destino: " + numSlot);
        if (origen != numSlot && !inventory.items[numSlot].isFull)
        {
            //Debug.Log("asdf");
            inventory.items[numSlot].isFull = inventory.items[origen].isFull;
            inventory.items[numSlot].amount = inventory.items[origen].amount;
            inventory.items[numSlot].type = inventory.items[origen].type;
            inventory.items[numSlot].name = inventory.items[origen].name;
            inventory.items[numSlot].slotSprite.GetComponent<Slot>().img.sprite = inventory.items[origen].slotSprite.GetComponent<Slot>().img.sprite;
            inventory.items[numSlot].slotSprite.GetComponent<Slot>().img.enabled = true;
            inventory.EmptySlot(origen, inventory.items[origen].slotSprite.GetComponent<Slot>().img);
        }
    }

    //Metodo que valida cuando el mouse deja de presionar clic izquierdo haya o no haga movido nada
    public void OnEndDrag(PointerEventData eventData)
    {
        rectMove.anchoredPosition = rectMoveStart;
    }
}
