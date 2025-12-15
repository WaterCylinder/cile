using Godot;
using System;

namespace Utils
{
	public class Tools
    {
        public static string Path(string path, string basePath = "")
        {
            string[] pathList = path.Split(['/','\\','.','-']);
			string result = basePath;
			for(int i = 0; i<pathList.Length; i++)
            {
                result = result.PathJoin(pathList[i]);
            }
			return result;
        }
    }
}
