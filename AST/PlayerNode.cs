namespace SCPSLParamsLanguage.AST
{
    internal class PlayerNode : ExpressionNode
    {
        public Token playerID { get; private set; }
        public PlayerNode(Token playerID) => this.playerID = playerID;
    }
}
