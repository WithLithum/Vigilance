using System.Collections.Generic;
using Rage;
using RandomNameGeneratorLibrary;

namespace Vigilance.Entities
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Persona
    {
        internal static readonly Dictionary<Ped, Persona> Personas = new Dictionary<Ped, Persona>();

        public Persona(Ped ped)
        {
            var generator = new PersonNameGenerator();

            _theName = ped.IsMale ? generator.GenerateRandomMaleFirstAndLastName() : generator.GenerateRandomFemaleFirstAndLastName();
        }

        private string _theName;

        public string FirstName
        {
            get => _theName.Split(' ')[0];
            set
            {
                string[] names = _theName.Split(' ');
                names[0] = value;
                _theName = names[0] + " " + names[1];
            }
        }
        public string LastName
        {
            get => _theName.Split(' ')[1];
            set
            {
                string[] names = _theName.Split(' ');
                names[1] = value;
                _theName = names[0] + " " + names[1];
            }
        }
        public string FullName
        {
            get => _theName;
            set
            {
                string[] exceptedName = value.Split(' ');
                FirstName = exceptedName[0];
                LastName = exceptedName[1];
            }
        }
    }
}
