using System;
using System.Collections.Generic;

namespace SCPSLParamsLanguage
{
    public class PlayerSelector
    {
        private Func<List<Smod2.API.Player>, string, List<Smod2.API.Player>> tagPredicate;
        /// <summary>
        /// Requires tag predicate for tag() selector work.
        /// </summary>
        /// <param name="tagPredicate">Predicate.</param>
        public PlayerSelector(Func<List<Smod2.API.Player>, string, List<Smod2.API.Player>> tagPredicate)
        {
            this.tagPredicate = tagPredicate;
        }

        /// <summary>
        /// Doesn't requires tag predicate, but tag() selector will not work.
        /// </summary>
        public PlayerSelector() : this(null) { }

        /// <summary>
        /// Selects players by expression "arg".
        /// </summary>
        /// <param name="arg">Expression.</param>
        /// <returns>List of players.</returns>
        /// <exception cref="Exception"></exception>
        public List<Smod2.API.Player> Select(string arg)
        {
            Parser parser = new Parser(new Lexer(arg.ToLower()).LexicalAnalysis());
            return parser.Run(parser.ParseCode(), tagPredicate);
        }
    }
}
