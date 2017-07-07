using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntaxGeneration
{
    public class Template
    {
        private readonly Random _random;

        public Template(int seed)
        {
            _random = new Random(seed);
        }

        public string GetNumber(int comparison)
        {
            if (comparison == 0)
            {
                return "The comapriosn you entered isn't correct";
            }

            var x = new List<string>();

            x.FirstOrDefault(x => x.Equals(""));

            var randomInt = _random.Next();

            if (randomInt > comparison)
            {
                return "number generated is greater than number entered";
            }
            if (randomInt < comparison)
            {
                return "number generated is less than number entered";
            }
            if (randomInt == comparison)
            {
                return "the numbers were identical";
            }

            return "i have no idea what happened";
        }
    }
}