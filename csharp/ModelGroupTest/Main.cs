using System;
using System.Collections.Generic;
using ModelGroup.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
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
                CommandArgs commandArg = CommandLine.Parse(args);
                Dictionary<string, string> argPairs = commandArg.ArgPairs;
                string strPath = argPairs.ContainsKey("path")?argPairs["path"]:System.Environment.CurrentDirectory;
                string strEnumPath = argPairs.ContainsKey("enum") ? argPairs["enum"] : "enum.json";
                string strResPath = argPairs.ContainsKey("resource") ? argPairs["resource"] : "resource.json";
                string strChannelPath = argPairs.ContainsKey("channel") ? argPairs["channel"] : "channel.json";

                string strEnumSchema = System.IO.File.ReadAllText(strPath + @"\enum_schema.json");
                string strResSchema = System.IO.File.ReadAllText(strPath + @"\resource_schema.json");
                string strChannelSchema = System.IO.File.ReadAllText(strPath + @"\channel_schema.json");

                string strEnum = System.IO.File.ReadAllText(strPath + @"\" + strEnumPath);
                string strRes = System.IO.File.ReadAllText(strPath + @"\" + strResPath);
                string strChannel = System.IO.File.ReadAllText(strPath + @"\" + strChannelPath);

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

                Console.WriteLine("Valid successed");

                String[] fileName = { @"\enum.bson", @"\resource.bson", @"\channel.bson" };
                Object[] objects = { enums ,resources,channels};
                for (int i = 0; i < 3; i++)
                {
                    System.IO.FileStream fs = new System.IO.FileStream(strPath + fileName[i], System.IO.FileMode.Create);
                    using (BsonWriter writer = new BsonWriter(fs))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(writer, objects[i]);
                    }
                }
                //ModelGroupConfig.Instance.LoadConfig(strEnum, strRes, strChannel, strPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}
	}
}
