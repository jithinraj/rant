﻿using System;
using System.Collections.Generic;
using System.Linq;

using Rant.Engine.Syntax;
using Rant.Stringes;

namespace Rant.Engine.Compiler.Parselets
{
    internal class FunctionParselet : Parselet
    {
        [TokenParser(R.RightSquare)]
        IEnumerable<Parselet> RightSquare(Token<R> token)
        {
            // TODO: is it 'tag' or 'method' or 'function' or what?
            compiler.SyntaxError(token, "Unexpected function terminator");
            yield break;
        }

        [TokenParser(R.LeftSquare)]
        IEnumerable<Parselet> LeftSquare(Token<R> token)
        {
            var tagToken = reader.ReadToken();
            switch (tagToken.ID)
            {
                default:
                    compiler.SyntaxError(tagToken, "Expected function name, subroutine, regex or a script.");
                    break;

                case R.Text:
                    yield return Parselet.GetParselet("FunctionText", tagToken, AddToOutput);
                    break;

                case R.Regex:
                    yield return Parselet.GetParselet("FunctionRegex", tagToken, AddToOutput);
                    break;

                case R.At:
                    AddToOutput(compiler.ReadExpression());
                    break;

                case R.Dollar:
                    yield return Parselet.GetParselet("FunctionSubroutine", tagToken, AddToOutput);
                    break;
            }
        }
    }
}
