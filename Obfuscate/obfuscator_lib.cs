using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscate
{
    class obfuscator_lib
    {
        string[] operatorSymbols = { ",", ";", ".", "+", "-", "*", "^", "&", "=", "~", "!", "/", "<", ">", "(", ")", "[", "]", "|", "%", "?", "\'", "\"", ":", "{", "}" };
        char[] literals = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };
        char[] startLiterals = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_' };
        char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

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
            "main",
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
        private StringBuilder code;
        private StringBuilder codeClone;
        public obfuscator_lib(StringBuilder InputCode)
        {
            code = InputCode;
        }

        private void RemoveComments()
        {
            StringBuilder currentChars = new StringBuilder();
            int deleteLength = 2;
            for(int i = 0;i<code.Length - 2; i++)
            {
                deleteLength = 2;
                if("//" == code[i].ToString() + code[i + 1].ToString())
                {
                    for(int j = i + 2; j < code.Length-2; j++)
                    {
                        deleteLength++;
                        if (code[j] == '\n')
                            break;
                        if(code[j].ToString()+code[j+1].ToString() == "\r\n")
                        {
                            deleteLength++;
                            break;
                        }
                    }
                    code.Remove(i, deleteLength);
                    i = 0;
                    continue;
                }
                if(code[i].ToString()+code[i+1].ToString() == "/*")
                {
                    for(int j = i+2;j<code.Length - 2; j++)
                    {
                        deleteLength++;
                        if(code[j].ToString()+code[j+1].ToString() == "*/")
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
        private void RemoveEmptySpaces()
        {
            for(int i = 0; i < code.Length - 2; i++)
            {
                if(code[i].ToString()+code[i+1].ToString() == "  ")
                {
                    code.Remove(i + 1, 1);
                    i = 0;
                    continue;
                }
            }
        }
        private void RemovingFormatSymbols()
        {
            code.Replace("\t", " ");
            code.Replace("\n", " ");
            code.Replace("\r", " ");
            code.Replace("\r\n", " ");
            for (int j = 0; j < operatorSymbols.Length-1; j++)
            {
                for (int i = 0; i < code.Length-2; i++)
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
                for (int k = 1; k < code.Length - 2; k++)
                {
                    // Исключаем выражения в кавычках и "<>"
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

        private bool isStartLiteral(char ch)
        {
            foreach(char i in startLiterals)
            {
                if (ch == i)
                    return true;
            }
            return false;
        }
        private bool IsLiteral(char c)
        {
            foreach (char i in literals)
            {
                if (i == c) return true;
            }
            return false;
        }
        private bool reservedWord(string s)
        {
            foreach(string str in reservedWords)
            {
                if (s == str) return true;
            }
            return false;
        }

        private bool isOperator(char ch)
        {
            foreach (string s in operatorSymbols)
                if (ch.ToString() == s) return true;
            return false;
        }
        private bool isNumber(char c)
        {
            foreach (char ch in numbers)
                if (c == ch) return true;
            return false;

        }
        private List<string> declaredFields;
        private List<int> declaredFieldsEntries;
        private List<string> UniqueFields;
        private void IgnoreQuotes(ref int i)
        {
            if(code[i] == '"')
            {
                i++;
                while((code[i] != '"') && (i < code.Length - 1))
                {
                    if('\\' == code[i])
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



        private void FindDeclared()
        {
            UniqueFields = new List<string>();
            declaredFields = new List<string>();
            declaredFieldsEntries = new List<int>();
            List<string> vars = new List<string>();
            List<int> varsEntered = new List<int>();
            List<bool> isDeclaredVar = new List<bool>();
            List<string> funcs = new List<string>();
            List<int> funcsEnters = new List<int>();
            List<bool> isDeclaredFunc = new List<bool>();
            string tempName = "";
            int tempEntry = 0;
            for(int i = 0; i < code.Length - 1; i++)
            {
                IgnoreQuotes(ref i);
                if(isStartLiteral(code[i])||code[i] == '#')
                {
                    tempName = code[i].ToString();
                    tempEntry = i;
                    i++;
                    while (IsLiteral(code[i])){
                        tempName += code[i];
                        i++;
                    }
                    if(reservedWord(tempName))
                    {
                        if("#include" == tempName)
                        {
                            if('<' == code[i]||'<' == code[i + 1])
                            {
                                while('>' != code[i])
                                {
                                    i++;
                                }
                            }
                        }
                        i--;
                        continue;
                    }
                    if(code[i] == '(')
                    {
                        funcs.Add(tempName);
                        funcsEnters.Add(tempEntry);
                        continue;
                    }
                    if ('(' != code[i])
                    {
                        vars.Add(tempName);
                        varsEntered.Add(tempEntry);
                        continue;
                    }
                }

            }
            for(int currentFunc = 0; currentFunc < funcsEnters.Count(); currentFunc++)
            {
                int i = 0;
                int openBracketsCount = 0;
                int closeBracketsCount = 0;
                for(i = funcsEnters[currentFunc] + funcs[currentFunc].Length; i < code.Length - 1; i++)
                {
                    if (code[i] == '(') openBracketsCount++;
                    if (code[i] == ')') closeBracketsCount++;
                    if (code[i] == ';' || code[i] == '{')
                        break;
                }
                if (code[i] == ';' || openBracketsCount != closeBracketsCount)
                {
                    isDeclaredFunc.Add(false);
                    continue;
                }
                if(code[i] == '{')
                {
                    isDeclaredFunc.Add(true);
                    continue;
                }

            }
            for(int i = 0; i < funcs.Count; i++)
            {
                if (isDeclaredFunc[i])
                    UniqueFields.Add(funcs[i]);
            }
            for(int funcCounter = 0; funcCounter < funcs.Count; funcCounter++)
            {
                foreach(string s in UniqueFields)
                {
                    if(s == funcs[funcCounter])
                    {
                        declaredFields.Add(funcs[funcCounter]);
                        declaredFieldsEntries.Add(funcsEnters[funcCounter]);
                    }
                }
            }
            for(int currentVar = 0; currentVar < varsEntered.Count; currentVar++)
            {
                string tempStr = "";
                string[] tempArr;
                for(int i = varsEntered[currentVar]; i >= 0; i--)
                {
                    if (code[i] == '{' || code[i] == '(' || code[i] == ';')
                        break;

                    tempStr += code[i];
                }
                string t = "";
                for(int i = tempStr.Length - 1; i >= 0; i--)
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
                foreach(var word in tempArr)
                {
                    foreach(var dataType in dataTypes)
                    {
                        if(word == dataType)
                        {
                            d = true;
                            break;
                        }
                    }
                }
                isDeclaredVar.Add(d);
            }

            for(int i = 0;i<vars.Count - 1; i++)
            {
                if (isDeclaredVar[i])
                    UniqueFields.Add(vars[i]);
            }
            for(int varCounter = 0; varCounter < vars.Count; varCounter++)
            {
                foreach (string s in UniqueFields)
                {
                    if(s == vars[varCounter])
                    {
                        declaredFields.Add(vars[varCounter]);
                        declaredFieldsEntries.Add(varsEntered[varCounter]);
                    }
                }
            }
            for(int i = declaredFieldsEntries.Count-1;i>=0;i--)
            {
                for(int j = 0; j < i; j++)
                {
                    if(declaredFieldsEntries[j]>declaredFieldsEntries[j+1])
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
        private void RenameVar()
        {
            string[] newUniqueIds = new string[UniqueFields.Count];
            int MAXIDLENGTH = 10;
            foreach(string s in UniqueFields)
            {
                if (s.Length > MAXIDLENGTH) MAXIDLENGTH = s.Length;
            }
            Random rand = new Random();
            for(int idIter = 0; idIter < UniqueFields.Count; idIter++)
            {
                string tempID = "";
                tempID += startLiterals[rand.Next(0, startLiterals.Length - 1)];
                for(int i = 1; i < MAXIDLENGTH; i++)
                {
                    tempID += literals[rand.Next(0, literals.Length - 1)];
                }
                bool eq = false;
                for(int l = 0;l<newUniqueIds.Length - 1; l++)
                {
                    if(tempID == newUniqueIds[l])
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
            string[] replIds = new string[declaredFields.Count];
            for(int i = 0; i < declaredFields.Count; i++)
            {
                for(int j = 0; j < UniqueFields.Count; j++)
                {
                    if (declaredFields[i] == UniqueFields[j])
                        replIds[i] = newUniqueIds[j];
                }
            }
            int[] entrIds = declaredFieldsEntries.ToArray();
            for(int i = declaredFieldsEntries.Count - 1; i >= 0; i--)
            {
                code.Remove(declaredFieldsEntries[i], declaredFields[i].Length);
                code.Insert(declaredFieldsEntries[i], replIds[i]);
            }
        }
        List<string> intConst = new List<string>();
        List<int> iconstEntries = new List<int>();
        private void ConvertToHexadecimal()
        {
            string tempName = "";
            int tempEntry = 0;
            for(int i = 1;i<code.Length - 2; i++)
            {
                IgnoreQuotes(ref i);
                if(isNumber(code[i])&&code[i]!='0'&&!IsLiteral(code[i-1])&&code[i-1]!= '.'&&code[i+1] != '.')
                {
                    tempName = code[i].ToString();
                    tempEntry = i;
                    i++;
                    while (isNumber(code[i]))
                    {
                        tempName += code[i];
                        i++;
                    }
                    intConst.Add(tempName);
                    iconstEntries.Add(tempEntry);
                    continue;
                }
            }
        }
        private void ConvertConst()
        {
            string[] convertIntConst = new string[intConst.Count];
            for(int iconsIter = 0; iconsIter < intConst.Count; iconsIter++)
            {
                string tempIconst = "0x";
                tempIconst += Convert.ToString(Convert.ToInt32(intConst[iconsIter]), 16);
                convertIntConst[iconsIter] = tempIconst;
            }
            for(int i = iconstEntries.Count - 1; i >= 0; i--)
            {
                code.Remove(iconstEntries[i], intConst[i].Length);
                code.Insert(iconstEntries[i], convertIntConst[i]);
            }
            intConst.Clear();
            iconstEntries.Clear();
        }
        public StringBuilder GetObfuscatedCode()
        {
                RemoveComments();
                RemoveEmptySpaces();
                RemovingFormatSymbols();
                RemoveEmptySpaces();
                codeClone = new StringBuilder();
                codeClone.Append(code.ToString());
                FindDeclared();
                RenameVar();
                ConvertToHexadecimal();
                ConvertConst();

            return code;
        }
        public List<string> GetIncludeLibs()
        {
            RemoveComments();
            RemoveEmptySpaces();
            RemovingFormatSymbols();
            RemoveEmptySpaces();
            List<string> Libs = new List<string>();
            const string INCL = "#include";
            string currLib = "";
            bool isLibExists = false;
            for (int i = 0; i < code.Length - (INCL.Length + 1); i++)
            {
                currLib = "";
                isLibExists = false;
                if(INCL == code.ToString().Substring(i, INCL.Length))
                {
                    i += INCL.Length;
                    if (code[i] == '<' || code[i + 1] == '<')
                        continue;
                    if (code[i] == '"')
                        i++;
                    if (code[i + 1] == '"')
                        i += 2;
                    while(code[i] != '"')
                    {
                        isLibExists = true;
                        currLib += code[i];
                        i++;
                    }
                }
                if (isLibExists)
                    Libs.Add(currLib);
            }
            return Libs;
        }

        public void RemoveLibs(string libName)
        {
            string currLib;
            bool isLibExists;
            const string INCL = "#include";
            int startIndex, length;
            for(int i = 0;i<code.Length - (INCL.Length + 1); i++)
            {
                currLib = "";
                isLibExists = false;
                startIndex = 0;
                length = 0;
                if(INCL == code.ToString().Substring(i, INCL.Length))
                {
                    startIndex = i;
                    i += INCL.Length;
                    if (code[i] == '<' || code[i + 1] == '<')
                        continue;
                    if (code[i] == '"')
                        i++;
                    if (code[i + 1] == '"')
                        i += 2;
                    while(code[i] != '"')
                    {
                        isLibExists = true;
                        currLib += code[i];
                        i++;
                    }
                    length = i - startIndex + 1;
                }
                if (isLibExists || (currLib == libName))
                    code.Remove(startIndex, length);
            }



        }

    }
}
