namespace SCPSLParamsLanguage.AST
{
    internal class NumberNode : ExpressionNode
    {
        public Token number { get; private set; }
        public NumberNode(Token number) => this.number = number;
    }
}
