namespace SCPSLParamsLanguage.AST
{
    internal class BinOpNode : ExpressionNode
    {
        public Token op { get; private set; }
        public ExpressionNode leftOp { get; private set; }
        public ExpressionNode rightOp { get; private set; }
        public BinOpNode(Token op, ExpressionNode leftOp, ExpressionNode rightOp)
        {
            this.op = op;
            this.leftOp = leftOp;
            this.rightOp = rightOp;
        }
    }
}
