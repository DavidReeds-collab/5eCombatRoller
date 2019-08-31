using System;

namespace _5eMassCombatRoller
{
    class Program
    {

        static void Main(string[] args)
        {
            //Inputs for initial statistics
            Console.WriteLine("input first army name;");

            string army1Name = Console.ReadLine();

            int army1Size = -1;

            int army1CR = -1;

            int army1Morale = -1;

            InputToValue("first army size", ref army1Size);

            InputToValue("first army Challenge Rating", ref army1CR);

            InputToValue("first army Morale", ref army1Morale);

            Console.WriteLine("input second army name;");

            string army2Name = Console.ReadLine();

            int army2Size = -1;

            int army2CR = -1;

            int army2Morale = -1;

            InputToValue("second army size", ref army2Size);

            InputToValue("second army Challenge Rating", ref army2CR);

            InputToValue("second army Morale", ref army2Morale);

            //Amount of simulations
            int simulations = -1;

            InputToValue("amount of simulations", ref simulations);
                       
            Army army1 = new Army(army1Name, army1Size, army1CR, true, army1Morale);

            Army army2 = new Army(army2Name, army2Size, army2CR, false, army2Morale);

            //Resolves the combat X amount of times
            for (int i = 0; i < simulations; i++)
            {
                ResolveCombat(army1, army2);

                Console.WriteLine("");
            }

            decimal winrate = ((decimal)(simulations - army1.Defeats) / (decimal)simulations);

            Console.WriteLine($"{army1.Name} has a {(winrate * 100)}% winrate against {army2.Name}.");

            //Resets the size and morale to prevent influences of the last combat rounds. 
            army1.Reset();

            army2.Reset();

            //User check to see if the armies are/should be balanced.
            Console.WriteLine($"{army1.Name} powerrating = {army1.Size + army1.CR + army1.BR}");

            Console.WriteLine($"{army2.Name} powerrating = {army2.Size + army2.CR + army2.BR}");

            Console.ReadKey();
        }

        //standardized method to set the statistics of the armies. 
        private static void InputToValue(string message, ref int value)
        {
            while (value <= -1)
            {
                Console.WriteLine($"Input {message}");
                if (Int32.TryParse(Console.ReadLine(), out value))
                {

                }
                else
                {
                    Console.WriteLine($"Input {message}");
                }

            }

        }

        //Implement later perhaps for the strings used? Is this needed? If not, delete later. 
        private static void InputToValue(string message, string value)
        {
            
        }

        static void ResolveCombat(Army attacker, Army defender)
        {
            //Resets both armies to prevent influence of previous rounds.
            attacker.Reset();

            defender.Reset();

            //Do turns until the lose-conditions are met. 
            while (CombatContinue(attacker) && CombatContinue(defender))
            {
                ResolveTurn(attacker, defender);
                if(!(CombatContinue(attacker) && CombatContinue(defender)))
                {

                    break;
                }
                ResolveTurn(defender, attacker);
            }
            //Logging the reason for defeat for analysis.
            Console.WriteLine($"Final score: The {attacker.Name} has {attacker.Size} size and {attacker.Moralle} morale left.");

            Console.WriteLine($"Final score: The {defender.Name} has {defender.Size} size and {defender.Moralle} morale left.");
        }

        static bool CombatContinue(Army army)
        {
            bool Continue = (army.Moralle > 0 && army.Size > 0); ;

            if (!Continue)
            {
                army.Defeats += 1;

                Console.WriteLine($"{army.Name} lost!");
            }

            return Continue;
        }

        private static void ResolveTurn(Army attacker, Army defender)
        {
            Console.WriteLine($"The {attacker.Name}(size {attacker.Size}, BR {attacker.BR}, CR {attacker.CR}) attack {defender.Name}(size {defender.Size}, BR {defender.BR}, CR {defender.CR}). ");

            int atttackerRoll = attacker.rollD20();

            Console.WriteLine($"The {attacker.Name} rolled {atttackerRoll}, giving him an {attacker.rollToHit(atttackerRoll)}(D20 {atttackerRoll} + size {attacker.Size} + CR {attacker.CR}) against the {defender.Name}'s {defender.AC}");

            if (attacker.rollToHit(atttackerRoll) > defender.AC)
            {
                defender.Moralle -= 1;

                Console.WriteLine($"{attacker.Name} hit. The {defender.Name}'s morale is {defender.Moralle}.");

                if (defender.Moralle < -1)
                {
                    Console.WriteLine($"{defender.Name} broke. Combat over.");
                    return;

                }
                if (attacker.BR == 0)
                {
                    Console.WriteLine($"{defender.Name} looses 1 size.");
                    defender.Size -= 1;
                }
                else
                {
                    Console.WriteLine($"{defender.Name} looses {attacker.BR} size.");
                    defender.Size -= attacker.BR;
                }
            }
            else
            {
                attacker.Moralle -= 1;
            }

            if (defender.Size <= 0)
            {
                Console.WriteLine($"{defender.Name} is dead. Combat over.");
                return;
            }
        }

        class Army
        {
            public string Name { get; set; }
            public int Size { get; set; }

            //How many mistakes the army can make before breaking. 
            public int Moralle { get; set; }

            //Challenge rating straight from the monster manual.
            public int CR { get; set; }

            //Sepetate value needed to limit the damage high level creatures can do.
            public int BR { get { return (int)MathF.Floor(this.CR / 3); } }

            public bool Attacker = false;

            public int Defeats = 0;

            //Rolls a D20 with no added statistics. Returns the base roll.
            public int rollD20()
            {
                Random random = new Random();

                return random.Next(1, 20);
            }

            //Tries to make a hit with a D20
            public int rollToHit()
            {
                Random random = new Random();

                return random.Next(1, 20) + this.Size + this.CR;
            }

            //Tries to make a hit with D20 input.
            public int rollToHit(int d20)
            {
                return d20 + this.Size + this.CR;
            }

            //The contested check for the hits.
            public int AC { get { return 10 + Size + CR; } }

            public Army(string name, int size, int cr, bool attacker, int morale)
            {
                Name = name;
                Size = size;
                CR = cr;
                Moralle = morale;
                Attacker = attacker;

                resetSize = size;
                resetMorale = morale;

            }

            private int resetSize;
            private int resetMorale;

            public void Reset()
            {
                Moralle = resetMorale;
                Size = resetSize;
            }
        }
    }
}
