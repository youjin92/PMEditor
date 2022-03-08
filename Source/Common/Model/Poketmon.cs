using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Common.Model
{
    public class Poketmon : BindableBase
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Health { get; set; }
        public string Attack { get; set; }
        public string Defense { get; set; }
        public string SPAttack { get; set; }
        public string SPDefense { get; set; }
        public string Speed { get; set; }
        public string TotalSum { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        public override string ToString()
        {
            return Number + "/" + Name + "/" + Health + "/" + Attack + "/" + Defense + "/" + SPAttack + "/" + SPDefense + "/" + Speed + "/" + TotalSum + "/" + Property1 + "/" + Property2;
        }

        public Poketmon Clone(Poketmon pok)
        {
            return new Poketmon() { Number = pok.Number, Name = pok.Name, Attack = pok.Attack, Defense = pok.Defense, 
                Health = pok.Health, Property1 = pok.Property1, Property2 = pok.Property2, SPAttack = pok.SPAttack, 
                SPDefense = pok.SPDefense, Speed = pok.Speed, TotalSum = pok.TotalSum};
        }

    }
}
