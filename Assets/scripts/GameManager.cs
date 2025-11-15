using System.Collections;
using System.Collections.Generic;
using TMPro;                  
using Unity.Mathematics;      
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //遊戲關卡控制器
    public TileBoard board;                  
    public CanvasGroup gameOver;             
    public TextMeshProUGUI nowScore;          
    public TextMeshProUGUI highScore;         
    public int score;                          

    void Start()
    {
        NewGame();                           
    }

    public void NewGame()
    {
        SetScore(0);                           
        highScore.text = LoadHighScore().ToString(); 

    
        gameOver.alpha = 0;
        gameOver.interactable = false;

        board.ClearBoard();                   
        board.CreatTile();                     
        board.CreatTile();                      
        board.enabled = true;                   
    }

    public void GameOver()
    {
        board.enabled = false;                  
        gameOver.interactable = true;          
        StartCoroutine(Fade(gameOver, 1f, 1f)); 
    }

    // 淡入淡出 Coroutine
    private IEnumerator Fade(CanvasGroup group, float to, float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.1f;                 
        float from = group.alpha;              

        // 逐幀漸變透明度
        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(from, to, elapsed / duration); 
            elapsed += Time.deltaTime;        
            yield return null;                 
        }
        group.alpha = to;                      
    }

    private void SetScore(int score)
    {
        this.score = score;                  
        nowScore.text = score.ToString();      
        SaveLowScore();                       
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("history", 0);
    }

    private void SaveLowScore()
    {
        int high = LoadHighScore();           
        if (score > high)                    
        {
            PlayerPrefs.SetInt("history", score); 
        }
    }

    public void IncreaseScore(int number)
    {
        SetScore(score + number);             
    }
}
