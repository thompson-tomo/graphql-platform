using HotChocolate.Fusion.Options;
using HotChocolate.Types.Mutable.Serialization;
using static HotChocolate.Fusion.CompositionTestHelper;

namespace HotChocolate.Fusion;

public sealed class SourceSchemaMergerArgumentTests
{
    [Theory]
    [MemberData(nameof(ExamplesData))]
    public void Examples(string[] sdl, string executionSchema)
    {
        // arrange
        var merger = new SourceSchemaMerger(
            CreateSchemaDefinitions(sdl),
            new SourceSchemaMergerOptions
            {
                RemoveUnreferencedTypes = false,
                AddFusionDefinitions = false
            });

        // act
        var result = merger.Merge();

        // assert
        Assert.True(result.IsSuccess);
        SchemaFormatter.FormatAsString(result.Value).MatchInlineSnapshot(executionSchema);
    }

    public static TheoryData<string[], string> ExamplesData()
    {
        return new TheoryData<string[], string>
        {
            // Consider two field definitions that share the same "filter" argument, but with
            // slightly different types and descriptions. In the merged schema, the "filter"
            // argument is defined with the most restrictive type ("ProductFilter!"), includes the
            // description from the original field in Schema A, and is marked as required.
            {
                [
                    """"
                    # Schema A
                    type Query {
                        searchProducts(
                            """
                            Filter to apply to the search
                            """
                            filter: ProductFilter!
                        ): [Product]
                    }
                    """",
                    """"
                    # Schema B
                    type Query {
                        searchProducts(
                            """
                            Search filter to apply
                            """
                            filter: ProductFilter
                        ): [Product]
                    }
                    """"
                ],
                """
                schema {
                    query: Query
                }

                type Query
                    @fusion__type(schema: A)
                    @fusion__type(schema: B) {
                    searchProducts("Filter to apply to the search" filter: ProductFilter!
                        @fusion__inputField(schema: A)
                        @fusion__inputField(schema: B, sourceType: "ProductFilter")): [Product]
                        @fusion__field(schema: A)
                        @fusion__field(schema: B)
                }

                scalar Product
                    @fusion__type(schema: A)
                    @fusion__type(schema: B)

                scalar ProductFilter
                    @fusion__type(schema: A)
                    @fusion__type(schema: B)
                """
            },
            // If any of the arguments is marked as @inaccessible, then the merged argument is also
            // marked as @inaccessible in the execution schema.
            {
                [
                    """
                    # Schema A
                    type Query {
                        field(limit: Int): Int
                    }
                    """,
                    """
                    # Schema B
                    type Query {
                        field(limit: Int @inaccessible): Int
                    }
                    """
                ],
                """
                schema {
                    query: Query
                }

                type Query
                    @fusion__type(schema: A)
                    @fusion__type(schema: B) {
                    field(limit: Int
                        @fusion__inputField(schema: A)
                        @fusion__inputField(schema: B)
                        @fusion__inaccessible): Int
                        @fusion__field(schema: A)
                        @fusion__field(schema: B)
                }
                """
            },
            // Suppose we have two variants of the same argument, "limit", from different services.
            {
                [
                    """
                    # Schema A
                    type Query {
                        field(limit: Int = 10): Int
                    }
                    """,
                    """"
                    # Schema B
                    type Query {
                        field(
                            """
                            Number of items to fetch
                            """
                            limit: Int!
                        ): Int
                    }
                    """"
                ],
                """
                schema {
                    query: Query
                }

                type Query
                    @fusion__type(schema: A)
                    @fusion__type(schema: B) {
                    field("Number of items to fetch" limit: Int! = 10
                        @fusion__inputField(schema: A, sourceType: "Int")
                        @fusion__inputField(schema: B)): Int
                        @fusion__field(schema: A)
                        @fusion__field(schema: B)
                }
                """
            },
            // @require
            {
                [
                    """
                    type Product {
                        id: ID!
                        dimension: ProductDimension!
                        delivery(
                            zip: String!
                            size: Int! @require(field: "dimension.size")
                            weight: Int! @require(field: "dimension.weight")
                        ): DeliveryEstimates
                    }

                    type ProductDimension {
                        size: Int!
                        weight: Int!
                    }
                    """
                ],
                """
                type Product
                    @fusion__type(schema: A) {
                    delivery(zip: String!
                        @fusion__inputField(schema: A)): DeliveryEstimates
                        @fusion__field(schema: A)
                        @fusion__requires(schema: A, requirements: "dimension { size weight }", field: "delivery(zip: String! size: Int! weight: Int!): DeliveryEstimates", map: [ null, "dimension.size", "dimension.weight" ])
                    dimension: ProductDimension!
                        @fusion__field(schema: A)
                    id: ID!
                        @fusion__field(schema: A)
                }

                type ProductDimension
                    @fusion__type(schema: A) {
                    size: Int!
                        @fusion__field(schema: A)
                    weight: Int!
                        @fusion__field(schema: A)
                }

                scalar DeliveryEstimates
                    @fusion__type(schema: A)
                """
            }
        };
    }
}
