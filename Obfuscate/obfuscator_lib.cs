using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscate
{
    class obfuscator_lib
    {
        // Создаем массив символов операторов
        string[] operatorSymbols = { ",", ";", ".", "+", "-", "*", "^", "&", "=", "~", "!", "/", "<", ">", "(", ")", "[", "]", "|", "%", "?", "\'", "\"", ":", "{", "}" };
        // Созд 
        char[] literals = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };
        char[] startLiterals = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_' };
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        // Данные с элементами видов данных
        List<string> dataTypes = new List<string>
        {
            "char",
            "const",
            "enum",
            "int ",
            "short",
            "long",
            "signed",
            "struct",
            "unsigned",
            "bool"
        };
        // Данные с элементами зарезервированных системой словами
        List<string> reservedWords = new List<string>
        {
            "break",
            "case",
            "char",
            "const",
            "continue",
            "default",
            "do",
            "double",
            "else",
            "enum",
            "extern",
            "float",
            "for",
            "goto",
            "if",
            "int",
            "long",
            "namespace",
            "static",
            "class",
            "void main",
            "return",
            "args",
            "main",
            "Main",
            "void",
            "short",
            "signed",
            "sizeof",
            "static",
            "struct",
            "switch",
            "unsigned",
            "void",
            "while",
        };
        // Исходный код программы
        private StringBuilder code;
        // Копия исходного кода программы
        private StringBuilder codeClone;
        // Конструктор класса
        public obfuscator_lib(StringBuilder InputCode)
        {
            code = InputCode;
        }
        // Функция, удаляющая комментарии в исходном коде
        private void RemoveComments()
        {
            StringBuilder currentChars = new StringBuilder();
            int deleteLength = 2;
            for (int i = 0; i < code.Length - 2; i++)
            {
                deleteLength = 2;
                if ("//" == code[i].ToString() + code[i + 1].ToString())
                {
                    for (int j = i + 2; j < code.Length - 2; j++)
                    {
                        deleteLength++;
                        if (code[j] == '\n')
                            break;
                        if (code[j].ToString() + code[j + 1].ToString() == "\r\n")
                        {
                            deleteLength++;
                            break;
                        }
                    }
                    code.Remove(i, deleteLength);
                    i = 0;
                    continue;
                }
                if (code[i].ToString() + code[i + 1].ToString() == "/*")
                {
                    for (int j = i + 2; j < code.Length - 2; j++)
                    {
                        deleteLength++;
                        if (code[j].ToString() + code[j + 1].ToString() == "*/")
                        {
                            deleteLength++;
                            break;
                        }
                    }
                    code.Remove(i, deleteLength);
                    i = 0;
                    continue;
                }
            }
        }
        // Удаляем пустое место, так называемое двойным пробелом
        private void RemoveEmptySpaces()
        {
            for (int i = 0; i < code.Length - 2; i++)
            {
                if (code[i].ToString() + code[i + 1].ToString() == "  ")
                {
                    code.Remove(i + 1, 1);
                    i = 0;
                    continue;
                }
            }
        }
        // Удаляем символы форматирования, такие как переход на новую строку, символ табуляции, и символ возврата каретки
        private void RemovingFormatSymbols()
        {
            // Удаляем знаки табуляции
            code.Replace("\t", " ");
            // Удаляем знак переноса строки
            code.Replace("\n", " ");
            // Удаляем символ возврата каретки в начальное положение
            code.Replace("\r", " ");
            code.Replace("\r\n", " ");
            // Удаляем пробелы возле служебных символов
            for (int j = 0; j < operatorSymbols.Length - 1; j++)
            {
                // Удаляем пробелы после служебных символов
                for (int i = 0; i < code.Length - 2; i++)
                {
                    IgnoreQuotes(ref i);
                    if (i == code.Length - 1)
                    {
                        Obfuscate.MainWindow.ShowExceptionMessageBox();
                        return;
                    }
                    if ((operatorSymbols[j] == code[i].ToString()) && code[i + 1].ToString() == " ")
                    {
                        code.Remove(i + 1, 1);
                        i = 0;
                        continue;
                    }
                }
                // Удаляем пробелы перед служебными символами
                for (int k = 1; k < code.Length - 2; k++)
                {
                    IgnoreQuotes(ref k);

                    if ((operatorSymbols[j] == code[k].ToString()) &&
                          (" " == code[k - 1].ToString()))
                    {
                        code.Remove(k - 1, 1);
                        k = 1;
                        continue;
                    }
                }
            }
        }
        // Проверка на начальный литералл функции и переменых
        private bool isStartLiteral(char ch)
        {
            foreach (char i in startLiterals)
            {
                if (ch == i)
                    return true;
            }
            return false;
        }
        // Проверяет данный символ кода на литерал
        private bool IsLiteral(char c)
        {
            foreach (char i in literals)
            {
                if (i == c) return true;
            }
            return false;
        }
        // Проверяет, явялется ли данное слово зарезервированным
        private bool reservedWord(string s)
        {
            foreach (string str in reservedWords)
            {
                if (s == str) return true;
            }
            return false;
        }
        // Проверяет является ли данный символ оператором
        private bool isOperator(char ch)
        {
            foreach (string s in operatorSymbols)
                if (ch.ToString() == s) return true;
            return false;
        }
        // Проверяет является ли данный символ цифрой
        private bool isNumber(char c)
        {
            foreach (char ch in numbers)
                if (c == ch) return true;
            return false;

        }
        // Обьявленные идентификаторы
        private List<string> declaredFields;
        // Запысывает вхождение идентификаторов
        private List<int> declaredFieldsEntries;
        // Уникальные обьявленные илентификаторы
        private List<string> UniqueFields;
        // Исключения выражения в кавычках
        private void IgnoreQuotes(ref int i)
        {
            // Если очередной символ '"'
            if (code[i] == '"')
            {
                i++;
                while ((code[i] != '"') && (i < code.Length - 1))
                {
                    // Пока символ "\\"
                    if ('\\' == code[i])
                    {
                        i++;
                    }
                    i++;
                }
            }
            if ('\'' == code[i])
            {
                i++;
                if ('\\' == code[i])
                {
                    i++;
                }
                i += 2;
            }
        }
        // Исключили выражения в кавычках
        // Находит обьявленные идентификаторы
        private void FindDeclared()
        {
            // Только имена обьявленных идентификаторов
            UniqueFields = new List<string>();
            // Инициализация списка обьявленных идентификаторов
            declaredFields = new List<string>();
            // Инициализация списка вхождений обьявленных идентификаторов
            declaredFieldsEntries = new List<int>();
            List<string> vars = new List<string>();
            List<int> varsEntered = new List<int>();
            List<bool> isDeclaredVar = new List<bool>();
            List<string> funcs = new List<string>();
            List<int> funcsEnters = new List<int>();
            List<bool> isDeclaredFunc = new List<bool>();
            string tempName = "";
            int tempEntry = 0;
            for (int i = 0; i < code.Length - 1; i++)
            {
                IgnoreQuotes(ref i);
                // Находим функции и переменные
                if (isStartLiteral(code[i]) || code[i] == '#')
                {
                    tempName = code[i].ToString();
                    tempEntry = i;
                    i++;
                    while (IsLiteral(code[i]))
                    {
                        tempName += code[i];
                        i++;
                    }
                    // Выделение слова
                    if (reservedWord(tempName))
                    {
                        if ("#include" == tempName)
                        {
                            if ('<' == code[i] || '<' == code[i + 1])
                            {
                                while ('>' != code[i])
                                {
                                    i++;
                                }
                            }
                        }
                        // Чтобы не перескочить на следующий символ
                        i--;
                        continue;
                    }
                    // Проверка на функцию 
                    if (code[i] == '(')
                    {
                        funcs.Add(tempName);
                        funcsEnters.Add(tempEntry);
                        continue;
                    }
                    // Проверка на переменную
                    if ('(' != code[i])
                    {
                        vars.Add(tempName);
                        varsEntered.Add(tempEntry);
                        continue;
                    }
                }
                // Нашли функции и переменные
            }
            // Находим обьявленные функции
            for (int currentFunc = 0; currentFunc < funcsEnters.Count(); currentFunc++)
            {
                int i = 0;
                int openBracketsCount = 0;
                int closeBracketsCount = 0;
                for (i = funcsEnters[currentFunc] + funcs[currentFunc].Length; i < code.Length - 1; i++)
                {
                    // Подсчет открытых скобок
                    if (code[i] == '(') openBracketsCount++;
                    // ПОдсчет закрытых скобок
                    if (code[i] == ')') closeBracketsCount++;
                    // Подсчет смвола ';'
                    if (code[i] == ';' || code[i] == '{')
                        break;
                }
                // Если это был простой вызов
                if (code[i] == ';' || openBracketsCount != closeBracketsCount)
                {
                    isDeclaredFunc.Add(false);
                    continue;
                }
                // Если функция обьявлена
                if (code[i] == '{')
                {
                    isDeclaredFunc.Add(true);
                    continue;
                }
                // Нашли обьявления функции
                // ЗАписываем вхождения функции
                // Запись в список каждого идентификатора, который обьявлен
            }
            for (int i = 0; i < funcs.Count; i++)
            {
                if (isDeclaredFunc[i])
                    UniqueFields.Add(funcs[i]);
            }
            // Проход по всему списку вхождений
            for (int funcCounter = 0; funcCounter < funcs.Count; funcCounter++)
            {
                //Проход по каждому имени обьявленного идентификатора
                foreach (string s in UniqueFields)
                {
                    if (s == funcs[funcCounter])
                    {
                        declaredFields.Add(funcs[funcCounter]);
                        declaredFieldsEntries.Add(funcsEnters[funcCounter]);
                    }
                }
            }
            //Записали вхождения функции
            //Находим обьявленные переменные
            for (int currentVar = 0; currentVar < varsEntered.Count; currentVar++)
            {
                string tempStr = "";
                string[] tempArr;
                for (int i = varsEntered[currentVar]; i >= 0; i--)
                {
                    if (code[i] == '{' || code[i] == '(' || code[i] == ';')
                        break;

                    tempStr += code[i];
                }
                string t = "";
                for (int i = tempStr.Length - 1; i >= 0; i--)
                {
                    t += tempStr[i];
                }
                tempStr = t;
                List<char> splitSymbols = new List<char>();
                splitSymbols.Add(' ');
                foreach (var i in operatorSymbols)
                    splitSymbols.Add(i[0]);
                tempArr = tempStr.Split(splitSymbols.ToArray());
                bool d = false;
                foreach (var word in tempArr)
                {
                    foreach (var dataType in dataTypes)
                    {
                        if (word == dataType)
                        {
                            d = true;
                            break;
                        }
                    }
                }
                isDeclaredVar.Add(d);
            }
            // Нашли обьявленные переменные
            //------------------------------
            //------------------------------
            // Записываем вхождения переменных
            // Запись в список  каждого идентификатора, который обьявлен
            for (int i = 0; i < vars.Count - 1; i++)
            {
                if (isDeclaredVar[i])
                    UniqueFields.Add(vars[i]);
            }
            // ПРоход по всему списку вхождений
            for (int varCounter = 0; varCounter < vars.Count; varCounter++)
            {
                foreach (string s in UniqueFields)
                {
                    // Если таковое не встречается - записываем его и его вхождения в список идентификатора
                    if (s == vars[varCounter])
                    {
                        declaredFields.Add(vars[varCounter]);
                        declaredFieldsEntries.Add(varsEntered[varCounter]);
                    }
                }
            }
            // Записали вхождения переменных
            //------------------------------
            //------------------------------
            // Сортировка списка идентификаторов
            for (int i = declaredFieldsEntries.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (declaredFieldsEntries[j] > declaredFieldsEntries[j + 1])
                    {
                        tempEntry = declaredFieldsEntries[j];
                        tempName = declaredFields[j];
                        declaredFieldsEntries[j] = declaredFieldsEntries[j + 1];
                        declaredFields[j] = declaredFields[j + 1];
                        declaredFieldsEntries[j + 1] = tempEntry;
                        declaredFields[j + 1] = tempName;
                    }
                }
            }
            vars.Clear();
            varsEntered.Clear();
            isDeclaredFunc.Clear();
            funcs.Clear();
            funcsEnters.Clear();
            isDeclaredFunc.Clear();
        }
        // Заменяет обьявленные функции и переменные на нелогичные
        //------------------------------
        //------------------------------
        private void RenameVar()

        {
            // Новые имена обьявленных идентификаторов
            string[] newUniqueIds = new string[UniqueFields.Count];
            // Максимальная длина идентификатора
            int MAXIDLENGTH = 10;
            // Вычисление макс. длины идентификатора
            foreach (string s in UniqueFields)
            {
                if (s.Length > MAXIDLENGTH) MAXIDLENGTH = s.Length;
            }
            // Создание новых нелогичных идентификаторов
            Random rand = new Random();
            for (int idIter = 0; idIter < UniqueFields.Count; idIter++)
            {
                string tempID = "";
                // Стартовый символ
                tempID += startLiterals[rand.Next(0, startLiterals.Length - 1)];
                // Оставшиеся символы
                for (int i = 1; i < MAXIDLENGTH; i++)
                {
                    tempID += literals[rand.Next(0, literals.Length - 1)];
                }
                // Проверка на одинаковые идентификаторы
                bool eq = false;
                for (int l = 0; l < newUniqueIds.Length - 1; l++)
                {
                    if (tempID == newUniqueIds[l])
                    {
                        eq = true;
                        break;
                    }
                }
                if (eq)
                {
                    idIter--;
                    continue;
                }
                newUniqueIds[idIter] = tempID;
            }
            // Массив заменяемых идентификаторов
            string[] replIds = new string[declaredFields.Count];
            // Прохождение по объявленным идентификаторам
            for (int i = 0; i < declaredFields.Count; i++)
            {
                // Прохождение по уникальным именам идентификаторов
                for (int j = 0; j < UniqueFields.Count; j++)
                {
                    // Если такой идентификатор встречается, то в массив заменяемых
                    // вставляем на его место обфусцированную версию
                    if (declaredFields[i] == UniqueFields[j])
                        replIds[i] = newUniqueIds[j];
                }
            }
            // Вхождения новых идентификаторов
            int[] entrIds = declaredFieldsEntries.ToArray();
            // Замена идентификаторов
            for (int i = declaredFieldsEntries.Count - 1; i >= 0; i--)
            {
                code.Remove(declaredFieldsEntries[i], declaredFields[i].Length);
                code.Insert(declaredFieldsEntries[i], replIds[i]);
            }
        }
        // Целочисленные константы
        List<string> intConst = new List<string>();
        // Вхождения целочисленных констант
        List<int> iconstEntries = new List<int>();
        // Поиск констант
        private void ConvertToHexadecimal()
        {
            string tempName = "";
            int tempEntry = 0;
            for (int i = 1; i < code.Length - 2; i++)
            {
                IgnoreQuotes(ref i);
                // Находим константы
                if (isNumber(code[i]) && code[i] != '0' && !IsLiteral(code[i - 1]) && code[i - 1] != '.' && code[i + 1] != '.')
                {
                    tempName = code[i].ToString();
                    tempEntry = i;
                    i++;
                    while (isNumber(code[i]))
                    {
                        tempName += code[i];
                        i++;
                    }
                    // Добавление целочисленной константы
                    intConst.Add(tempName);
                    iconstEntries.Add(tempEntry);
                    continue;
                }
            }
        }
        // Заменяет константы из 10-ричной СС в 16-ричную
        private void ConvertConst()
        {
            // Массив конвертированных в 16-ричную СС значения констант
            string[] convertIntConst = new string[intConst.Count];
            // Перевод из 10 в 16 СС
            for (int iconsIter = 0; iconsIter < intConst.Count; iconsIter++)
            {
                string tempIconst = "0x";
                // Стартовый символ
                tempIconst += Convert.ToString(Convert.ToInt32(intConst[iconsIter]), 16);
                convertIntConst[iconsIter] = tempIconst;
            }
            // Замена констант
            for (int i = iconstEntries.Count - 1; i >= 0; i--)
            {
                code.Remove(iconstEntries[i], intConst[i].Length);
                code.Insert(iconstEntries[i], convertIntConst[i]);
            }
            intConst.Clear();
            iconstEntries.Clear();
        }
        // Возращает обфусцированный код
        public StringBuilder GetObfuscatedCode()
        {
            // "Причёсывание" кода
            RemoveComments();
            // Удаляем множественные пробелы
            RemoveEmptySpaces();
            // Удаляем символы форматирования
            RemovingFormatSymbols();
            // Ещё раз удаляем множественные пробелы
            RemoveEmptySpaces();
            // Клонируем код
            codeClone = new StringBuilder();
            codeClone.Append(code.ToString());
            FindDeclared();
            RenameVar();
            // Переводим числовые значение в 16-ричную систему счисления
            ConvertToHexadecimal();
            ConvertConst();

            return code;
        }
    }
}