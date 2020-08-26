using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using UnityEditor;

public class Randomizer : MonoBehaviour
{
	public TMP_Text TextPrefab;
	public List<string> ItemText = new List<string>();
	public Transform Container;
	public Color[] Colors;
	public TMP_Dropdown Dropdown;

	private List<TMP_Text> _itemList = new List<TMP_Text>();
	private int _selectedItemIndex;

	private void Start()
	{
		List<string> names = new List<string>();

		DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
		print("Streaming Assets Path: " + Application.streamingAssetsPath);
		FileInfo[] allFiles = directoryInfo.GetFiles("*.*");


		foreach (FileInfo fi in allFiles)
		{
			if (fi.Name.ToLower().Contains("meta") || string.IsNullOrEmpty(fi.Name))
				continue;
			names.Add(fi.Name);
			string s = fi.OpenText().ReadToEnd();
			Debug.Log(s);
			ItemText.Add(s);
		}
		Dropdown.AddOptions(names);
	}

	public void SelectText(int t)
	{
		BuiltList(ItemText[t-1]);
		Dropdown.gameObject.SetActive(false);
	}

	private void BuiltList(string l)
	{
		StartCoroutine(BuildListCoroutine(l));
	}

	private IEnumerator BuildListCoroutine(string l)
	{
		var list = l.Split('\n');
		List<string> sortedList = list.OrderBy(x => UnityEngine.Random.value).ToList();

		Debug.Log("Items sorted: ");
		foreach (string s in sortedList)
		{
			if (string.IsNullOrEmpty(s))
				continue;
			TMP_Text txt = Instantiate(TextPrefab, Container);
			txt.text = s;
			txt.color = Colors[UnityEngine.Random.Range(0, Colors.Length)];
			_itemList.Add(txt);
			_itemList[_selectedItemIndex].GetComponent<Item>().Select(false, 1f);
			_selectedItemIndex = _itemList.Count-1;
			_itemList[_selectedItemIndex].GetComponent<Item>().Select(true, 200f);
			yield return new WaitForSeconds(0.4f);
		}
		_itemList[_selectedItemIndex].GetComponent<Item>().Select(false, 1f);
		_selectedItemIndex = 0;
		_itemList[_selectedItemIndex].GetComponent<Item>().Select(true, 180f);
	}

	private void Update()
	{
		int dir = Input.GetKeyDown(KeyCode.UpArrow) ? -1 : Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0;
		if(dir != 0)
		{
			_itemList[_selectedItemIndex].GetComponent<Item>().Select(false, 1f);

			_selectedItemIndex += dir;
			if (_selectedItemIndex >= _itemList.Count)
				_selectedItemIndex = 0;
			else if (_selectedItemIndex < 0)
				_selectedItemIndex = _itemList.Count - 1;

			_itemList[_selectedItemIndex].GetComponent<Item>().Select(true, 180);
		}

		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}
}
