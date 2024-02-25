using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
	protected Game game;
	protected Coroutine routine = null;

	public virtual void Setup(Game game)
	{
		this.game = game;
	}

	public virtual void Start()
	{
		// iniciar corrutina.
		if (routine != null) { game.StopCoroutine(routine); }
		routine = game.StartCoroutine(Routine());
	}
	public virtual void Stop()
	{
		// parar corrutina.
		if (routine != null) { game.StopCoroutine(routine); }
	}

	protected virtual IEnumerator Routine()
	{
		yield return null;
	}
}
