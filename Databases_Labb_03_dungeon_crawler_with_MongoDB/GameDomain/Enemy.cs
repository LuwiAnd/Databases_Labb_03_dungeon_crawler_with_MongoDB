using System;
//using System.Data;

//using Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain;

namespace Databases_Labb_03_dungeon_crawler_with_MongoDB.GameDomain
{
    abstract class Enemy : LevelElement
    {
        //public string Type { get; set; }
        public double HP { get; set; }

        public Dice AttackDice;
        public Dice DefenceDice;

        //protected AttackDice;
        //protected DefenceDice;

        //public abstract void Update(Hero hero, List<LevelElement> elements);
        public abstract void Update(Hero hero, LevelData levelData);
        public virtual bool Defend(int damage)
        {
            HP -= damage;
            bool isDead = HP <= 0;
            return isDead;
        }

        //public virtual void AttackHero(Hero hero)
        public virtual void AttackHero(Hero hero, LevelData levelData)
        {
            int attack = AttackDice.Throw();
            int defence = hero.DefenceDice.Throw();
            //int damage = attack - defence;
            //if (damage < 0) { damage = 0; }
            int damage = Math.Max(0, attack - defence);
            hero.HP -= damage;

            var message =
                $"{Type} (HP: {HP}) throws dices: {AttackDice.ToString()} => {attack}. " + 
                $"Hero (HP: {hero.HP}) throws: {hero.DefenceDice.ToString()} => {defence}. " + 
                $"Damage = {damage}.";

            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"{Type} (HP: {HP}) throws dices: {AttackDice.ToString()} => {attack}. Hero (HP: {hero.HP}) throws: {hero.DefenceDice.ToString()} => {defence}. Damage = {damage}.");
            Console.WriteLine(message);
            levelData.Log(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

    }
}