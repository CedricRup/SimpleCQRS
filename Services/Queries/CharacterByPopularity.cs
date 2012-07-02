using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using YourDomain;

namespace Services.Queries
{
    public class CharacterByPopularity : AbstractIndexCreationTask<Vote,CharacterByPopularity.Result>
    {
        public class Result
        {
            public string CharacterId { get; set; }
            public int Popularity { get; set; }
        }

        public class CharacterWithPopalurity
        {
            public string Name { get; set; }
            public int Popularity { get; set; }
        }



        public CharacterByPopularity()
        {
            Map = votes => from vote in votes
                                  select new {vote.CharacterId, Popularity = vote.Points};

            Reduce = votes => from vote in votes
                              group vote by vote.CharacterId into g
                              select new {CharacterId = g.Key, Popularity = g.Sum(v=>v.Popularity)};

            TransformResults = (database, votes) => from vote in votes //pour chaque resultat du reduce
                                                    let characterName = database.Load<Character>(vote.CharacterId) //on retrouve le document Character
                                                    select new { characterName.Name, vote.Popularity }; // et on utilise son nom dans le résultat

            Sort(p=>p.Popularity,SortOptions.Int);
        }
    }
}
