
using UnityEngine;
using System.Collections;

public class PlayerExploration : MonoBehaviour {
	public static PlayerExploration instance;

	public float move_speed = 1f;
	public Animator generic_animator;
	public Animator sprite_animator;
	public SpriteRenderer main_renderer;
	public Transform visuals_transform;
	public Transform hitboxes_transform;

	public bool can_control = true;
	Vector2 _move_vector;
	Transform _transform;
	void Awake()
	{
		_transform = transform;
		instance = this;
	}

	void FixedUpdate()
	{
//		rigidbody2D.MovePosition(rigidbody2D.position + (_move_vector * move_speed * Time.deltaTime));
		rigidbody2D.velocity = _move_vector * move_speed * Time.deltaTime;

		generic_animator.SetFloat("MoveSpeed", _move_vector.magnitude);
	}
	void Update()
	{
		if (can_control == true)
			CheckControls();
		else
			_move_vector = Vector2.zero;
//		_transform.position += (Vector3)_move_vector * move_speed * Time.deltaTime;
	}
	public void EnableControls()
	{
		can_control = true;
	}
	public void DisableControls()
	{
		can_control = false;
	}
	void CheckControls()
	{
		_move_vector.x = Input.GetAxis("Horizontal");
		_move_vector.y = Input.GetAxis("Vertical");
		
		if (_move_vector.x > 0)
			_move_vector.x = 1f;
		else if (_move_vector.x < 0)
			_move_vector.x = -1f;
		else
			_move_vector.x = 0f;
		if (_move_vector.y > 0)
			_move_vector.y = 1f;
		else if (_move_vector.y < 0)
			_move_vector.y = -1f;
		else
			_move_vector.y = 0f;

		if (_move_vector != Vector2.zero)
		{
			_move_vector.Normalize();			
			RotatePlayer(_move_vector);
		}
	}
	public void RotatePlayer(Vector2 direction)
	{
		Rotater2D.LookAt(transform, transform.position + (Vector3)direction);
	}
	public void UpdateMosterExploration()
	{
		sprite_animator.runtimeAnimatorController = Player.instance.current_moster.GetAnimatorController();
//		main_renderer.color = Color.green;
		hitboxes_transform.localScale = Player.instance.current_moster.exploration_hitboxes_transform.localScale;
		visuals_transform.localScale = Player.instance.current_moster.visuals_transform.localScale;
	}
}
