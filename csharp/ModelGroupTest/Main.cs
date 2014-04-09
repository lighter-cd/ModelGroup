using System;
using System.Collections.Generic;
using ModelGroup.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ModelGroupTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            try
            {
                string strPath = System.Environment.CurrentDirectory;

                string strEnumSchema = System.IO.File.ReadAllText(strPath + @"\enum_schema.json");
                string strResSchema = System.IO.File.ReadAllText(strPath + @"\resource_schema.json");
                string strChannelSchema = System.IO.File.ReadAllText(strPath + @"\channel_schema.json");

                string strEnum = System.IO.File.ReadAllText(strPath + @"\enum.json");
                string strRes = System.IO.File.ReadAllText(strPath + @"\resource.json");
                string strChannel = System.IO.File.ReadAllText(strPath + @"\channel.json");

                JsonSchema schemaEnum = JsonSchema.Parse(strEnumSchema);
                JsonSchema schemaResource = JsonSchema.Parse(strResSchema);
                JsonSchema schemaChannel = JsonSchema.Parse(strChannelSchema);

                JObject jsonEnum = JObject.Parse(strEnum);
                JObject jsonResource = JObject.Parse(strRes);
                JObject jsonChannel = JObject.Parse(strChannel);

                IList<string> messages = null ;
                bool valid = jsonEnum.IsValid(schemaEnum, out messages);
                if (valid)
                {
                    valid = jsonResource.IsValid(schemaResource, out messages);
                    if(valid)
                    {
                        valid = jsonChannel.IsValid(schemaChannel, out messages);
                    }
                }
                if (!valid)
                {
                    foreach (string msg in messages)
                    {
                        Console.WriteLine(msg);
                    }
                    return;
                }

                Enums enums = JsonConvert.DeserializeObject<Enums>(strEnum);
                Resource resources = JsonConvert.DeserializeObject<Resource>(strRes);
                Channels channels = JsonConvert.DeserializeObject<Channels>(strChannel);

                if (!new EnumsValider().Valid(enums, out messages))
                {
                    foreach (string msg in messages)
                    {
                        Console.WriteLine(msg);
                    }
                    return;
                }
                if (!new ResourceValider().Valid(enums, resources,out messages))
                {
                    foreach (string msg in messages)
                    {
                        Console.WriteLine(msg);
                    }
                    return;
                }
                if (!new ChannelValider().Valid(enums, resources, channels,out messages))
                {
                    foreach (string msg in messages)
                    {
                        Console.WriteLine(msg);
                    }
                    return;
                }


                ModelGroupConfig.Instance.LoadConfig(strEnum, strRes, strChannel, strPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}
	}
}
