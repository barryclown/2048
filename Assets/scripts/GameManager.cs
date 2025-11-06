using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameover;
    public TextMeshProUGUI nowscore;
    public TextMeshProUGUI highscore;
    public int score;
    void Start()
    {
        NewGame();
    }
    public void NewGame()
    {
        SetScore(0);
        highscore.text =LoadHighScore().ToString();
        //³z©ú
        gameover.alpha = 0;
        gameover.interactable = false;
        board.ClearBoard();
        board.CreatTile();
        board.CreatTile();
        board.enabled = true;
        
    }
    public void GameOver()
    {
        board.enabled=false;
        gameover.interactable = true;
        StartCoroutine(fade(gameover,1f,1f));
        
    }
    private IEnumerator fade(CanvasGroup group,float to,float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.1f;

        float from = group.alpha;
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
        nowscore.text = score.ToString();

        SaveLowScore();
    }
    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("history", 0);
    }
    private void SaveLowScore() 
    {
        int high =LoadHighScore();
        if (score > high)
        {
            PlayerPrefs.SetInt("history", score);
        }
    }
    public void increaseScore(int number)
    {
        SetScore(score+number);
    }



}
