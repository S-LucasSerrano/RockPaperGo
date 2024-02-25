using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayGameState : GameState
{
	[SerializeField] float gameTime = 1;
	[SerializeField] AnimationCurve gameTimeCurve = AnimationCurve.Linear(0, 0, 1, 1);
	[SerializeField] Image gameBar = null;
	[SerializeField] GameObject[] barsToTurnOff = null;

	[Space]
	[SerializeField] float timeShowingPopText = 0.2f;
	[SerializeField] float timeBeforeShowingRoundWinner = 0.3f;
	[SerializeField] float timeShowingRoundWinner = 1;
	[SerializeField] float timeShowingTie = 1;
	[SerializeField] float timeBeforeShowingGameWinner = 2;

	[Space]
	[SerializeField] SoundGroup roundWiningSound = null;
	[SerializeField] SoundGroup tieSound = null;
	[SerializeField] SoundGroup gameWinningsound = null;
	[SerializeField] AudioSource backgroundMusic = null;


	// -----------------------------------------------------------------------------

	public override void Setup(Game game)
	{
		base.Setup(game);

		gameBar.fillAmount = 0;
		SetBars(false);
	}

	public override void Start()
	{
		game.StartCoroutine(PopRoutine());

		// Activar la barra.
		SetBars(true);

		base.Start();	
	}

	IEnumerator PopRoutine()
	{
		game.SetPopText("Go!");
		yield return new WaitForSeconds(timeShowingPopText);
		game.SetPopText(null);
	}

	public override void Stop()
	{
		base.Stop();
		gameBar.fillAmount = 0;
		SetBars(false);
	}

	protected override IEnumerator Routine()
	{
		float counter = 0;
		while (counter < gameTime)
		{
			// Vamos actualizando la barra.
			float progress = counter / gameTime;
			progress = gameTimeCurve.Evaluate(progress);
			gameBar.fillAmount = progress;

			counter += Time.deltaTime;
			yield return null;
		}

		Player winningPlayer;
		Player losingPlayer;
		CheckWinningPlayer(out winningPlayer, out losingPlayer);

		SetBars(false);

		// bloqueamos player
		game.playerLeft.enabled = false;
		game.playerRight.enabled = false;

		// EMPATE
		if (winningPlayer == null || losingPlayer == null)
		{
			tieSound.Play();

			// Mostrar texto de empate.
			game.SetPopText("Tie!");
			yield return new WaitForSeconds(timeShowingTie);
			game.SetPopText(null);

			// Reiniciamos.
			game.EnterPreludeState();

			yield break;
		}

		// NO EMPATE
		yield return new WaitForSeconds(timeBeforeShowingRoundWinner);

		// Le decimos a cada player so pierde o gana.
		losingPlayer.LoseRound();
		winningPlayer.WinRound();

		// sonido
		roundWiningSound.Play();

		// VICTORIA DEFINITIVA
		if (winningPlayer.score >= game.scoreToWin)
		{

			yield return new WaitForSeconds(timeBeforeShowingGameWinner);

			roundWiningSound.Stop();
			backgroundMusic.Stop();
			gameWinningsound.Play();

			game.PlayerWinsGame(winningPlayer);

			yield break;

		}

		// Si no, reiniciamos.
		yield return new WaitForSeconds(timeShowingRoundWinner);
		game.EnterPreludeState();
	}

	void CheckWinningPlayer(out Player winningPlayer, out Player losingPlayer)
	{
		winningPlayer = null;
		losingPlayer = null;

		var left = game.playerLeft.chosenFigure;
		var right = game.playerRight.chosenFigure;

		// Empate si han jugado lo mismo.
		if (left == right)
			return;

		// Si uno no ha jugado nada, pierde el que no ha jugado nada.
		if (left == RockPaperOrScissors.None)
		{
			losingPlayer = game.playerLeft;
			winningPlayer = game.playerRight;
			return;
		}
		else if (right == RockPaperOrScissors.None)
		{
			losingPlayer = game.playerRight;
			winningPlayer = game.playerLeft;
			return;
		}

		// Comprobar quien gana. Piedra > Papel > Tijera
		else if (left == RockPaperOrScissors.Rock)
		{
			if (right == RockPaperOrScissors.Paper)
			{
				// gana derecha
				winningPlayer = game.playerRight;
			}
			else if (right == RockPaperOrScissors.Scissors)
			{
				// gana izquierda
				winningPlayer = game.playerLeft;
			}
		}
		else if (left == RockPaperOrScissors.Paper)
		{
			if (right == RockPaperOrScissors.Scissors)
			{
				// gana derecha
				winningPlayer = game.playerRight;
			}
			else if (right == RockPaperOrScissors.Rock)
			{
				// gana izquierda
				winningPlayer = game.playerLeft;
			}
		}
		else if (left == RockPaperOrScissors.Scissors)
		{
			if (right == RockPaperOrScissors.Rock)
			{
				// gana derecha
				winningPlayer = game.playerRight;
			}
			else if (right == RockPaperOrScissors.Paper)
			{
				// gana izquierda
				winningPlayer = game.playerLeft;
			}
		}

		if (winningPlayer == game.playerLeft)
			losingPlayer = game.playerRight;
		else
			losingPlayer = game.playerLeft;
	}

	void SetBars(bool value)
	{
		foreach (var bar in barsToTurnOff)
			bar.SetActive(value);
	}
}
