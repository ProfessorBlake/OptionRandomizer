using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
	public TMP_Text Text;

	private float _startSize;
	private float _targetSize;
	private bool _selected;
	private Color _startColor;

	private void Start()
	{
		_startSize = Text.fontSize;
		_startColor = Text.color;
	}

	private void Update()
	{
		if(_selected)
		{
			Text.fontSize = Mathf.Lerp(Text.fontSize, _targetSize, Time.deltaTime * 4f);
			Text.color = Color.Lerp(Text.color, _startColor, Time.deltaTime * 5f);
		}
		else
		{
			Text.fontSize = Mathf.Lerp(Text.fontSize, _startSize, Time.deltaTime * 10f);
			Text.color = Color.Lerp(Text.color, new Color(_startColor.r, _startColor.g, _startColor.b, 0.25f), Time.deltaTime * 10f);
		}
	}

	public void Select(bool select, float size)
	{
		_selected = select;
		_targetSize = size;
		Text.enableAutoSizing = !_selected;
	}
}
