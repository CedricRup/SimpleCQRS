using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using NUnit.Framework;
using Raven.Client.Document;
using YourDomain;
using YourDomain.Aggregates;

namespace Tests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Tests()
        {
            var glop = Enumerable.Range(0, 1000).Select(_ =>
            {
                var first = GetRandom.FirstName();
                var last = GetRandom.LastName();
                var company = GetRandom.Usa.State();
                var email =
                    string.Format("{0}.{1}@{2}.com",
                                  first, last, company);
                var password =
                    GetRandom.LowerCaseString(10);
                return new Account(email, first, last,
                                   password);
            });

            var documentore = new DocumentStore { Url = "http://localhost:8080/databases/CQRSSimpleExample" };
            documentore.Initialize();
            using (var session = documentore.OpenSession())
            {
                foreach (var singleObjectBuilder in glop)
                {
                    session.Store(singleObjectBuilder);
                }
                session.SaveChanges();
            }
        }

        public IEnumerable<Character> Characters()
        {
            yield return new Character{Name = "Cersei Lannister",Allegiance = Allegiance.Lannister};
            yield return new Character { Name = "Tyrion Lannister", Allegiance = Allegiance.Lannister };
            yield return new Character { Name = "Jaime Lannister", Allegiance = Allegiance.Lannister };
            yield return new Character { Name = "Tywin Lannister", Allegiance = Allegiance.Lannister };
            yield return new Character { Name = "Stanis Baratheon", Allegiance = Allegiance.Baratheon };
            yield return new Character { Name = "Melisandre", Allegiance = Allegiance.Baratheon };
            yield return new Character { Name = "Daenarys Targaryen", Allegiance = Allegiance.Targaryen };
            yield return new Character { Name = "Jaqen H'ghar", Allegiance = Allegiance.Unknown };
            yield return new Character { Name = "Jorah Mormont", Allegiance = Allegiance.Targaryen };
            yield return new Character { Name = "Joffrey Baratheon", Allegiance = Allegiance.Lannister };
            yield return new Character { Name = "Arya Stark", Allegiance = Allegiance.Stark };
            yield return new Character { Name = "Jon Snow", Allegiance = Allegiance.NightWatch };
            yield return new Character { Name = "Robb Stark", Allegiance = Allegiance.Stark };
            yield return new Character { Name = "Eddard Stark", Allegiance = Allegiance.Stark };
            yield return new Character { Name = "Mance Ryder", Allegiance = Allegiance.Wildlings };
        }

        [Test]
        public void StoreCharacters()
        {

            var documentore = new DocumentStore { Url = "http://localhost:8080/databases/CQRSSimpleExample" };
            documentore.Initialize();
            using (var session = documentore.OpenSession())
            {


                foreach (var character in Characters())
                {
                    session.Store(character);
                }
                session.SaveChanges();

            }
        }

        [Test]
        public void Glip()
        {
            var random = new Random();
            var documentore = new DocumentStore { Url = "http://localhost:8080/databases/CQRSSimpleExample" };
            documentore.Initialize();
            using (var session = documentore.OpenSession())
            {
                var characters = session.Query<Character>().ToList();
                var ids = session.Query<Account>().Take(1003).Select(a => new { a.Id }).ToList();
                var randomPicker = new RandomItemPicker<string>(ids.Select(a => a.Id).ToList(), new RandomGenerator(new Random()));


                var votes = from character in characters
                            from number in Enumerable.Range(0, random.Next(1000))
                            select new Vote {CharacterId = character.Id, UserId = randomPicker.Pick(), Points = 1};
                foreach (var vote in votes)
                {
                    session.Store(vote);
                }
                
                session.SaveChanges();

                
            }
        }


    }

}


