using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CheckState : GameState
{
	[SerializeField] Player playerLeft = null; 
	[SerializeField] Player playerRight = null; 


	public override void Setup(Game game)
	{
		base.Setup(game);
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Stop()
	{
		base.Stop();
	}

	protected override IEnumerator Routine()
	{
		// Desactivamos la barra.
		// Esperamos un pelin.
		// Se comprueba que ha jugado cada uno y quien gana.
		// Si los dos son iguales, empate.
		// piedra > tijera > papel > piedra.
		// Todo gana a no haber jugado nada.
		// Se ponen animaciones de perder o ganas.

		// Se asignan y muestran puntos.
		// Si un jugador tiene tres puntos gana el juego.
		// Si no se espera un peli y se repite.

		// Quiza todo esto necesita estar en game.

		yield return null;
	}
}
