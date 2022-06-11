using System;
using System.Collections.Generic;
using System.Linq;
using QnAGame.Utils;
using QnAGame.Models;
using Newtonsoft.Json;
using System.IO;

namespace QnAGame
{
    internal class Program
    {
        private static GameSettings _gameSettings;
        private const string gamesPath = "Saved", gamesFileName = "saves.game";
        private static int _currentScore = 0;

        static void Main(string[] args)
        {
            _gameSettings = LoadGame($"{gamesPath}/{gamesFileName}");
            MainMenu();
        }

        private static void MainMenu()
        {
            Console.Clear();
            Utilities.ShowTitle("¡BIENVENIDO AL JUEGO TIPO Q&A DE BRAYAN HERNANDEZ!", ConsoleColor.Red, true);

            var options = new string[] {
                "Jugar la experiencia predeterminada",
                "Seleccionar juego",
                "Crea tu propio juego",
                "Ver tabla de posiciones",
                "Salir del juego" };

            var mainMenu = new InteractiveMenu("", options, ConsoleColor.DarkBlue, ConsoleColor.Cyan);
            switch (mainMenu.Show())
            {
                case 0:
                    StartGame(_gameSettings.Quizes[0]);
                    break;
                case 1:
                    SelectGame();
                    break;
                case 2:
                    CreateGame();
                    break;
                case 3:
                    ScoreTable();
                    break;
                case 4:
                    ExitGame();
                    break;
                default:
                    break;
            }
        }

        private static void StartGame(Quiz game)
        {
            Console.Clear();
            Utilities.ShowTitle("Recuerda que cada pregunta correcta es +10 puntos, pero por cada perdida -5, llega a -50 y pierdes!", ConsoleColor.White, true);
            if (new InteractiveMenu("¿Quieres continuar?", new string[] { "Si", "No" }).Show() == 1)
            { 
                MainMenu();
                return; 
            }

            DoQuiz(game);

            Utilities.ShowTitle($"¡Haz conseguido {_currentScore} puntos!", ConsoleColor.Yellow, true);

            if (new InteractiveMenu("¿Quieres guardar tu puntuaje?", new string[] { "Si", "No" }).Show() == 1)
            {
                MainMenu();
                return;
            }

            Console.Clear();
            var playerName = Utilities.ValidateInput($"{Utilities.CreateTitle("Guardando Jugador", true)}\nPon tu nombre: ", "ERROR: Debes ingresar algo!");

            _gameSettings.Players.Add(new Player(playerName, _currentScore));
            SaveGame(_gameSettings);
            MainMenu();
        }

        private static void DoQuiz(Quiz game)
        {
            Console.Clear();
            _currentScore = 0;
            foreach (var question in game.Questions)
            {
                Utilities.ShowTitle($"juego {game.Name}", ConsoleColor.Cyan, true);
                Console.WriteLine($"Score: {_currentScore}\n");

                var rightAnswer = question.Choices[0];
                var rand = new Random();
                var shuffledChoices = question.Choices.OrderBy(x => rand.Next()).ToList();

                var preguntaMenu = new InteractiveMenu(question.Title, shuffledChoices.ToArray());
                var answer = shuffledChoices[preguntaMenu.Show()];
                if (answer.Equals(rightAnswer))
                {
                    _currentScore += 5;
                    Utilities.ShowAlert("¡CORRECTO! +10", ConsoleColor.Green);
                }
                else
                {
                    _currentScore -= 5;
                    if (_currentScore <= -50)
                    {
                        Utilities.ShowAlert($"¡INCORRECTO {_currentScore}, HAS PERDIDO!", ConsoleColor.DarkRed); MainMenu(); return;
                    }
                    else
                    {
                        Utilities.ShowAlert("¡INCORRECTO! -5", ConsoleColor.Red);
                    }
                }

                Console.Clear();
            }
        }

        private static void SelectGame()
        {
            Console.Clear();
            var quizzes = _gameSettings.Quizes;
            if (quizzes.Count < 2)
            {
                Utilities.ShowAlert("¡No se encontraron juegos! ¡Empieza creando uno!", ConsoleColor.DarkRed);
                MainMenu();
                return;
            }

            var gameNames = new List<string>();
            for (int i = 0; i < quizzes.Count; i++)
            {
                if (i < 1) continue;
                gameNames.Add(quizzes[i].Name);
            }
            gameNames.Add("Regresar");

            Utilities.ShowTitle("Seleccion de juegos", ConsoleColor.Green, true);
            var menu = new InteractiveMenu("Selecciona un juego: ", gameNames.ToArray(), ConsoleColor.DarkGreen, ConsoleColor.Yellow);
            menu.OnSelect += (index) => {
                if (index >= quizzes.Count - 1) { MainMenu(); return; }
                StartGame(quizzes[index + 1]);
            };
            menu.Show();
        }

        private static void CreateGame()
        {
            Console.Clear();
            Utilities.ShowTitle("¡Ponle un nombre a tu juego!", ConsoleColor.DarkYellow, true);
            Console.Write("¿Cómo se va llamar?: ");
            var name = Console.ReadLine();
            Console.Clear();

            Utilities.ShowTitle($"Configurando {name}", ConsoleColor.DarkYellow, true);
            var categoriesCount = Utilities.ValidateInputInt($"¿Cuantas categorias tendra?: ",
                "El valor ingresado no es correcto, intentelo de nuevo");

            var categories = new List<string>();
            for (int i = 0; i < categoriesCount; i++)
            {
                while (true)
                {
                    Utilities.ShowTitle($"Configurando {name}", ConsoleColor.DarkYellow, true);
                    Console.Write($"Ingrese la categoria{(categoriesCount > 1 ? $" número {i + 1}/{categoriesCount}" : "")}: ");
                    categories.Add(Console.ReadLine());
                    if (new InteractiveMenu("\n¿Estás seguro?", new string[] { "Si", "No" }).Show() == 1) { Console.Clear(); continue; }
                    break;
                }

                Console.Clear();
            }

            Utilities.ShowTitle($"Configurando {name}", ConsoleColor.DarkYellow, true);
            var QPCCount = Utilities.ValidateInputInt($"¿Cuantas preguntas por categoria tendra?: ",
                "El valor ingresado no es correcto, intentelo de nuevo");

            var questions = FillQuestionsList(name, categories, QPCCount);

            _gameSettings.Quizes.Add(new Quiz(name, categories, questions));
            SaveGame(_gameSettings, true, name);
            MainMenu();
        }

        private static List<Question> FillQuestionsList(string gameName, List<string> categories, int QpCCount)
        {
            var questions = new List<Question>();

            for (int i = 0; i < categories.Count; i++)
            {
                for (int j = 0; j < QpCCount; j++)
                {
                    string title;
                    while (true)
                    {
                        Utilities.ShowTitle($"Configurando las preguntas de la categoria \"{categories[i]}\" de {gameName}", ConsoleColor.DarkYellow, true);
                        Console.Write($"¿Cuál será el título de la pregunta{(QpCCount > 1? $" {j + 1}/{QpCCount}" : "")}?: ");
                        title = Console.ReadLine();

                        if (new InteractiveMenu("\n¿Estás seguro?", new string[] { "Si", "No" }).Show() == 1) { Console.Clear(); continue; }
                        break;
                    }


                    var answers = new List<string>();
                    for (int k = 0; k < 4; k++)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(Utilities.CreateTitle($"Configurando las respuestas de la categoria \"{categories[i]}\" de {gameName}", true));
                            Console.Write($"¿Cúal sera la respuesta {(k == 0 ? "correcta" : "número " + (k + 1))}?: ");
                            answers.Add(Console.ReadLine());

                            if (new InteractiveMenu("\n¿Estás seguro?", new string[] { "Si", "No" }).Show() == 1) { Console.Clear(); continue; }
                            break;
                        }
                    }

                    questions.Add(new Question(title, categories[i], answers));
                    Console.Clear();
                }
            }

            return questions;
        }

        private static void ScoreTable()
        {
            Console.Clear();
            Utilities.ShowTitle("Tabla de Posiciones", ConsoleColor.DarkMagenta, true);

            if(_gameSettings.Players.Count < 1) { Utilities.ShowAlert("¡Aún no hay jugadores, sé el primero!", ConsoleColor.DarkRed); MainMenu(); }

            foreach (var player in _gameSettings.Players)
            {
                Utilities.ShowTitle($"{player.Name} || {player.Highscore} puntos", ConsoleColor.Yellow, true);
            }

            Console.ReadKey();
            MainMenu();
        }

        private static void ExitGame()
        {
            SaveGame(_gameSettings);
            Console.Clear();
            Utilities.ShowTitle("¡GRACIAS POR JUGAR, HASTA LA PROXIMA!", ConsoleColor.Green, true);
        }

        private static GameSettings LoadGame(string path)
        {
            GameSettings availableGame = new GameSettings();
            try
            {
                string text = File.ReadAllText(path);
                availableGame = JsonConvert.DeserializeObject<GameSettings>(text);
            }
            catch (Exception)  { }

            return availableGame;
        }

        private static void SaveGame(GameSettings game, bool showMessage = false, string newGameName = "New Game")
        {
            try
            {
                var gameJson = JsonConvert.SerializeObject(game, Formatting.Indented);
                Utilities.SaveFile(gamesPath, gamesFileName, gameJson);
            }
            catch (Exception)
            {
                Console.WriteLine(Utilities.CreateTitle($"ERROR: Juego {newGameName} no pudo ser guardado", true));
                Console.ReadKey();
                return;
            }

            if (showMessage) { Console.WriteLine(Utilities.CreateTitle($"¡Juego {newGameName} guardado con exito!", true)); Console.ReadKey(); }
        }
    }
}
