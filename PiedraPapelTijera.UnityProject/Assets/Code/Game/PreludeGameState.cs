using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Parte de la ronda antes de que se puedan sacar figuras. Si alguien saca, pierde la ronda. </summary>
[System.Serializable]
public class PreludeGameState : GameState
{
	Player playerLeft;
	Player playerRight;

	static string preludeAnim = "prelude";

	public string popText = "Go!";
	public float popTextTime = 0.3f;

	public override void Setup(Game game)
	{
		base.Setup(game);

		playerLeft = game.playerLeft;
		playerRight = game.playerRight;
	}


	public override void Start()
	{
		hasOtherPlayerChosed = false;

		// activar players.
		playerLeft.enabled = true;
		playerLeft.animator.SetBool(preludeAnim, true);

		playerRight.enabled = true;
		playerRight.animator.SetBool(preludeAnim, true);

		// Añadir listeners a los players.
		playerLeft.onChose -= (WhenLeftPlay);
		playerLeft.onChose += (WhenLeftPlay);

		playerRight.onChose -= (WhenRightPlay);
		playerRight.onChose += (WhenRightPlay);

		base.Start();
	}

	protected override IEnumerator Routine()
	{
		game.SetPopText(popText);
		yield return new WaitForSeconds(popTextTime);
		game.SetPopText(null);
	}


	// -----------------------------------------------------------------------------

	bool hasOtherPlayerChosed = false;

	void WhenLeftPlay(RockPaperOrScissors figure)
	{
		playerLeft.onChose -= (WhenLeftPlay);

		PlayerPlay(playerLeft);
	}

	void WhenRightPlay(RockPaperOrScissors figure)
	{
		playerRight.onChose -= (WhenRightPlay);

		PlayerPlay(playerRight);
	}

	void PlayerPlay(Player player)
	{
		// Parar animacion.
		player.animator.SetBool(preludeAnim, false);

		/* Pasar de fase directamente en cuanto juegue uno. */
		if (hasOtherPlayerChosed)
			return;
		hasOtherPlayerChosed = true;

		game.EnterPlayState();
		return;

		// Cuando hayan jugado los dos pasamos de fase.
		//if (hasOtherPlayerChosed)
		//{
		//	game.EnterPlayState();
		//	return;
		//}
		//hasOtherPlayerChosed = true;
	}
}
