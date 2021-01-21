using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using RandomNameGeneratorLibrary;

namespace Vigilance.Entities
{
    public class Persona
    {
        internal static Dictionary<Ped, Persona> Personas = new Dictionary<Ped, Persona>();

        public Persona(Ped ped)
        {
            PersonNameGenerator generator = new PersonNameGenerator();

            if (ped.IsMale)
            {
                theName = generator.GenerateRandomMaleFirstAndLastName();
            }
            else
            {
                theName = generator.GenerateRandomFemaleFirstAndLastName();
            }
        }

        string theName;

        public string FirstName
        {
            get => theName.Split(' ')[0];
            set
            {
                string[] names = theName.Split(' ');
                names[0] = value;
                theName = names[0] + " " + names[1];
            }
        }
        public string LastName
        {
            get => theName.Split(' ')[1];
            set
            {
                string[] names = theName.Split(' ');
                names[1] = value;
                theName = names[0] + " " + names[1];
            }
        }
        public string FullName
        {
            get => theName;
            set
            {
                string[] exceptedName = value.Split(' ');
                FirstName = exceptedName[0];
                LastName = exceptedName[1];
            }
        }
    }
}
