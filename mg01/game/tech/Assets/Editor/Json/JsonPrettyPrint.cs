/* ==============================================================================
 * JsonPrettyPrint
 * @author jr.zeng
 * 2017/12/4 15:43:37
 * ==============================================================================*/

using System.Text;
using System.Collections.Generic;


namespace Edit
{
    public class JsonPrettyPrint
    {
        #region class members
        const string Space = " ";
        const int DefaultIndent = 0;
        const string Indent = Space + Space + Space + Space;
        static readonly string NewLine = "\n";
        #endregion

        static bool inDoubleString = false;
        static bool inSingleString = false;
        static bool inVariableAssignment = false;
        static char prevChar = '\0';

        static Stack<JsonContextType> context = new Stack<JsonContextType>();

        private enum JsonContextType
        {
            Object, Array
        }

        static void BuildIndents(int indents, StringBuilder output)
        {
            indents += DefaultIndent;
            for (; indents > 0; indents--)
                Append(output, Indent);
        }

        static bool InString()
        {
            return inDoubleString || inSingleString;
        }


        static void Append(StringBuilder sb, string str)
        {
            sb.Append(str);
        }

        static void Append(StringBuilder sb, char c)
        {
            sb.Append(c);
        }

        public static string Format(string input)
        {
            // Clear all states
            inDoubleString = false;
            inSingleString = false;
            inVariableAssignment = false;
            prevChar = '\0';
            context.Clear();

            var output = new StringBuilder(input.Length * 2);
            char c;

            for (int i = 0; i < input.Length; i++)
            {
                c = input[i];

                switch (c)
                {
                    case '[':
                    case '{':
                        if (!InString())
                        {
                            if (inVariableAssignment || (context.Count > 0 && context.Peek() != JsonContextType.Array))
                            {
                                Append(output, NewLine);
                                BuildIndents(context.Count, output);
                            }
                            Append(output, c);
                            context.Push(JsonContextType.Object);
                            Append(output, NewLine);
                            BuildIndents(context.Count, output);
                        }
                        else
                            Append(output, c);

                        break;

                    case ']':
                    case '}':
                        if (!InString())
                        {
                            Append(output, NewLine);
                            context.Pop();
                            BuildIndents(context.Count, output);
                            Append(output, c);
                        }
                        else
                            Append(output, c);

                        break;
                    case '=':
                        Append(output, c);
                        break;

                    case ',':
                        Append(output, c);

                        if (!InString())
                        {
                            BuildIndents(context.Count, output);
                            Append(output, NewLine);
                            BuildIndents(context.Count, output);
                            inVariableAssignment = false;
                        }

                        break;

                    case '\'':
                        if (!inDoubleString && prevChar != '\\')
                            inSingleString = !inSingleString;

                        Append(output, c);
                        break;

                    case ':':
                        if (!InString())
                        {
                            inVariableAssignment = true;
                            Append(output, Space);
                            Append(output, c);
                            Append(output, Space);
                        }
                        else
                            Append(output, c);

                        break;

                    case '"':
                        if (!inSingleString && prevChar != '\\')
                            inDoubleString = !inDoubleString;

                        Append(output, c);
                        break;
                    case ' ':
                        if (InString())
                            Append(output, c);
                        break;

                    default:
                        Append(output, c);
                        break;
                }
                prevChar = c;
            }

            return output.ToString();
        }

    }

}