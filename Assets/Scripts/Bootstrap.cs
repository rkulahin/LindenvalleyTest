using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] Figure [] _figures;
	private List<FigureData> _figureDatas;
	private Data _data;
	private Data _newData;
	private string jsonStr;

	private void Start() {
		InitFigure();
		LoadData();
		StartCoroutine(Parse());
	}

	private void InitFigure()
	{
		for(int i = 0; i < _figures.Length; i++)
		{
			_figures[i] = Instantiate(_figures[i], Vector3.zero, Quaternion.identity);
		}
	}

	private void SetData()
	{
		_newData = JsonUtility.FromJson<Data>(jsonStr);
		if(_data == null)
		{
			_data = _newData;
			UpdateData();
		}
		else
		{
			if(!String.Equals(_newData.modifiedData, _data.modifiedData))
				UpdateData();
		}
	}

	private void UpdateData()
	{
		for(int i = 0; i < _figures.Length; i++)
		{
			if(!String.Equals(_figures[i].GetModifiedData(), _newData.data[i].modifiedData))
				UpdateFigureData(_figures[i], _newData.data[i]);
		}
	}

	private void UpdateFigureData(Figure figure, FigureData figureData)
	{
		Debug.Log("UpdateFigure");
		figure.SetData(figureData.modifiedData, figureData.coordinate,
						figureData.name, figureData.id, figureData.type);
	}

	private void SaveData()
	{
		PlayerPrefs.SetString("json", jsonStr);
		PlayerPrefs.Save();
	}

	private void LoadData()
	{
		jsonStr = PlayerPrefs.GetString("json");
	}

    IEnumerator Parse()
    {
        for (; ; )
        {
			Debug.Log("Check");
            StartCoroutine(GetText());
            yield return new WaitForSeconds(10f);
        }
    }

	IEnumerator GetText() {
        UnityWebRequest www = UnityWebRequest.Get("https://owncloud.lindenvalley.de/index.php/s/lXIIkptI7tKJRL8/download");
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
		{
			jsonStr = www.downloadHandler.text;
			SetData();
			SaveData();
			StopCoroutine(GetText());
		}
    }

}
