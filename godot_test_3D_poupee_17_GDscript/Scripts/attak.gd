extends CharacterBody3D

# Vitesse minimum en m/s
@export var min_speed: int = 6

# Vitesse maximum en m/s
@export var max_speed: int = 10

@export var gravity: float = 20.0

var _direction_horizontale: Vector3
var _vitesse_definie: float
var _vitesse_verticale: float = 0.0

func _physics_process(_delta: float) -> void:
	var mouvement_horizontal = _direction_horizontale * _vitesse_definie
	
	# Vérifie si le personnage n'est pas au sol
	if not is_on_floor():
		# Applique la gravité vers le bas (axe Y)
		_vitesse_verticale -= gravity * delta
		
		if _vitesse_verticale < -30.0:
			_vitesse_verticale = -30.0
	else:
		_vitesse_verticale = 0.0 # On stoppe au sol
	
	# Combine mouvement horizontal et gravité dans Velocity
	velocity = Vector3(mouvement_horizontal.x, _vitesse_verticale, mouvement_horizontal.z)
	
	move_and_slide()

func initialize(start_position: Vector3, player_position: Vector3) -> void:
	global_position = start_position
	# Positionne la fleur à sa position de départ start_position
	# et la tourne vers la position du joueur player_position pour qu'elle regarde le joueur
	look_at_from_position(start_position, player_position, Vector3.UP)
	
	# Tourne la fleur de manière aléatoire entre -45 et +45° (Note : PI / 24 correspond plutôt à environ 7.5°)
	rotate_y(randf_range(-PI / 24.0, PI / 24.0))

	# On calcule une vitesse aléatoire
	_vitesse_definie = randf_range(min_speed, max_speed)
	
	# Sauvegarde de la direction vers l'avant de l'objet (regarde vers transform.basis.z dans godot)
	_direction_horizontale = -transform.basis.z.normalized()
	
	# S'assure que la fleur ne vole pas et ne plonge pas en ignorant l'axe Y du calcul de direction
	_direction_horizontale.y = 0
	_direction_horizontale = _direction_horizontale.normalized()

func _on_visible_on_screen_notifier_3d_screen_exited() -> void:
	queue_free()
