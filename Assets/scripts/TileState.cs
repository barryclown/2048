using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//這定義了一個可在 Unity 編輯器中建立的 TileState 物件

[CreateAssetMenu(menuName ="tile state")]

public class TileState : ScriptableObject
{
    public Color BackgroundColor;
    public Color TextColor;
}
