using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SCPSLParamsLanguage
{
    internal class Lexer
    {
        string code;
        int pos = 0;
        List<Token> tokenList = new List<Token>();
        public Lexer(string code) => this.code = code;
        public List<Token> LexicalAnalysis()
        {
            while (NextToken()) { }
            return tokenList.Where(x => x.type.name != TokenType.tokenTypeList["SPACE"].name).ToList();
        }
        private bool NextToken()
        {
            if (pos >= code.Length)
                return false;
            List<TokenType> tokenTypesList = TokenType.tokenTypeList.Values.ToList();
            for (int i = 0; i < tokenTypesList.Count; i++)
            {
                TokenType tokenType = tokenTypesList[i];
                Regex regex = new Regex("^" + tokenType.regex);
                string result = regex.Match(code.Substring(pos)).Value;
                if (!string.IsNullOrEmpty(result))
                {
                    Token token = new Token(tokenType, result, pos);
                    pos += result.Length;
                    tokenList.Add(token);
                    return true;
                }
            }
            throw new Exception($"An error was found at {pos} position.");
        }
    }
}
