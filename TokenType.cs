using System.Collections.Generic;

namespace SCPSLParamsLanguage
{
    internal sealed class TokenType
    {
        public string name;
        public string regex;
        public TokenType(string name, string regex)
        {
            this.name = name;
            this.regex = regex;
        }
        public static readonly Dictionary<string, TokenType> tokenTypeList = new Dictionary<string, TokenType>()
        {
            { "PLAYER", new TokenType("PLAYER", "[0-9]{17}@steam|[0-9]{18}@discord") },
            { "NUMBER", new TokenType("NUMBER", "[0-9]*") },
            { "LPAR", new TokenType("LPAR", "\\(") },
            { "RPAR", new TokenType("RPAR", "\\)") },
            { "COMMA", new TokenType("COMMA", ",") },
            { "ALL", new TokenType("ALL", "\\*") },
            { "SPACE", new TokenType("SPACE", " ") },
            { "RAND", new TokenType("RAND", "rand") },
            { "RANK", new TokenType("RANK", "rank") },
            { "ROLE", new TokenType("ROLE", "role") },
            { "TEAM", new TokenType("TEAM", "team") },
            { "NAME", new TokenType("NAME", "name") },
            { "TAG", new TokenType("TAG", "tag") },
            { "ANY", new TokenType("ANY", "[^) ]+") }
        };
    }
}
