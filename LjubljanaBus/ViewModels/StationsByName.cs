

using System.Collections.Generic;
using System.IO;
using System.Windows;
using System;
namespace LjubljanaBus.ViewModels
{
    public class StationsByName : List<StationsInGroup>
    {
        private static readonly string Groups = "#abcdefghijklmnopqrstuvwxyz";

        private Dictionary<int, Station> _personLookup = new Dictionary<int, Station>();

        public StationsByName()
        {
            List<Station> people = new List<Station>(AllPeople.Current);
            //people.Sort(Person.CompareByFirstName);

            Dictionary<string, PeopleInGroup> groups = new Dictionary<string, PeopleInGroup>();

            foreach (char c in Groups)
            {
                PeopleInGroup group = new PeopleInGroup(c.ToString());
                this.Add(group);
                groups[c.ToString()] = group;
            }

            foreach (Person person in people)
            {
                groups[Person.GetFirstNameKey(person)].Add(person);
            }
        }

        private void LoadStationsFromFile()
        {
            //FileStream file = ;
            string line;// = reader.ReadToEnd();

            using (StreamReader reader = new StreamReader(Application.GetResourceStream(new Uri(@"/LjubljanaBus;component/Data/stations.csv", UriKind.Relative)).Stream))
            //using (StreamReader reader = new StreamReader(File.OpenRead("Data/stations.csv")))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tmp = line.Split(new char[] { ';' });
                    if (tmp.Length == 4)
                    {
                        string id;
                        if (tmp[2].Length == 1)
                            id = "00" + tmp[2];
                        else if (tmp[2].Length == 2)
                            id = "0" + tmp[2];
                        else
                            id = tmp[2];

                        base.Add(new Station() { Langitude = tmp[0], Latitude = tmp[1], ID = id, Name = tmp[3] });


                    }
                }
            }
        }
    }
}
