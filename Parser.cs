using Smod2.API;
using SCPSLParamsLanguage.AST;
using System.Collections.Generic;
using System.Linq;

namespace SCPSLParamsLanguage
{
    internal class Parser
    {
        public List<Token> tokens;
        int pos = 0;

        public Parser(List<Token> tokens) => this.tokens = tokens;

        public Token Match(TokenType[] expected)
        {
            if (pos < tokens.Count)
            {
                Token currentToken = tokens[pos];
                if (expected.Any(x => x.name == currentToken.type.name))
                {
                    pos += 1;
                    return currentToken;
                }
            }
            return null;
        }
        public Token Require(TokenType[] expected)
        {
            Token token = Match(expected);
            if (token == null)
                throw new System.Exception($"{expected[0].name} is expected at {pos} position.");
            return token;
        }
        public List<Player> Run(ExpressionNode node, System.Func<List<Player>, string, List<Player>> tagPredicate)
        {
            if (node is StatementsNode stNode)
            {
                node = stNode.codeStrings[0];
            }
            try
            {
                if (node is BinOpNode biNode)
                {
#if DEBUG
                    Smod2.PluginManager.Manager.Logger.Info("PARAM", $"BiNode:\nop: {biNode.op.type.name}\nleftOp: {biNode.leftOp}\nrightOp: {biNode.rightOp}");
#endif
                    if (biNode.op.type == TokenType.tokenTypeList["ROLE"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Role int: {int.Parse(((NumberNode)biNode.rightOp).number.text)}");
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Role type: {(Smod2.API.RoleType)int.Parse(((NumberNode)biNode.rightOp).number.text)}");
#endif
                        return Run(biNode.leftOp, tagPredicate).Where(x => x.PlayerRole.RoleID == (RoleType)int.Parse(((NumberNode)biNode.rightOp).number.text) && !x.OverwatchMode).ToList();
                    }
                    else if (biNode.op.type == TokenType.tokenTypeList["TEAM"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Role int: {int.Parse(((NumberNode)biNode.rightOp).number.text)}");
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Team type: {(Smod2.API.TeamType)int.Parse(((NumberNode)biNode.rightOp).number.text)}");
#endif
                        return Run(biNode.leftOp, tagPredicate).Where(x => x.PlayerRole.Team == (TeamType)int.Parse(((NumberNode)biNode.rightOp).number.text) && !x.OverwatchMode).ToList();
                    }
                    else if (biNode.op.type == TokenType.tokenTypeList["TAG"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Tag: {((AnyNode)biNode.rightOp).any.text}");
#endif
                        if (tagPredicate == null)
                            throw new System.Exception("Tag predicate not implemented.");
                        return tagPredicate(Run(biNode.leftOp, tagPredicate).Where(x => !x.OverwatchMode).ToList(), ((AnyNode)biNode.rightOp).any.text);
                    }
                    else if (biNode.op.type == TokenType.tokenTypeList["RAND"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Rand leftOp: {biNode.leftOp}");
#endif
                        List<Player> list = Run(biNode.leftOp, tagPredicate).Where(x => !x.OverwatchMode).ToList();
                        List<Player> output = new List<Player>();
                        int count = int.Parse(((NumberNode)biNode.rightOp).number.text);
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Rand count: {count}");
#endif
                        byte[] bytes = new byte[count];
                        new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(bytes);
                        for (int i = 0; i < count && list.Count != 0; i++)
                        {
                            int randIndex = bytes[i] * list.Count / 255;
                            output.Add(list[randIndex]);
                            list.RemoveAt(randIndex);
                        }
                        return output;
                    }
                    else if (biNode.op.type == TokenType.tokenTypeList["RANK"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Rank leftOp: {biNode.leftOp}");
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Rank: {((AnyNode)biNode.rightOp).any.text}");
#endif
                        string selectorText = ((AnyNode)biNode.rightOp).any.text;
                        if (selectorText == "*")
                            return Run(biNode.leftOp, tagPredicate).Where(x => !string.IsNullOrEmpty(x.GetRankName()) && !x.OverwatchMode).ToList();
                        else if (selectorText == "!*")
                            return Run(biNode.leftOp, tagPredicate).Where(x => string.IsNullOrEmpty(x.GetRankName()) && !x.OverwatchMode).ToList();
                        else if (selectorText.StartsWith("!"))
                            return Run(biNode.leftOp, tagPredicate).Where(x => x.GetRankName() != selectorText.Substring(1) && !x.OverwatchMode).ToList();
                        else
                            return Run(biNode.leftOp, tagPredicate).Where(x => x.GetRankName() == selectorText && !x.OverwatchMode).ToList();
                    }
                }
                if (node is AllNode)
                {
#if DEBUG
                    Smod2.PluginManager.Manager.Logger.Info("PARAM", "All node");
#endif
                    return Smod2.PluginManager.Manager.Server.GetPlayers().ToList();
                }
                if (node is UnOpNode unNode)
                {
                    if (unNode.op.type == TokenType.tokenTypeList["NAME"])
                    {
#if DEBUG
                        Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Name: {((AnyNode)unNode.operand).any.text}");
#endif
                        return new List<Player>() { Smod2.PluginManager.Manager.Server.GetPlayers().First(x => x.Name.StartsWith(((AnyNode)unNode.operand).any.text)) };
                    }
                }
                if (node is PlayerNode playerNode)
                {
#if DEBUG
                    Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Player node: {playerNode.playerID.text}");
#endif
                    return new List<Player>() { Smod2.PluginManager.Manager.Server.GetPlayers().First(x => x.UserID == playerNode.playerID.text) };
                }
                if (node is NumberNode numNode)
                {
#if DEBUG
                    Smod2.PluginManager.Manager.Logger.Info("PARAM", $"Num node: {int.Parse(numNode.number.text)}");
#endif
                    return new List<Player>() { Smod2.PluginManager.Manager.Server.GetPlayer(int.Parse(numNode.number.text)) };
                }
                throw new System.Exception("Error!");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        public ExpressionNode ParseCode()
        {
            StatementsNode root = new StatementsNode();
            while (pos < tokens.Count)
            {
                ExpressionNode codeStringNode = ParseExpression();
                root.AddNode(codeStringNode);
            }
            return root;
        }
        private ExpressionNode ParseExpression()
        {
            if (Match(new TokenType[] { TokenType.tokenTypeList["NUMBER"] }) != null)
            {
                pos -= 1;
                NumberNode node = (NumberNode)ParseNumber();
                return node;
            }
            else if (Match(new TokenType[] { TokenType.tokenTypeList["PLAYER"] }) != null)
            {
                pos -= 1;
                PlayerNode node = (PlayerNode)ParsePlayer();
                return node;
            }
            else if (Match(new TokenType[] { TokenType.tokenTypeList["ALL"] }) != null)
            {
                pos -= 1;
                AllNode node = (AllNode)ParseAll();
                return node;
            }
            else if (Match(new TokenType[] { TokenType.tokenTypeList["ANY"] }) != null)
            {
                pos -= 1;
                AnyNode node = (AnyNode)ParseAny();
                return node;
            }
            return ParseFormula();
            throw new System.Exception($"Operator expected at {pos} position.");
        }
        private ExpressionNode ParseFormula()
        {
            Token op = Match(new TokenType[] { TokenType.tokenTypeList["NAME"], TokenType.tokenTypeList["RAND"], TokenType.tokenTypeList["RANK"], TokenType.tokenTypeList["ROLE"], TokenType.tokenTypeList["TEAM"], TokenType.tokenTypeList["TAG"] });
            if (op != null)
            {
                if (Require(new TokenType[] { TokenType.tokenTypeList["LPAR"] }) != null)
                {
                    if (Match(new TokenType[] { TokenType.tokenTypeList["NAME"] }) != null)
                    {
                        AnyNode name = (AnyNode)ParseAny();
                        return new UnOpNode(op, name);
                    }
                    ExpressionNode leftOp = ParseExpression();
                    Require(new TokenType[] { TokenType.tokenTypeList["COMMA"] });
                    if (Match(new TokenType[] { TokenType.tokenTypeList["TAG"] }) != null)
                    {
                        AnyNode tagName = (AnyNode)ParseAny();
                        return new BinOpNode(op, leftOp, tagName);
                    }
                    if (Match(new TokenType[] { TokenType.tokenTypeList["RANK"] }) != null)
                    {
                        AnyNode rankName = (AnyNode)ParseAny();
                        return new BinOpNode(op, leftOp, rankName);
                    }
                    ExpressionNode rightOp = ParseExpression();
                    Require(new TokenType[] { TokenType.tokenTypeList["RPAR"] });
                    return new BinOpNode(op, leftOp, rightOp);
                }
            }
            throw new System.Exception($"Operator expected at {pos} position.");
        }

        private ExpressionNode ParseNumber()
        {
            Token num = Match(new TokenType[] { TokenType.tokenTypeList["NUMBER"] });
            if (num != null)
                return new NumberNode(num);
            throw new System.Exception($"Number expected at {pos} position.");
        }
        private ExpressionNode ParsePlayer()
        {
            Token player = Match(new TokenType[] { TokenType.tokenTypeList["PLAYER"] });
            if (player != null)
                return new PlayerNode(player);
            throw new System.Exception($"UserID expected at {pos} position.");
        }
        private ExpressionNode ParseAll()
        {
            Token all = Match(new TokenType[] { TokenType.tokenTypeList["ALL"] });
            if (all != null)
                return new AllNode(all);
            throw new System.Exception($"* expected at {pos} position.");
        }
        private ExpressionNode ParseAny()
        {
            Token any = Match(new TokenType[] { TokenType.tokenTypeList["ANY"] });
            if (any != null)
                return new AnyNode(any);
            throw new System.Exception($"\"[^) ]+\" regex string match expected at {pos} position.");
        }
    }
}
