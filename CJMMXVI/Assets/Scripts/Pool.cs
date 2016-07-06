using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pool<T>
{
	#region Fields
	readonly Func<T> creator;

	readonly List<T> free;
	readonly List<T> used;

	public Action<T> onGet;
	public Action<T> onFree;
	#endregion
	
	#region Properties
	public int Count
	{
		get { return free.Count + used.Count; }
	}
	#endregion

	#region Constructors
	public Pool(Func<T> creator)
	{
		this.creator = creator;
		this.free = new List<T>();
		this.used = new List<T>();
	}

	public void Grow(int size)
	{
		if(Count >= size) { return; }

		while(Count < size)
		{
			T obj = creator();
			free.Add(obj);
			onFree(obj);
		}
	}

	public T Get()
	{
		T obj = default(T);

		if(free.Count == 0)
		{
			obj = creator();
		}
		else
		{
			obj = free[0];
			free.RemoveAt(0);
		}

		used.Add(obj);

		if(onGet != null) { onGet(obj); }

		return obj;
	}

	public void Return(T obj)
	{
		bool removed = used.Remove(obj);

		// Not handled by pool
		if(!removed) { return; }

		free.Add(obj);

		if(onFree != null) { onFree(obj); }
	}
	#endregion
}
