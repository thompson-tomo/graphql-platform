using HotChocolate.Language;
using HotChocolate.Tests;
using HotChocolate.Types;
using HotChocolate.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Execution.DependencyInjection;

public class RequestExecutorBuilderExtensionsValidationTests
{
    [Fact]
    public void AddValidationVisitor_1_Builder_Is_Null()
    {
        void Fail() => RequestExecutorBuilderExtensions
            .AddValidationVisitor<MockVisitor>(null!);

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public void AddValidationVisitor_2_Builder_Is_Null()
    {
        void Fail() => RequestExecutorBuilderExtensions
            .AddValidationVisitor<MockVisitor>(
                null!,
                (_, _) => throw new NotImplementedException());

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public void AddValidationVisitor_2_Factory_Is_Null()
    {
        void Fail() => new ServiceCollection()
            .AddGraphQL()
            .AddValidationVisitor<MockVisitor>(null!);

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public void AddValidationRuler_1_Builder_Is_Null()
    {
        void Fail() => RequestExecutorBuilderExtensions
            .AddValidationRule<MockRule>(null!);

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public void AddValidationRule_2_Builder_Is_Null()
    {
        void Fail() => RequestExecutorBuilderExtensions
            .AddValidationRule<MockRule>(
                null!,
                (_, _) => throw new NotImplementedException());

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public void AddValidationRule_2_Factory_Is_Null()
    {
        void Fail() => new ServiceCollection()
            .AddGraphQL()
            .AddValidationRule<MockRule>(null!);

        Assert.Throws<ArgumentNullException>(Fail);
    }

    [Fact]
    public async Task AddIntrospectionAllowedRule_IntegrationTest_NotAllowed()
    {
        await new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
            .DisableIntrospection()
            .ExecuteRequestAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .Build())
            .MatchSnapshotAsync();
    }

    [Fact]
    public async Task AllowIntrospection_IntegrationTest_NotAllowed()
    {
        await new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
            .DisableIntrospection(disable: true)
            .ExecuteRequestAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .Build())
            .MatchSnapshotAsync();
    }

    [Fact]
    public async Task AllowIntrospection_IntegrationTest_Allowed()
    {
        await new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
            .DisableIntrospection(disable: false)
            .ExecuteRequestAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .Build())
            .MatchSnapshotAsync();
    }

    [Fact]
    public async Task AllowIntrospection_IntegrationTest_NotAllowed_CustomMessage()
    {
        await new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
            .DisableIntrospection()
            .ExecuteRequestAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .SetIntrospectionNotAllowedMessage("Bar")
                    .Build())
            .MatchSnapshotAsync();
    }

    [Fact]
    public async Task AddIntrospectionAllowedRule_IntegrationTest_NotAllowed_CustomMessage()
    {
        await new ServiceCollection()
            .AddGraphQLServer()
            .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
            .DisableIntrospection()
            .ExecuteRequestAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .SetIntrospectionNotAllowedMessage("Baz")
                    .Build())
            .MatchSnapshotAsync();
    }

    [Fact]
    public async Task AddIntrospectionAllowedRule_IntegrationTest_Allowed()
    {
        var executor =
            await new ServiceCollection()
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query").Field("foo").Resolve("bar"))
                .DisableIntrospection()
                .BuildRequestExecutorAsync();

        var results = new List<string>();

        var result =
            await executor.ExecuteAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .AllowIntrospection()
                    .Build());
        results.Add(result.ToJson());

        result =
            await executor.ExecuteAsync(
                OperationRequestBuilder
                    .New()
                    .SetDocument("{ __schema { description } }")
                    .Build());
        results.Add(result.ToJson());

        results.MatchSnapshot();
    }

    [Fact]
    public void SetMaxAllowedValidationErrors_Builder_Is_Null()
    {
        void Fail()
            => RequestExecutorBuilderExtensions.SetMaxAllowedValidationErrors(null!, 6);

        Assert.Throws<ArgumentNullException>(Fail);
    }

    public class MockVisitor : DocumentValidatorVisitor;

    public class MockRule : IDocumentValidatorRule
    {
        public ushort Priority => ushort.MaxValue;
        public bool IsCacheable => true;

        public void Validate(DocumentValidatorContext context, DocumentNode document)
        {
            throw new NotImplementedException();
        }
    }
}
