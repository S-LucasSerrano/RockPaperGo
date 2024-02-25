using UnityEngine;

public class Tuto : MonoBehaviour
{
	[Space]
	[SerializeField] Sprite initialSprite = null;
	[Space]
	[SerializeField] PlayerTutorialData leftPlayer = new();
	[SerializeField] PlayerTutorialData rightPlayer = new();

	const string preludeAnim = "prelude";


	private void Start()
	{
		leftPlayer.Setup(this);
		rightPlayer.Setup(this);
	}

	[System.Serializable]
	public class PlayerTutorialData
	{
		public Player player = null;

		[Space]
		public Animator rockButton = null;
		public Animator paperButton = null;
		public Animator scissorsButton = null;

		public void Setup(Tuto tuto)
		{
			player.animator.SetBool(preludeAnim, true);
			//player.handImage.sprite = tuto.initialSprite;

			player.onChose += OnPlay;
		}

		void OnPlay(RockPaperOrScissors figure)
		{
			player.animator.SetBool(preludeAnim, false);

			switch (figure)
			{
				case RockPaperOrScissors.Rock:
					rockButton.SetTrigger("pop");
					break;

				case RockPaperOrScissors.Paper:
					paperButton.SetTrigger("pop");
					break;

				case RockPaperOrScissors.Scissors:
					scissorsButton.SetTrigger("pop");
					break;

				default:
					break;
			}
		}
	}
}
