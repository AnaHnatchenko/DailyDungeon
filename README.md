# ⚔️ DailyDungeon

[🇺🇦 Українська версія](#-українська-версія) | [🇬🇧 English Version](#-english-version)

---

## 🇺🇦 Українська версія

**DailyDungeon** — це десктопний Windows-додаток, який перетворює вашу щоденну рутину на захопливий RPG-квест. Виконуйте завдання, формуйте корисні звички, заробляйте ігрові монети та налаштовуйте свій профіль!

### ✨ Основний функціонал
* **Трекінг завдань та звичок:** Зручне створення та керування щоденними справами з дедлайнами та тегами.
* **Система винагород:** Отримуйте монети залежно від обраної складності завдання.
* **Магазин та кастомізація:** Витрачайте важко зароблені монети на розблокування нових унікальних аватарів та фонів для профілю.
* **RPG-елементи:** Гейміфікація продуктивності, що мотивує виконувати більше рутинних справ.

### 🛠️ Технологічний стек
* **Мова:** C#
* **Інтерфейс (UI):** WPF (Windows Presentation Foundation)
* **База даних:** Microsoft SQL Server (LocalDB)
* **ORM:** Entity Framework (ADO.NET)
* **Інсталятор:** Inno Setup Compiler

### 🚀 Встановлення та запуск (Для гравців)
Найпростіший спосіб спробувати DailyDungeon — завантажити готовий інсталятор.

1. Перейдіть на сторінку проєкту: **[Завантажити з itch.io](https://anahnatchenko.itch.io/dailydungeon)**
2. Завантажте файл `DailyDungeonInstaller.exe`.
3. Запустіть інсталятор. 
> **Важливо:** Windows попросить надати права Адміністратора під час встановлення. Це необхідно **виключно** для безпечного встановлення рушія бази даних (Microsoft SQL Server LocalDB), який зберігає ваш прогрес локально на комп'ютері.
4. Запустіть гру, створіть акаунт і почніть свій перший квест!

### 💻 Для розробників (Збірка з вихідного коду)
Якщо ви хочете скомпілювати проєкт самостійно:

1. Склонуйте репозиторій:
   `git clone https://github.com/AnaHnatchenko/DailyDungeon.git`
2. Відкрийте файл `.sln` у Visual Studio 2022.
3. Переконайтеся, що у вас встановлено **SQL Server Express LocalDB**.
4. Відновіть NuGet-пакети (Entity Framework).
5. Натисніть `Build -> Rebuild Solution` та запустіть проєкт.

---

## 🇬🇧 English Version

**DailyDungeon** is a Windows desktop application that turns your daily routine into an exciting RPG quest. Complete tasks, build healthy habits, earn in-game coins, and customize your character profile!

> **Note:** The application UI is currently available only in **Ukrainian**.

### ✨ Key Features
* **Task & Habit Tracking:** Easily manage your daily to-do list with deadlines and custom tags.
* **Dynamic Rewards:** Earn coins based on the complexity of your tasks.
* **Shop & Customization:** Spend your hard-earned coins in the shop to unlock unique avatars and profile backgrounds.
* **RPG Elements:** Gamify your productivity and turn boring chores into a rewarding experience.

### 🛠️ Tech Stack
* **Language:** C#
* **Framework (UI):** WPF (Windows Presentation Foundation)
* **Database:** Microsoft SQL Server (LocalDB)
* **ORM:** Entity Framework (ADO.NET)
* **Installer:** Inno Setup Compiler

### 🚀 How to Install and Play
The easiest way to try DailyDungeon is by downloading the compiled installer.

1. Visit the project page: **[Download on itch.io](https://anahnatchenko.itch.io/dailydungeon)**
2. Download the `DailyDungeonInstaller.exe` file.
3. Run the installer. 
> **Important:** Windows will ask for Administrator privileges during installation. This is strictly required to install the local database engine (Microsoft SQL Server LocalDB) that saves your progress securely on your PC.
4. Launch the app, create an account, and start your quest!

### 💻 For Developers (Building from source)
If you want to build the project yourself:

1. Clone the repository:
   `git clone https://github.com/AnaHnatchenko/DailyDungeon.git`
2. Open the `.sln` file in Visual Studio 2022.
3. Make sure you have **SQL Server Express LocalDB** installed on your machine.
4. Restore NuGet packages (Entity Framework).
5. Click `Build -> Rebuild Solution` and run the project.

---
*Created by Anastasia*
