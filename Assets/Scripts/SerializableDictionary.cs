using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<Key,Value>
{

	[SerializeField]
	private List<Key> keys;

	[SerializeField]
	private List<Value> values;


	public SerializableDictionary()
	{
		keys = new List<Key>();
		values = new List<Value>();
	}

	public void Add(Key key, Value value)
	{
		if(keys.Contains(key))
			return;

		keys.Add(key);
		values.Add(value);
	}
	
	public Value Get(Key key)
	{
		int i;
		if((i = keys.IndexOf(key)) > 0)
			return values[i];
		else
			return default(Value);
	}

	public bool ContainsKey(Key key)
	{
		return keys.Contains(key);
	}

	public bool TryGetValue(Key key, out Value value)
	{
		if (keys.Count != values.Count)
		{
			keys.Clear();
			values.Clear();
			value = default(Value);
			return false;
		}
		
		if (!keys.Contains(key))
		{
			value = default(Value);
			return false;
		}
		
		int index = keys.IndexOf(key);
		value = values[index];
		
		return true;
	}
	
	public void ChangeValue(Key key, Value value)
	{
		if (!keys.Contains(key))
			return;
		
		int index = keys.IndexOf(key);
		
		values[index] = value;
	}

	public List<Key> GetKeys()
	{
		return new List<Key>(keys);
	}

	public List<Value> GetValues()
	{
		return new List<Value>(values);
	}

	public Dictionary<Key,Value> CreateDictionary()
	{
		Dictionary<Key,Value> dict = new Dictionary<Key, Value>();
		for(int i = 0; i < keys.Count; i++) {
			dict.Add(keys[i],values[i]);
		}
		return dict;
	}
}

