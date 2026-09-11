using Godot;
using System;

public partial class fleur_seule_01 : Area3D
{
	[Export] public float FallSpeed = 5.0f;
	private bool _isAttached = false;

	public override void _Ready()
	{
		// Test immédiat pour valider que la console fonctionne
		GD.Print("Le script de l'objet est bien actif et prêt !");
		
		// Connecte le signal d'entrée de zone
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isAttached) return;

		// Fait descendre l'objet tout seul s'il n'est pas attaché
		Position -= new Vector3(0, FallSpeed * (float)delta, 0);
	}

	private void OnBodyEntered(Node body)
	{
		if (_isAttached) return;

		GD.Print("Collision détectée avec : " + body.Name);

		var headMarker = body.GetNodeOrNull<Marker3D>("ChapeauFleur");
		if (headMarker != null)
		{
			GD.Print("Tête du personnage trouvée, accrochage en cours...");
			_isAttached = true;

			// Sauvegarde la position mondiale
			Transform3D globalTrans = GlobalTransform;

			// Change de parent de manière sécurisée
			GetParent().RemoveChild(this);
			headMarker.AddChild(this);

			// Remet les coordonnées à zéro par rapport à la tête
			GlobalTransform = globalTrans;
			Position = Vector3.Zero;
			Rotation = Vector3.Zero;

			// Coupe la détection pour ne plus déclencher d'événements
			SetDeferred(PropertyName.Monitoring, false);
		}
		else
		{
			GD.Print("L'objet a touché un corps mais sans 'HeadMarker'.");
		}
	}
}
