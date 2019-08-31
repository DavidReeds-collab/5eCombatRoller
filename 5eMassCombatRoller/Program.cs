using System;

namespace _5eMassCombatRoller
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("input first army name;");
            string army1Name = Console.ReadLine();
            int army1Size = 0;
            while (army1Size <= 0)
            {
                Console.WriteLine("input first army size;");
                if (Int32.TryParse(Console.ReadLine(), out army1Size))
                {

                }
                else
                {
                    Console.WriteLine("input first army size;");
                }

            }

            int army1CR = -1;
            while (army1CR <= -1)
            {
                Console.WriteLine("input first army CR;");
                if (Int32.TryParse(Console.ReadLine(), out army1CR))
                {

                }
                else
                {
                    Console.WriteLine("input first army CR;");
                }

            }

            int army1Morale = 0;
            while (army1Morale == 0)
            {
                Console.WriteLine("input first army morale;");
                if (Int32.TryParse(Console.ReadLine(), out army1Morale))
                {

                }
                else
                {
                    Console.WriteLine("input first army morale;");
                }

            }

            Console.WriteLine("input second army name;");
            string army2Name = Console.ReadLine();

            int army2Size = 0;
            while (army2Size <= 0)
            {
                Console.WriteLine("input second army size;");
                if (Int32.TryParse(Console.ReadLine(), out army2Size))
                {

                }
                else
                {
                    Console.WriteLine("input second army size;");
                }

            }

            int army2CR = -1;
            while (army2CR <= -1)
            {
                Console.WriteLine("input second army CR;");
                if (Int32.TryParse(Console.ReadLine(), out army2CR))
                {

                }
                else
                {
                    Console.WriteLine("input second army CR;");
                }

            }

            int army2Morale = 0;
            while (army2Morale == 0)
            {
                Console.WriteLine("input second army morale;");
                if (Int32.TryParse(Console.ReadLine(), out army2Morale))
                {

                }
                else
                {
                    Console.WriteLine("input second army morale;");
                }

            }

            int simulations = 0;
            while (simulations == 0)
            {
                Console.WriteLine("input amount of simulations;");
                if (Int32.TryParse(Console.ReadLine(), out simulations))
                {

                }
                else
                {
                    Console.WriteLine("input amount of simulations;");
                }

            }

            Army army1 = new Army(army1Name, army1Size, army1CR, true, army1Morale);
            Army army2 = new Army(army2Name, army2Size, army2CR, false, army2Morale);

            for (int i = 0; i < simulations; i++)
            {
                ResolveCombat(army1, army2);

                Console.WriteLine("");
            }

            decimal winrate = ((decimal)(simulations - army1.Defeats) / (decimal)simulations);

            Console.WriteLine($"{army1.Name} has a {(winrate * 100)}% winrate against {army2.Name}.");
            army1.Reset();
            army2.Reset();
            Console.WriteLine($"{army1.Name} powerrating = {army1.Size + army1.CR + army1.BR}");
            Console.WriteLine($"{army2.Name} powerrating = {army2.Size + army2.CR + army2.BR}");

            Console.ReadKey();
        }

        static void ResolveCombat(Army attacker, Army defender)
        {
            attacker.Reset();
            defender.Reset();

            while (CombatContinue(attacker) && CombatContinue(defender))
            {
                ResolveTurn(attacker, defender);
                if(!(CombatContinue(attacker) && CombatContinue(defender)))
                {

                    break;
                }
                ResolveTurn(defender, attacker);
            }
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

            public int Moralle { get; set; }

            public int CR { get; set; }

            public int BR { get { return (int)MathF.Floor(this.CR / 3); } }

            public bool Attacker = false;

            public int Defeats = 0;

            public int rollD20()
            {
                Random random = new Random();

                return random.Next(1, 20);
            }

            public int rollToHit()
            {
                Random random = new Random();

                return random.Next(1, 20) + this.Size + this.CR;
            }

            public int rollToHit(int d20)
            {
                return d20 + this.Size + this.CR;
            }

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
