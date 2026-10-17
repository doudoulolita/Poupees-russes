using Godot;
using System;

public partial class guiScript : Control
{
	[Export] public Label ScoreLabel;
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void UpdateDisplay()
	{
		var joueur = GetTree().CurrentScene.FindChild("poupee_01", true, false);
		
		int currentScore = poupee_01.Instance.Score;
		
		if(joueur is poupee_01 poupee_01)
		{
			int currentScore = poupee_01.Score;
			
			if(ScoreLabel !=null)
			{
				ScoreLabel.Text = $"Score : {currentScore}";
			}
		}

	}
}
