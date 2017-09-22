using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCatcher : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            GameStateManager.setGameState(GameState.GameOver);
            Application.LoadLevelAsync("GameOverScene");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("End"))
        {
            GameStateManager.setGameState(GameState.GameOver);
            Application.LoadLevelAsync("SuccessGameOver");
        }
    }

        void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            GameStateManager.setGameState(GameState.GameOver);
            Application.LoadLevelAsync("GameOverScene");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("End"))
        {
            Debug.Log("END");
            GameStateManager.setGameState(GameState.GameOver);
            Application.LoadLevelAsync("SuccessGameOver");
        }
    }
}
