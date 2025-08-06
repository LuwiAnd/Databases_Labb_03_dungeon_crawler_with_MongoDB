using System;
using System.Data;

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

        public virtual void AttackHero(Hero hero)
        {
            int attack = AttackDice.Throw();
            int defence = hero.DefenceDice.Throw();
            int damage = attack - defence;
            if (damage < 0) { damage = 0; }
            hero.HP -= damage;

            (int left, int top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Type} (HP: {HP}) throws dices: {AttackDice.ToString()} => {attack}. Hero (HP: {hero.HP}) throws: {hero.DefenceDice.ToString()} => {defence}. Damage = {damage}.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

    }
}