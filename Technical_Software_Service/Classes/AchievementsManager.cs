using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Technical_Software_Service.Classes
{
    public class AchievementsManager
    {
        private Users user;

        public AchievementsManager(Users user)
        {
            this.user = user;
        }

        public static void CheckClosedTicketsAchievements(Users user)
        {
            int closedTicketsCount = user.CompletedCountTicketsClosed;

            if (closedTicketsCount >= 1)
            {
                UnlockAchievement(user, "Первый шаг");
            }

            if (closedTicketsCount >= 10)
            {
                UnlockAchievement(user, "Путь к достижениям");
            }

            if (closedTicketsCount >= 50)
            {
                UnlockAchievement(user, "Мастер закрытия заявок");
            }

            if (closedTicketsCount >= 100)
            {
                UnlockAchievement(user, "Профессионал закрытых заявок");
            }

            if (closedTicketsCount >= 500)
            {
                UnlockAchievement(user, "Чемпион закрытия заявок");
            }

            if (closedTicketsCount >= 1000)
            {
                UnlockAchievement(user, "Мастерство в закрытии заявок");
            }
        }

        public static void CheckCreatedTicketsAchievements(Users user)
        {
            int createdTicketsCount = user.CreateCountTickets;

            if (createdTicketsCount >= 1)
            {
                UnlockAchievement(user,"Создатель");
            }

            if (createdTicketsCount >= 10)
            {
                UnlockAchievement(user,"Путь к продуктивности");
            }

            if (createdTicketsCount >= 50)
            {
                UnlockAchievement(user,"Мастер создания заявок");
            }

            if (createdTicketsCount >= 100)
            {
                UnlockAchievement(user, "Профессиональный создания заявок");
            }

            if (createdTicketsCount >= 500)
            {
                UnlockAchievement(user, "Чемпион создания заявок");
            }

            if (createdTicketsCount >= 1000)
            {
                UnlockAchievement(user, "Мастерство в создании заявок");
            }
        }

        public static void CheckLevelAchievements(Users user)
        {
            int level = user.Level;

            if (level >= 10)
            {
                UnlockAchievement(user, "Достижение 10 уровней");
            }

            if (level >= 25)
            {
                UnlockAchievement(user, "Достижение 25 уровней");
            }

            if (level >= 50)
            {
                UnlockAchievement(user, "Достижение 50 уровней");
            }

            if (level >= 75)
            {
                UnlockAchievement(user, "Достижение 75 уровней");
            }

            if (level >= 100)
            {
                UnlockAchievement(user, "Достижение 100 уровней ");
            }
        }

        public static void UnlockAchievement(Users user, string achievementName)
        {
            // Получение достижения по его названию
            Achievements achievements = DataBase.Base.Achievements.FirstOrDefault(a => a.Title == achievementName);
            
            // Проверка, разблокировал ли пользователь уже достижение
            if (user.UserAchievements.Any(ua => ua.Achievements.Title == achievementName))
            {
                // Пользователь уже разблокировал достижение, поэтому ничего не делаем
                return;
            }

            // Добавление разблокированного достижения в UserAchievements
            user.UserAchievements.Add(new UserAchievements
            {
                AchievementID = achievements.Id,
                UserId = user.Id,
                IsCompleted = true
            });

            // Сохранения достижения в БД
            DataBase.Base.SaveChanges();

            MessageBox.Show($"Вы получили достижение \"{achievementName}\"!");
        }
    }
}

