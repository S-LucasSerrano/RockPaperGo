using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[Space]
	public FigureData rock;
	public FigureData paper;
	public FigureData scissors;
	private FigureData currentFigure;

	[Space]
	public float cooldown = 0.2f;
	private bool canPlay = true;

	[Space]
	public Image handImage;
	public Animator animator;
	public ParticleSystem winningParticles;

	[Space]
	[SerializeField] Sprite winningSprite = null;
	[SerializeField] Sprite losingSprite = null;
	[SerializeField] Sprite finalWinSprite = null;

	[Space]
	[SerializeField] AudioSource popSound = null;
	[SerializeField] Vector2 minMaxPitch = new Vector2(0.9f, 1.1f);


	[HideInInspector] public RockPaperOrScissors chosenFigure = RockPaperOrScissors.None;
	[HideInInspector] public UnityAction<RockPaperOrScissors> onChose;

	[HideInInspector] public int score = 0;

	[Space]
	[SerializeField] Image scoreImage = null;
	[SerializeField] Sprite oneScoreSprite = null;
	[SerializeField] Sprite twoScoreSprite = null;
	[SerializeField] Sprite threeScoreSprite = null;
	[SerializeField] Animator scoreAnimator;


	// -------------------------------------------------------------------------------

	private void OnEnable()
	{
		canPlay = true;

		Pop();
		handImage.sprite = rock.sprite;
		chosenFigure = RockPaperOrScissors.None;
		currentFigure = null;

		UpdateScoreUI();
	}

	private void Update()
	{
		// checkear cuando se pulsan las teclas.
		CheckKey(rock);
		CheckKey(paper);
		CheckKey(scissors);
	}

	private void CheckKey(FigureData figure)
	{
		if (Input.GetKeyDown(figure.key))
			Choose(figure);
	}


	// -------------------------------------------------------------------------------

	public void Choose(FigureData figure)
	{
		if (!canPlay || currentFigure == figure)
			return;

		// muestra el sprite
		Pop();
		handImage.sprite = figure.sprite;

		// sonido
		float pitch = Random.Range(minMaxPitch.x, minMaxPitch.y);
		popSound.pitch = pitch;
		popSound.Play();

		// Invocar un evento de que el player ha elegido.
		RockPaperOrScissors rps = RockPaperOrScissors.None;
		if (figure == rock)
			rps = RockPaperOrScissors.Rock;
		else if (figure == paper)
			rps = RockPaperOrScissors.Paper;
		else if (figure == scissors)
			rps = RockPaperOrScissors.Scissors;
		chosenFigure = rps;

		onChose?.Invoke(rps);

		currentFigure = figure;

		// esperar el cooldown
		StartCoroutine(CooldownRoutine());
	}

	private IEnumerator CooldownRoutine()
	{
		canPlay = false;
		yield return new WaitForSeconds(cooldown);
		canPlay = true;
	}

	public void WinRound()
	{
		// Poner animacion de ganar ronda.
		handImage.sprite = winningSprite;
		Pop();
		winningParticles.Play();

		// Actualiza la ui de puntos.
		scoreAnimator.SetTrigger("pop");
		score++;
		UpdateScoreUI();

		animator.SetBool("prelude", false);
	}

	public void LoseRound()
	{
		// poner animacion de perder ronda
		handImage.sprite = losingSprite;
		Pop();

		animator.SetBool("prelude", false);
	}

	public void WinGame()
	{
		// poner icono de ganar partida.
		handImage.sprite = finalWinSprite;
		// pop
		Pop();
		// estrellas
		winningParticles.Play();

		scoreImage.gameObject.SetActive(false);

		animator.SetBool("prelude", false);
	}

	public void LoseGame()
	{
		// poner animacion de perder partida.
		gameObject.SetActive(false);

		animator.SetBool("prelude", false);
	}

	public void Pop()
	{
		animator.SetTrigger("pop");
	}

	void UpdateScoreUI()
	{
		if (score == 0)
		{
			scoreImage.sprite = null;
			scoreImage.enabled = false;
			return;
		}

		scoreImage.enabled = true;

		if (score == 1)
			scoreImage.sprite = oneScoreSprite;
		else if (score == 2)
			scoreImage.sprite = twoScoreSprite;
		else if (score == 3)
			scoreImage.sprite = threeScoreSprite;
	}


	// -------------------------------------------------------------------------------

	[System.Serializable]
	public class FigureData
	{
		public Sprite sprite;
		public Button button;
		public KeyCode key = KeyCode.Alpha1;
	}
}
