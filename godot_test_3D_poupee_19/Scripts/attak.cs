using Godot;
using System;



public partial class attak : CharacterBody3D
{	
	// vitesse minimum en m/s
	[Export]
	public int MinSpeed { get; set; } = 6;
	
	[Export]
	public int MaxSpeed { get; set; } = 10;
	
	[Export]
	public float Gravity = 20.0f;
	
	private Vector3 _directionHorizontale;
	private float _vitesseDefinie;
	private float _vitesseVerticale = 0.0f;
	
	// vitesse maximum en m/s


	public override void _PhysicsProcess(double delta)
	{
		Vector3 mouvementHorizontal = _directionHorizontale * _vitesseDefinie;
		
		// vérifie si le personnage n'est pas au sol
		if(!IsOnFloor())
		{
			// applique la gravité vers le bas (axe Y)
			_vitesseVerticale -= Gravity * (float)delta;
			
			if (_vitesseVerticale < -30.0f) {_vitesseVerticale = -30.0f;}
		}
		else
		{
			_vitesseVerticale = 0f; // on stoppe au sol
		}
		
		// combine mouvement horizontal et gravité dans Velocity
		Velocity = new Vector3(mouvementHorizontal.X, _vitesseVerticale, mouvementHorizontal.Z);
		
		MoveAndSlide();
	}
	
		public void Initialize(Vector3 startPosition, Vector3 playerPosition)
	{
		GlobalPosition = startPosition;
		// positionne la fleur à sa position de départ startPosition
		// et la tourne vers la position du joueur playerPosition pour qu'elle regarde le joueur
		LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
		
		// tourne la fleur de manière aléatoire entre -45 et +45°
		RotateY((float)GD.RandRange(-Mathf.Pi / 24.0, Mathf.Pi / 24.0));

		// On calcule une vitesse aléatoire
		_vitesseDefinie = GD.RandRange(MinSpeed, MaxSpeed);
		
		// sauvegarde de la direction vers l'avant de l'objet (regarde vers Transform.Basis.Z dans godot)
		_directionHorizontale = -Transform.Basis.Z.Normalized();
		
		// s'assure que la fleur ne vole pas et ne plonge pas en ignorant l'axe Y du calcul de direction
		_directionHorizontale.Y = 0;
		_directionHorizontale = _directionHorizontale.Normalized();
	}	
	
	private void _on_visible_on_screen_notifier_3d_screen_exited()
{
	QueueFree();
}	
	
}
