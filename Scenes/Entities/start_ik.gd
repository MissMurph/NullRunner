@tool

extends SkeletonIK3D

func _ready():
	start(false)


func _process(delta):
	if !is_running():
		start(false)
