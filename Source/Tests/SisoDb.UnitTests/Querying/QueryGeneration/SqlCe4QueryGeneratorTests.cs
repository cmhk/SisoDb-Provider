using NUnit.Framework;
using SisoDb.Querying;
using SisoDb.SqlCe4;

namespace SisoDb.UnitTests.Querying.QueryGeneration
{
    [TestFixture]
    public class SqlCe4QueryGeneratorTests : QueryGeneratorTests
    {
        protected override IDbQueryGenerator GetQueryGenerator()
        {
            return new SqlCe4QueryGenerator();
        }

        [Test]
        public override void GenerateQuery_WithWhere_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhere_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select min(s.[Json]) [Json] from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "where (mem0.[IntegerValue] = @p0) " +
                "group by s.[StructureId];",
                sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "group by s.[StructureId] " +
                "order by mem0 Asc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithWhereAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "where (mem0.[IntegerValue] = @p0) " +
                "group by s.[StructureId] " +
                "order by mem0 Asc;",
                sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithTake_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTake_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select top(11) min(s.[Json]) [Json] from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId];",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithTakeAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTakeAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select top(11) min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "group by s.[StructureId] " +
                "order by mem0 Asc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithTakeAndWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithTakeAndWhereAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select top(11) min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "where (mem0.[IntegerValue] = @p0) " +
                "group by s.[StructureId] " +
                "order by mem0 Asc;",
                sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);
        }

        [Test]
        public override void GenerateQuery_WithPagingAndWhereAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithPagingAndWhereAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual("select min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0 " +
                            "from [MyClassStructure] s " +
                            "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                            "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                            "where (mem0.[IntegerValue] = @p0) " +
                            "group by s.[StructureId] " +
                            "order by mem0 Asc offset @offsetRows rows fetch next @takeRows rows only;",
                            sqlQuery.Sql);

            Assert.AreEqual("@p0", sqlQuery.Parameters[0].Name);
            Assert.AreEqual(42, sqlQuery.Parameters[0].Value);

            Assert.AreEqual("@offsetRows", sqlQuery.Parameters[1].Name);
            Assert.AreEqual(0, sqlQuery.Parameters[1].Value);
            
            Assert.AreEqual("@takeRows", sqlQuery.Parameters[2].Name);
            Assert.AreEqual(10, sqlQuery.Parameters[2].Value);
        }

        [Test]
        public override void GenerateQuery_WithExplicitSortingOnTwoDifferentMemberTypesAndSorting_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithExplicitSortingOnTwoDifferentMemberTypesAndSorting_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0, min(mem1.[StringValue]) mem1 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "inner join [MyClassIndexes] mem1 on mem1.[StructureId] = s.[StructureId] and mem1.[MemberPath] = 'String1' " +
                "group by s.[StructureId] " +
                "order by mem0 Asc, mem1 Desc;",
                sqlQuery.Sql);
        }

        [Test]
        public override void GenerateQuery_WithSortingOnTwoDifferentMemberOfSameType_GeneratesCorrectQuery()
        {
            var sqlQuery = On_GenerateQuery_WithSortingOnTwoDifferentMemberOfSameType_GeneratesCorrectQuery();

            Assert.AreEqual(
                "select min(s.[Json]) [Json], min(mem0.[IntegerValue]) mem0, min(mem1.[IntegerValue]) mem1 from [MyClassStructure] s " +
                "inner join [MyClassIndexes] si on si.[StructureId] = s.[StructureId] " +
                "inner join [MyClassIndexes] mem0 on mem0.[StructureId] = s.[StructureId] and mem0.[MemberPath] = 'Int1' " +
                "inner join [MyClassIndexes] mem1 on mem1.[StructureId] = s.[StructureId] and mem1.[MemberPath] = 'Int2' " +
                "group by s.[StructureId] " +
                "order by mem0 Asc, mem1 Asc;",
                sqlQuery.Sql);
        }
    }
}