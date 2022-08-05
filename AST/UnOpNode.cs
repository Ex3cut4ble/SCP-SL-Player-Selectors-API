namespace SCPSLParamsLanguage.AST
{
    internal class UnOpNode : ExpressionNode
    {
        public Token op { get; private set; }
        public ExpressionNode operand { get; private set; }
        public UnOpNode(Token op, ExpressionNode operand)
        {
            this.op = op;
            this.operand = operand;
        }
    }
}
