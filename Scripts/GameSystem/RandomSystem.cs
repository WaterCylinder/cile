using Godot;
using System;
using System.Collections.Generic;

public partial class RandomSystem
{
	private Random randomObjDefault;
	private int defaultSeed;
	private Dictionary<string, Random> randomObjDict = new();
	private int randomSeed;
	public RandomSystem(int seed)
	{
		defaultSeed = seed;
		randomSeed = defaultSeed + 1;
		randomObjDefault = new Random(seed);
	}
	public int NextSeed()
	{
		randomSeed += 1;
		return randomSeed;
	}
	public RandomSystem(object seed)
	{
		int result;
		try
		{
			result = Convert.ToInt32(seed);
		}
		catch (Exception e)
		{
			e.GetHashCode();
			result = seed.GetHashCode();
		}
		randomObjDefault = new Random(result);
	}

	public string SetRandomGenerator(int seed = -1, string generatorName = null)
	{
		if (seed == -1)
		{
			seed = NextSeed();
		}
		if (generatorName == null)
		{
		    generatorName = Guid.NewGuid().ToString();
		}
		if (! randomObjDict.ContainsKey(generatorName))
		{
			randomObjDict.Add(generatorName, new Random(seed));
		}
		return generatorName;
	}

	/// <summary>
	/// 获取随机数生成器
	/// </summary>
	/// <param name="generaotr"></param>
	/// <returns></returns>
	public Random GetGenerator(string generaotr)
	{
		Random rTemp = null;
		if (generaotr == null || !randomObjDict.ContainsKey(generaotr))
		{
			rTemp = randomObjDefault;
		}
		else
		{
			rTemp = randomObjDict[generaotr];
		}
		return rTemp;
	}

	public Random GetDefaultGenerator()
	{
		return GetGenerator(null);
	}

	/// <summary>
	/// 生成min到max的随机浮点数
	/// </summary>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <param name="generaotr"></param>
	/// <returns></returns>
	public float Float(float min = 0, float max = 1, string generaotr = null)
	{
		Random rTemp = GetGenerator(generaotr);
		float r = (float)rTemp.NextDouble();
		float result = min + (max - min) * r;
		return result;
	}

	/// <summary>
	/// 生成min到max的随机整数，包含min和max
	/// </summary>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <param name="generaotr"></param>
	/// <returns></returns>
	public int Int(int min = 0, int max = 100, string generaotr = null)
	{
		Random rTemp = GetGenerator(generaotr);
		int result = rTemp.Next(min, max + 1);
		return result;
	}

}
