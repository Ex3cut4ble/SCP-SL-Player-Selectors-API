namespace SCPSLParamsLanguage.AST
{
    internal class AnyNode : ExpressionNode
    {
        public Token any { get; private set; }
        public AnyNode(Token any) => this.any = any;
    }
}
