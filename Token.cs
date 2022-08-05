namespace SCPSLParamsLanguage
{
    internal class Token
    {
        public TokenType type { get; private set; }
        public string text { get; private set; }
        private int pos;

        public Token(TokenType type, string text, int pos)
        {
            this.type = type;
            this.text = text;
            this.pos = pos;
        }
    }
}
