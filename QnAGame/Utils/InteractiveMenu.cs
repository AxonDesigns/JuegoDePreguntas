using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnAGame.Utils
{
    internal class InteractiveMenu
    {
        private string _title;
        private string[] _options;
        private int _currentIndex;
        private ConsoleKeyInfo _key;
        private ConsoleColor _bgColor, _fgColor;
        private int cursorX, cursorY;

        public Action<int> OnSelect;

        public InteractiveMenu(string title, string[] options)
        {
            _title = title;
            _options = options;
            _currentIndex = 0;
            _bgColor = ConsoleColor.White;
            _fgColor = ConsoleColor.Black;
        }

        public InteractiveMenu(string title, string[] options, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            _title = title;
            _options = options;
            _currentIndex = 0;
            _bgColor = bgColor;
            _fgColor = fgColor;
        }

        public int Show()
        {
            Console.CursorVisible = false;
            Console.WriteLine(_title);
            cursorX = Console.CursorLeft;
            cursorY = Console.CursorTop;
            var result = Menu(_options, _currentIndex);

            while ((_key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                switch (_key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_currentIndex - 1 < 0) continue;
                        _currentIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (_currentIndex + 1 > _options.Length - 1) continue;
                        _currentIndex++;
                        break;
                    default:
                        break;
                }

                Console.CursorLeft = cursorX;
                Console.CursorTop = cursorY;
                result = Menu(_options, _currentIndex);
            }
            Console.CursorVisible = true;
            OnSelect?.Invoke(_currentIndex);
            return _currentIndex;
        }

        private string Menu(string[] items, int targetIndex)
        {
            var selection = string.Empty;

            for (int i = 0; i < items.Length; i++)
            {
                if(i == targetIndex)
                {
                    Console.ForegroundColor = _fgColor;
                    Console.BackgroundColor = _bgColor;
                    Console.WriteLine("==> " + items[i]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    selection = items[i];
                }
                else
                {
                    Console.CursorLeft = 0;
                    Console.WriteLine("- " + items[i] + "    ");
                }
            }
            return selection;
        }
    }
}
