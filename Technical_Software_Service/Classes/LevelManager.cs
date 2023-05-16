using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Technical_Software_Service.Classes
{
    public class LevelManager
    {
        private Users user;

        public LevelManager(Users user)
        {
            this.user = user;
        }

        public (int level, int currentXP, int nextLevel) CheckLevelUp()
        {
            int MAX_LEVEL = 100;
            int nextLevel = CalculateNextLevelXP(user.Level);

            if (user.XP >= nextLevel)
            {
                user.Level++;
                user.XP = 0;
                nextLevel = CalculateNextLevelXP(user.Level);
                MessageBox.Show($"Вы достигли уровня {user.Level}!");

                // Проверка достижений на основе уровня
                AchievementsManager.CheckLevelAchievements(user);
            }
            
            if (user.Level == MAX_LEVEL)
            {
                user.XP = 0;
                user.Level = 1;
                nextLevel = CalculateNextLevelXP(1);
                MessageBox.Show($"Поздравляем! Вы достигли максимального {MAX_LEVEL} уровня !");
                //Пасхалка
                MessageBox.Show("Дорогой товарищ!\n\nОт всей души поздравляем тебя с достижением 100 уровня. Это свидетельствует о твоем трудолюбии и упорстве в достижении поставленных целей.\n\nТы прошел долгий путь, преодолевая трудности и препятствия, но не сдаешься, идешь только вперед. В этом твоя главная сила - воля и мужество никогда не покидает тебя.\n\nМы рады видеть, что ты становишься все более сильным и готовым к новым вызовам. Иди дальше, товарищ, твой труд и настойчивость сделают мир лучше и справедливее!\n\nС уважением,\nГАУ НО ЦИТ", "Трудиться и ещё раз трудиться!");
                MessageBox.Show($"Уровень и опыт были сброшены");
            }
            // Сохраняем изменения в базе данных
            DataBase.Base.SaveChanges();
            return (user.Level, user.XP, nextLevel);
        }

        // Метод вычисляет максимальный опыт, необходимый для достижения следующего уровня
        private int CalculateNextLevelXP(int currentLevel)
        {
            int nextLevelXP = 100;
            int nextLevelMaxXP = currentLevel * nextLevelXP + nextLevelXP;
            return nextLevelMaxXP;
        }
    }
}
