using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    GameState currentState = null;

	[Space]
	public Player playerLeft;
	public Player playerRight;

	[Space]
	public int scoreToWin = 3;

	[Space]
	[SerializeField] TMPro.TextMeshProUGUI popText;
	Animator popTextAnimator = null;

	[SerializeField] GameObject leftWinPanel = null;
	[SerializeField] GameObject righWinPanel = null;

    [Space][SerializeField] PreludeGameState prelude = new PreludeGameState();
    [Space][SerializeField] PlayGameState play = new PlayGameState();

    void Start()
    {
        // setup.
        prelude.Setup(this);
        play.Setup(this);

		SetPopText(null);

		leftWinPanel.SetActive(false);
		righWinPanel.SetActive(false);

		// Iniciar.
		EnterPreludeState();
    }


	// -----------------------------------------------------------------------------

    public void EnterPreludeState()
    {
		if(currentState != null) currentState.Stop();

		currentState = prelude;
		prelude.Start();
	}

	public void EnterPlayState()
    {
		if(currentState != null) currentState.Stop();

		currentState = play;
		play.Start();
	}


	// -----------------------------------------------------------------------------

	public void SetPopText(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			popText.text = "";
			return;
		}

		if (popTextAnimator == null)
			popTextAnimator = popText.GetComponent<Animator>();

		popText.text = message;
		popTextAnimator.SetTrigger("pop");
	}

	public void PlayerWinsGame(Player winningPlayer)
	{
		Player losingPlayer = null;

		if (winningPlayer == playerLeft)
		{
			leftWinPanel.SetActive(true);
			losingPlayer = playerRight;
		}
		else if (winningPlayer == playerRight)
		{
			righWinPanel.SetActive(true);
			losingPlayer = playerLeft;
		}

		winningPlayer?.WinGame();
		losingPlayer?.LoseGame();
	}
}
