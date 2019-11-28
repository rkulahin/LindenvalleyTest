using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
	[SerializeField] protected string _modifiedData;
	[SerializeField] protected Vector3 _coordinate;
	[SerializeField] protected string _name;
	[SerializeField] protected int _id;
	[SerializeField] protected Enum _type;

	protected float _speed;
	protected Transform _tr;
	protected bool _active;
	private void Awake() {
		_active = false;
		_speed = 1;
		_tr = transform;	
	}

	private void Start()
	{
		StartCoroutine(WaitForStart());
	}
	private void Update() {
		if (_active && _tr.position != _coordinate)
			MoveObj();
	}

	private void MoveObj()
	{
		_tr.position = Vector3.MoveTowards(_tr.position, _coordinate, _speed);
	}

	private void SetName()
	{
		gameObject.name = _name;
	}

	public void SetData(string data, Vector3 coord, string name, int id, Enum type)
	{
		_modifiedData = data;
		_coordinate = coord;
		_name = name;
		_id = id;
		_type = type;
		SetName();
	}

	public string GetModifiedData()
	{
		return _modifiedData;
	}

	IEnumerator WaitForStart()
	{
		yield return new WaitForSeconds(5f);
		_active = true;
	}
}
