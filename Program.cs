using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GenerateTable
{
    class Program
    {
        static void Main(string[] args)
        {
            var configName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dict.opcorpora.xml");
            if (File.Exists(configName))
            {
                // чтение конфигурационного файла
                var xdoc = XDocument.Load(configName);
                XElement dictionary = xdoc.Element("dictionary");
                XElement grammemes = dictionary.Element("grammemes");
                XElement lemmata = dictionary.Element("lemmata");
                var list = new HashSet<string>();
                foreach (XElement lemma in lemmata.Elements("lemma"))
                {
                    foreach (XElement l in lemma.Elements("l"))
                    {
                        foreach (var attr in l.Attributes())
                        { 
                            list.Add(attr.Value);
                            break;
                        }
                    }
                }
                var lst = new List<string>(list.Distinct().OrderBy(item => item));
                var lst1 = new List<string>(lst);
                var lst2 = new List<string>(lst);
                var lst3 = new List<string>(lst);
                var rand = new Random();
                var tableName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "table.csv");
                if (File.Exists(tableName)) File.Delete(tableName);
                using (StreamWriter csv = new StreamWriter(tableName, true, Encoding.Default))
                {
                    foreach (var word in lst)
                    {
                        var ind1 = rand.Next(lst1.Count);
                        var ind2 = rand.Next(lst2.Count);
                        var ind3 = rand.Next(lst3.Count);
                        csv.WriteLine($"{word} {lst1[ind1]} {lst2[ind2]} {lst3[ind3]};{lst1[ind1]};{lst2[ind2]};{lst3[ind3]}");
                        lst1.RemoveAt(ind1);
                        lst2.RemoveAt(ind2);
                        lst3.RemoveAt(ind3);
                    }
                    csv.Close();
                }

            }
        }
    }
}
