using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Serializer
    {
        static public void SaveXml(int score, string datapath)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(int));

            FileStream fs = new FileStream(datapath, FileMode.Create);
            serializer.Serialize(fs, score);
            fs.Close();

        }

        static public int DeXml(string datapath)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(int));

            FileStream fs = new FileStream(datapath, FileMode.Open);
            int score = (int)serializer.Deserialize(fs);
            fs.Close();

            return score;
        }
    }
}
