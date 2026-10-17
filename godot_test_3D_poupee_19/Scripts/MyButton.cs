using Godot;
using System;

public partial class MyButton : Button
{
	// Glisser-déposer le noeud de l'interface à controler dans l'inspecteur
	[Export] public PackedScene GuiScene;
		
	private Node _guiInstance;
	
	public override void _Ready()
	{
		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		if (_guiInstance == null && GuiScene != null)
		{
			_guiInstance = GuiScene.Instantiate();
			
			// on l'ajoute à la scène actuelle (elle se superpose au jeu)
			GetTree().CurrentScene.AddChild(_guiInstance);
			
			// on s'assure qu'elle démarre visible puis qu'on vient de cliquer
			if (_guiInstance is Control controlElement)
			{
				controlElement.Visible = true;
			}
			if(_guiInstance is MyGuiScript guiScript)
			{
				guiScript.UpdateDisplay();
			}
			
			return;
		}
		
		// Si elle existe déjà, on alterne sa visibilité
		if (_guiInstance is Control guiControl)
		{
			guiControl.Visible = !guiControl.Visible;
			
			if(guiControl.Visible && _guiInstance is MyGuiScript guiScript)
			{
				guiScript.UpdateDisplay();
			}
		}
	}
}
