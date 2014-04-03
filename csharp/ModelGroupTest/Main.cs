using System;
using ModelGroup;

namespace ModelGroupTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            try
            {
                string strPath = System.Environment.CurrentDirectory;
                string strEnum = System.IO.File.ReadAllText(strPath + @"\enum.json");
                string strRes = System.IO.File.ReadAllText(strPath + @"\resource.json");
                string strChannel = System.IO.File.ReadAllText(strPath + @"\channel.json");
                ModelGroupConfig.Instance.LoadConfig(strEnum, strRes, strChannel, strPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}
	}
}
