using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int number {  get; private set; }
    public TileCell cell { get; private set; }
    public TileState state { get; private set; }
    public bool locked {  get;  set; }

    private Image background;
    private TextMeshProUGUI text;
    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetState(TileState state,int number)
    {
        this.state = state;
        this.number = number;
        background.color = state.BackgroundColor;
        
        text.color = state.TextColor;
        text.text=number.ToString();
    }
    public void spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile=null;
        }
        this.cell = cell;
        this.cell.tile = this;

        transform.position=cell.transform.position;
    }
    public void MoveTO(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;      
        StartCoroutine(Animate(cell.transform.position,false));
    }
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = null;
        cell.tile.locked = true;
       
        StartCoroutine(Animate(cell.transform.position, true));
    }
    private IEnumerator Animate(Vector3 to, bool mergeing)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        if (mergeing)
        {
            Destroy(gameObject);
        }

    }

}

  


