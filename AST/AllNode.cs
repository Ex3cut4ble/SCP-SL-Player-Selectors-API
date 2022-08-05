namespace SCPSLParamsLanguage.AST
{
    internal class AllNode : ExpressionNode
    {
        Token all;
        public AllNode(Token all) => this.all = all;
    }
}
