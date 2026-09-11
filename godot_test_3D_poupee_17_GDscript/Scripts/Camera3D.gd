extends Camera3D

@export var camera_offset: Vector3
var target: CharacterBody3D

# Appelé quand le nœud entre dans la scène pour la première fois.
func _ready():
	# Récupère le nœud du joueur depuis le parent
	target = get_parent().get_node("poupee_01") as CharacterBody3D
	
	var player_position = target.position
	var camera_position = position
	camera_offset = camera_position - player_position


# Appelé à chaque image (frame).
func _process(_delta: float):
	var player_position = target.position
	# On positionne la caméra par rapport à la poupée
	position = player_position + camera_offset
