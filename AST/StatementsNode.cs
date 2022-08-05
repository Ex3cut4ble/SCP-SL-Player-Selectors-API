using System.Collections.Generic;

namespace SCPSLParamsLanguage.AST
{
    internal class StatementsNode : ExpressionNode
    {
        public List<ExpressionNode> codeStrings = new List<ExpressionNode>();
        public void AddNode(ExpressionNode node) => codeStrings.Add(node);
    }
}
