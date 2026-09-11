extends CharacterBody3D

const SPEED = 5.0
const JUMP_VELOCITY = 7.0

# Récupère la gravité depuis les paramètres du projet
var gravity: float = ProjectSettings.get_setting("physics/3d/default_gravity")

var nbPoints: int = 0
var nbPoupeesB: int = 0
var nbPoupeesR: int = 0
var nbFleurs: int = 0
var nbFleursAttak: int = 0

var cpt: Label
var cpt2: Label
var cpt3: Label
var cpt4: Label
var cpt5: Label


func _ready():
	# Accès aux nœuds parents et enfants de l'interface GUI
	cpt = get_parent().get_node("GUI/Compteur") as Label
	cpt2 = get_parent().get_node("GUI/Compteur2") as Label
	cpt3 = get_parent().get_node("GUI/Compteur3") as Label
	cpt4 = get_parent().get_node("GUI/Compteur4") as Label
	cpt5 = get_parent().get_node("GUI/Compteur5") as Label
	
	# Initialisation des textes
	cpt.text = "Nombre de poupées bleues : "
	cpt2.text = "Nombre de poupées rouges attrapées : "
	cpt3.text = "Nombre de fleurs touchées :"
	cpt4.text = "Nombre de poupées bleues : "
	cpt5.text = "Nombre de fleurs rouges reçues : "


func _on_area_3d_body_entered(body: Node3D):
	# En GDScript, si votre Autoload s'appelle "Global", vous pouvez l'appeler directement.
	# Si besoin, décommentez la ligne ci-dessous :
	# var globalScript = get_node("/root/Global")
	var global_script = Global
	
	for i in range(5):
		if body.name == "poupee_0" + str(i):
			body.queue_free()
			nbPoupeesR = global_script.increase_count(nbPoupeesR) # Attention : vérifiez si votre fonction dans Global est en snake_case
			global_script.print_count(nbPoupeesR)
			cpt2.text = "Nombre de poupées rouges attrapées : " + str(nbPoupeesR)

	for i in range(4):
		if body.name == "ami_poupee_0" + str(i):
			body.queue_free()
			nbPoupeesB = global_script.increase_count(nbPoupeesB)
			global_script.print_count(nbPoupeesB)
			cpt4.text = "Nombre de poupées bleues attrapées : " + str(nbPoupeesB)

	if body.name == "Attak_rigid":
		body.queue_free()
		nbFleursAttak = global_script.increase_count(nbFleursAttak)
		global_script.print_count(nbFleursAttak)
		print("Fleur rouge")
		cpt5.text = "Nombre de fleurs rouges reçues : " + str(nbFleursAttak)
		
	for i in range(7):
		if body.name == "fleur_0" + str(i):
			print("Nombre de points : ")
			cpt3.text = "Nombre de fleurs touchées : "
			nbFleurs = global_script.increase_count(nbFleurs)
			global_script.print_count(nbFleurs)
			cpt3.text = "Nombre de fleurs touchées : " + str(nbFleurs)
			
		nbPoints = nbPoupeesB + nbFleurs - nbPoupeesR - nbFleursAttak
		global_script.print_count(nbPoints)
		
		if nbPoints > 0:
			cpt.text = "Nombre de points : " + str(nbPoints) + ". Bien !"
		elif nbPoints < 0:
			cpt.text = "Nombre de points : " + str(nbPoints) + ". Reprenez-vous !"
		else:
			cpt.text = "Votre score est de 0."


func _on_area_3d_area_entered(area: Area3D):
	var _global_script = Global
	
	if area.name == "Fin":
		print("Fin du niveau")
		cpt.text = "Fin du niveau. "
		
		if nbPoints > 0:
			cpt.text += "Vous avez gagné " + str(nbPoints) + " point(s). Bravo ! "
		elif nbPoints < 0:
			cpt.text += "Vous avez perdu " + str(abs(nbPoints)) + " point(s). Dommage !"
		else:
			cpt.text += "Votre score est de 0. Dommage !"
			
	if area.name == "Vide":
		print("Tombé !")
		cpt.text = "Tombé !"


func _physics_process(delta: float):
	var current_velocity = velocity

	# Ajout de la gravité
	if not is_on_floor():
		current_velocity.y -= gravity * delta

	# Gestion du saut
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		current_velocity.y = JUMP_VELOCITY

	# Gestion des mouvements
	var input_dir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	var direction = Vector3(-input_dir.x, 0, input_dir.y).normalized()
	
	if direction != Vector3.ZERO:
		current_velocity.x = direction.z * SPEED
		current_velocity.z = direction.x * SPEED
		
		# Rotation stable
		var angle = atan2(direction.z, direction.x)
		rotation = Vector3(0, angle, 0)
	else:
		current_velocity.x = move_toward(velocity.x, 0, SPEED)
		current_velocity.z = move_toward(velocity.z, 0, SPEED)

	velocity = current_velocity
	move_and_slide()
